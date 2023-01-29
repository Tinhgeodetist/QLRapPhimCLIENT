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
    public class RapController : Controller
    {
        public IActionResult Index()
        {
            var dsRap = Utilities.SendDataRequest<List<RapModel.Output.ThongTinRap>>(ConstantValues.Rap.DanhSachRap);
            return View(dsRap);
        }
        public IActionResult ThemRap(RapModel.Input.ThongTinRap input)
        {
            RapModel.Output.ThongTinRap rap = new();
            var input_nv = new NhanVienModel.Input.DanhSachNhanVien { QuanTri = true };
            var dsNhanVien = Utilities.SendDataRequest<List<NhanVienModel.Output.ThongTinNhanVien>>(ConstantValues.NhanVien.DanhSachNhanVien, input_nv);
            if (input != null && !string.IsNullOrEmpty(input.TenRap))
            {
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.Rap.ThemRap, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                    Utilities.PropertyCopier<RapModel.Input.ThongTinRap, RapModel.Output.ThongTinRap>.Copy(input, rap);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            ViewData["DSNhanVien"] = dsNhanVien;
            return View(rap);
        }

        public IActionResult CapNhatRap(int id)
        {
            if (id > 0)
            {
                var input = new RapModel.Input.DocThongTinRap { Id = id };
                var rap = Utilities.SendDataRequest<RapModel.Output.ThongTinRap>(ConstantValues.Rap.ThongTinRap, input);
                var input_nv = new NhanVienModel.Input.DanhSachNhanVien { QuanTri = true };
                var dsNhanVien = Utilities.SendDataRequest<List<NhanVienModel.Output.ThongTinNhanVien>>(ConstantValues.NhanVien.DanhSachNhanVien, input_nv);
                ViewData["DSNhanVien"] = dsNhanVien;
                if (rap != null && rap.Id > 0)
                {
                    return View(rap);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CapNhatRap(RapModel.Input.ThongTinRap input)
        {
            RapModel.Output.ThongTinRap rap = new();
            if (input == null || string.IsNullOrEmpty(input.TenRap)) return RedirectToAction("Index");
            var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.Rap.CapNhatRap, input);
            if (tb.MaSo > 0)
            {
                var input_nv = new NhanVienModel.Input.DanhSachNhanVien { QuanTri = true };
                var dsNhanVien = Utilities.SendDataRequest<List<NhanVienModel.Output.ThongTinNhanVien>>(ConstantValues.NhanVien.DanhSachNhanVien, input_nv);
                ViewData["DSNhanVien"] = dsNhanVien;
                ViewData["ThongBao"] = tb.NoiDung;
                Utilities.PropertyCopier<RapModel.Input.ThongTinRap, RapModel.Output.ThongTinRap>.Copy(input, rap);
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(rap);
        }

        public IActionResult XoaRap(int id)
        {
            if (id > 0)
            {
                var input = new RapModel.Input.XoaRap { Id = id };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.Rap.XoaRap, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                }
            }
            return RedirectToAction("Index");
        }
    }
}
