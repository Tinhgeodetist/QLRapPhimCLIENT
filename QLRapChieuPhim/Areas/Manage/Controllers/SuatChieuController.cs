using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLRapChieuPhim.Common;
using QLRapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SuatChieuController : Controller
    {
        public IActionResult Index()
        {
            var dsSuatChieu = Utilities.SendDataRequest<List<SuatChieuModel.Output.ThongTinSuatChieu>>(ConstantValues.SuatChieu.DanhSachSuatChieu);
            return View(dsSuatChieu);
        }

        public IActionResult DanhSachSuatChieu()
        {
            var dsSuatChieu = Utilities.SendDataRequest<List<SuatChieuModel.Output.ThongTinSuatChieu>>(ConstantValues.SuatChieu.DanhSachSuatChieu);
            return View(dsSuatChieu);
        }

        public IActionResult ThemSuatChieu()
        {
            SuatChieuModel.Output.ThemSuatChieu model = new();
            return View(model);
        }

        [HttpPost]
        public IActionResult ThemSuatChieu(IFormCollection form)
        {
            SuatChieuModel.Output.ThemSuatChieu model = new SuatChieuModel.Output.ThemSuatChieu();
            try
            {
                if (string.IsNullOrEmpty(form["TenSuatChieu"]))
                    model.ThongBao = "<p>- Tên suất chiếu phải khác rỗng</p>";
                if (string.IsNullOrEmpty(form["GioBatDau"]))
                    model.ThongBao += "<p>- Giờ bắt đầu phải khác rỗng</p>";
                else
                {
                    var batdau = form["GioBatDau"].ToString().Split(':');
                    if (batdau.Length != 2 || !batdau[0].All(char.IsDigit) || !batdau[1].All(char.IsDigit))
                        model.ThongBao += "<p>- Giờ bắt đầu không hợp lệ</p>";
                    else if (int.Parse(batdau[0]) < 8 || int.Parse(batdau[0]) > 24 || int.Parse(batdau[1]) < 0 || int.Parse(batdau[1]) > 59)
                        model.ThongBao += "<p>- Giờ bắt đầu không hợp lệ</p>";
                }
                if (string.IsNullOrEmpty(model.ThongBao))
                {
                    var suatChieuMoi = new SuatChieuModel.Output.ThemSuatChieu
                    {
                        TenSuatChieu = form["TenSuatChieu"].ToString(),
                        GioBatDau = form["GioBatDau"].ToString(),
                        GioKetThuc = form["GioKetThuc"].ToString()
                    };
                    var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.SuatChieu.ThemSuatChieu, suatChieuMoi);
                    if (tb.MaSo == 0) return RedirectToAction("Index");
                    model.ThongBao = tb.NoiDung;
                }
            }
            catch (Exception ex)
            {
                model.ThongBao = "Có lỗi xảy ra: " + ex.Message;
            }
            return View(model);
        }

        public IActionResult CapNhatSuatChieu(int id)
        {
            if (id > 0)
            {
                var input = new SuatChieuModel.Input.DocThongTinSuatChieu { Id = id };
                var suatChieu = Utilities.SendDataRequest<SuatChieuModel.Output.CapNhatSuatChieu>(ConstantValues.SuatChieu.ThongTinSuatChieu, input);
                if (suatChieu != null && suatChieu.Id > 0)
                {
                    return View(suatChieu);
                }
            }
            RedirectToAction("DanhSachSuatChieu");
            return View();
        }
        [HttpPost]
        public IActionResult CapNhatSuatChieu(IFormCollection form)
        {
            SuatChieuModel.Output.CapNhatSuatChieu model = new();
            try
            {
                if (string.IsNullOrEmpty(form["TenSuatChieu"]))
                    model.ThongBao = "<p>- Tên suất chiếu phải khác rỗng</p>";
                if (string.IsNullOrEmpty(form["GioBatDau"]))
                    model.ThongBao += "<p>- Giờ bắt đầu phải khác rỗng</p>";
                else
                {
                    var batdau = form["GioBatDau"].ToString().Split(':');
                    if (batdau.Length != 2 || !batdau[0].All(char.IsDigit) || !batdau[1].All(char.IsDigit))
                        model.ThongBao += "<p>- Giờ bắt đầu không hợp lệ</p>";
                    else if (int.Parse(batdau[0]) < 8 || int.Parse(batdau[0]) > 24 || int.Parse(batdau[1]) < 0 || int.Parse(batdau[1]) > 59)
                        model.ThongBao += "<p>- Giờ bắt đầu không hợp lệ</p>";
                }
                if (string.IsNullOrEmpty(model.ThongBao))
                {
                    var suatChieuCapNhat = new SuatChieuModel.Output.CapNhatSuatChieu
                    {
                        Id = int.Parse(form["Id"].ToString()),
                        TenSuatChieu = form["TenSuatChieu"].ToString(),
                        GioBatDau = form["GioBatDau"].ToString(),
                        GioKetThuc = form["GioKetThuc"].ToString()
                    };
                    var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.SuatChieu.CapNhatSuatChieu, suatChieuCapNhat);
                    model.ThongBao = tb.NoiDung;
                }
            }
            catch (Exception ex)
            {
                model.ThongBao = "Có lỗi xảy ra: " + ex.Message;
            }

            model.Id = int.Parse(form["Id"].ToString());
            model.TenSuatChieu = form["TenSuatChieu"].ToString();
            model.GioBatDau = form["GioBatDau"].ToString();
            model.GioKetThuc = form["GioKetThuc"].ToString();
            return View(model);
        }
        public IActionResult XoaSuatChieu(int id)
        {
            if (id > 0)
            {
                var input = new SuatChieuModel.Input.XoaSuatChieu { Id = id };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.SuatChieu.XoaSuatChieu, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                }
            }
            return RedirectToAction("Index");
        }
    }
}
