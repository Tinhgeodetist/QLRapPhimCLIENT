using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QLRapChieuPhim.Common;
using QLRapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            LoginModel model = new();
            return View(model);
        }

        public IActionResult DangNhap(string username, string password, bool remember = false, string returnUrl = null)
        {
            if (username == null) username = "";
            if (password == null) password = "";

           
            var input = new NhanVienModel.Input.ThongTinDangNhap { TenDangNhap = username, MatKhau = password };
            var nhanvien = Utilities.SendDataRequest<NhanVienModel.Output.ThongTinNhanVien>(ConstantValues.NhanVien.DangNhap, input);
            HttpContext.Session.Remove("NhanVien");
            if (nhanvien != null)
            {
                if (nhanvien.Id > 0)
                {
                    bool logined = LoginUser(nhanvien, remember).Result;
                    if (logined)
                        HttpContext.Session.Set<NhanVienModel.Output.ThongTinNhanVien>("NhanVien", nhanvien);
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    TempData["ThongBaoLogin"] = nhanvien.ThongBao;
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            var nhanvien = HttpContext.Session.Get<NhanVienModel.Output.ThongTinNhanVien>("NhanVien");
            var input = new NhanVienModel.Input.ThongTinDangNhap { TenDangNhap = nhanvien.Cmnd, MatKhau = nhanvien.MatKhau };
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();            
            Utilities.SendDataRequest<bool>(ConstantValues.NhanVien.DangXuat, input);
            return Redirect("/");
        }

        public IActionResult AccessDenied()
        {
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
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ThongBaoLogin"] = null;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ChangePasswordModel model = new();
                var cmnd = User.Claims.FirstOrDefault(x => x.Type == "CMND").Value;
                var id = User.Claims.FirstOrDefault(x => x.Type == "NHANVIENID").Value;

                var input = new NhanVienModel.Input.ThongTinThayDoiMatKhau {
                    Id = int.Parse(id),
                    Username = cmnd,
                    MatKhauCu = matkhaucu,
                    MatKhauMoi = matkhaumoi
                };
                var thong_bao = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.NhanVien.DoiMatKhau, input);
                if (thong_bao.MaSo == 0)
                    ViewData["ThongBao"] = $"<span style='color: #0000ff;'>{thong_bao.NoiDung}</span>";
                else
                    ViewData["ThongBao"] = $"<span style='color: #ff0000;'>{thong_bao.NoiDung}</span>";
                return View(model);
            }

        }
                
        private async Task<bool> LoginUser(NhanVienModel.Output.ThongTinNhanVien nhanvien, bool remember)
        {
            try
            {
                var userClaims = new List<Claim>() {
                    new Claim("NHANVIENID", nhanvien.Id.ToString()),
                    new Claim("USERNAME", nhanvien.Cmnd),
                    new Claim("QUYENHAN", nhanvien.QuyenHan.ToUpper()),
                    new Claim("HOTEN", nhanvien.HoTen),
                    new Claim("CMND", nhanvien.Cmnd)
                };
                var claimsIdentity = new ClaimsIdentity(userClaims,
                                            CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);
                var properties = new AuthenticationProperties { IsPersistent = remember };
                await HttpContext.SignInAsync(principal, properties);

                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }
    }
}
