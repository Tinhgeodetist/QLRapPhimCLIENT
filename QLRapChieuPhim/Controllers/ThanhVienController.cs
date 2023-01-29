using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLRapChieuPhim.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using QLRapChieuPhim.Models;
using Google.Protobuf.WellKnownTypes;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;

namespace QLRapChieuPhim.Controllers
{
    public class ThanhVienController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            TempData["ThongBaoLogin"] = " ";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DangNhap(string username, string password, bool remember = false)
        {
            if (username == null) username = "";
            if (password == null) password = "";
            var input = new ThanhVienModel.Input.ThongTinDangNhap
            {
                TenDangNhap = username,
                MatKhau = password
            };
            var thanhvien = Utilities.SendDataRequest<ThanhVienModel.Output.ThongTinThanhVien>(ConstantValues.ThanhVien.DangNhap, input);
            HttpContext.Session.Remove("ThanhVien");
            if (thanhvien != null) {
                if (thanhvien.Id > 0) {
                    bool logined = LoginUser(thanhvien, remember).Result;
                    if (logined)
                        HttpContext.Session.Set<ThanhVienModel.Output.ThongTinThanhVien>("ThanhVien", thanhvien);
                }
                else
                    TempData["ThongBaoLogin"] = thanhvien.ThongBao;
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            var thanhvien = HttpContext.Session.Get<ThanhVienModel.Output.ThongTinThanhVien>("ThanhVien");
            var input = new ThanhVienModel.Input.ThongTinDangNhap { TenDangNhap = thanhvien.Email, MatKhau = thanhvien.MatKhau };
            bool signout = Utilities.SendDataRequest<bool>(ConstantValues.ThanhVien.DangXuat, input);
            if (signout)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
            }
            return Redirect("~/Home/Index");
            
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register()
        {
            ThanhVienModel.Output.ThongTinThanhVienMoi thanhvien = new();
            return View(thanhvien);
        }
        [HttpPost]
        public IActionResult Register(ThanhVienModel.Input.ThongTinThanhVienMoi thanhvienmoi) 
        {
            if (ModelState.IsValid)
            {
                //var thanhviendangky = new Input.Types.ThongTinDangKy();
                //Utilities.PropertyCopier<ThanhVienModel.Input.ThongTinThanhVienMoi, 
                //                         Input.Types.ThongTinDangKy>.Copy(thanhvien, thanhviendangky);
                if (thanhvienmoi.NgaySinh.Year < 1900)
                    thanhvienmoi.NgaySinh = new DateTime(1900, 1, 1);

                //var tb = client.DangKyThanhVien(thanhviendangky);
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.ThanhVien.DangKyThanhVien, thanhvienmoi);
                if (tb.MaSo == 0)
                {
                    thanhvienmoi.Id = int.Parse(tb.NoiDung);
                    TempData["EmailDangKy"] = thanhvienmoi.Email;
                    var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(thanhvienmoi.Email));
                    //Tạo link xác nhận kích hoạt tài khoản thành viên
                    var callbackUrl = Url.ActionLink("ConfirmEmail", "ThanhVien", 
                                                    new { area = "", code = code }, Request.Scheme);
                    //Tạo nội dung Email 
                    var message = @"<div style='margin:4px 0;font-family:Arial,Helvetica,sans-serif;font-size:14px;color:#444;line-height:18px;font-weight:normal'>Chào <b>" + thanhvienmoi.HoTen + "</b>,<br/><br/></div>" +
                                "<div>Cám ơn bạn đã đăng ký thành viên tại CSC Cinema.<br/><br/> </div>" +
                                "<div>Để hoàn tất đăng ký, bạn vui lòng <b><a style='text-decoration: none;' href='" + callbackUrl + "'><span style='background-color: #ff6600 !important; color: #ffffff; padding: 5px 10px; border-radius: 3px;'>Nhấn vào đây</span></a></b><br/><br/></div>";
                    Utilities.SendMail("Xác nhận đăng ký thành viên", message, thanhvienmoi.Email);
                    return RedirectToAction("RegisterSuccess", "ThanhVien");
                }
                else
                    ViewData["ThongBaoDangKy"] = tb.NoiDung;
            }
            var model = new ThanhVienModel.Output.ThongTinThanhVienMoi();
            Utilities.PropertyCopier<ThanhVienModel.Input.ThongTinThanhVienMoi, ThanhVienModel.Output.ThongTinThanhVienMoi>.Copy(thanhvienmoi, model);
            return View(model);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        public IActionResult ConfirmEmail(string code)
        {
            var email = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var input = new ThanhVienModel.Input.KichHoatTaiKhoan { Email = email };
            var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.ThanhVien.KichHoatTaiKhoan, input);
            if (tb.MaSo == 0) // kích hoạt thành công
            {
                ViewData["ThongBaoKichHoat"] = "thanhcong";
            }
            else // kích hoạt bị lỗi
            {
                ViewData["ThongBaoKichHoat"] = "thatbai";
            }
            return View();
        }

        public IActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ThongBaoLogin"] = null;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ChangePasswordModel model = new();
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult ChangePassword(string matkhaucu, string matkhaumoi)
        {
            if (!User.Identity.IsAuthenticated) {
                TempData["ThongBaoLogin"] = null;
                return RedirectToAction("Index", "Home");
            }
            else {
                ChangePasswordModel model = new();
                var email = User.Claims.FirstOrDefault(x => x.Type == "EMAIL").Value;
                var id = User.Claims.FirstOrDefault(x => x.Type == "THANHVIENID").Value;
                
                var input = new ThanhVienModel.Input.ThongTinThayDoiMatKhau
                {
                    Id = int.Parse(id),
                    Username = email,
                    MatKhauCu = matkhaucu,
                    MatKhauMoi = matkhaumoi
                };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.ThanhVien.DoiMatKhau, input);
                if (tb.MaSo == 0) 
                    ViewData["ThongBao"] = $"<span style='color: #0000ff;'>{tb.NoiDung}</span>";
                else 
                    ViewData["ThongBao"] = $"<span style='color: #ff0000;'>{tb.NoiDung}</span>";
                return View(model);
            }

        }

        private async Task<bool> LoginUser(ThanhVienModel.Output.ThongTinThanhVien thanhvien, bool remember)
        {
            try {
                var userClaims = new List<Claim>() {
                    new Claim("THANHVIENID", thanhvien.Id.ToString()),
                    new Claim("USERNAME", thanhvien.Email),
                    new Claim("QUYENHAN", "ThanhVien"),
                    new Claim("HOTEN", thanhvien.HoTen),
                    new Claim("EMAIL", thanhvien.Email)
                };
                var claimsIdentity = new ClaimsIdentity(userClaims, 
                                            CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);
                var properties = new AuthenticationProperties { IsPersistent = remember };
                await HttpContext.SignInAsync(principal, properties);

                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        
    }
}
