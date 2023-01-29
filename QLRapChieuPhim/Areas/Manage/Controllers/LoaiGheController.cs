using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLRapChieuPhim.Models;
using QLRapChieuPhim.Common;

namespace QLRapChieuPhim.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class LoaiGheController : Controller
    {
        public IActionResult Index()
        {
            var dsLoaiGhe = Utilities.SendDataRequest<List<LoaiGheModel.Output.ThongTinLoaiGhe>>(ConstantValues.LoaiGhe.DanhSachLoaiGhe);
            return View(dsLoaiGhe);
        }
        public IActionResult ThemLoaiGhe()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ThemLoaiGhe(LoaiGheModel.Input.ThongTinLoaiGhe input)
        {
            LoaiGheModel.Output.ThongTinLoaiGhe loaighe = new();
            if (input != null)
            {
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.LoaiGhe.ThemLoaiGhe, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                    Utilities.PropertyCopier<LoaiGheModel.Input.ThongTinLoaiGhe, LoaiGheModel.Output.ThongTinLoaiGhe>.Copy(input, loaighe);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(loaighe);
        }
        public IActionResult XoaLoaiGhe(int id)
        {
            if (id > 0)
            {
                var input = new LoaiGheModel.Input.XoaLoaiGhe { Id = id };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.LoaiGhe.XoaLoaiGhe, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult CapNhatLoaiGhe(int id)
        {
            if (id > 0)
            {
                var input = new LoaiGheModel.Input.DocThongTinLoaiGhe { Id = id };
                var loaighe = Utilities.SendDataRequest<LoaiGheModel.Output.ThongTinLoaiGhe>(ConstantValues.LoaiGhe.ThongTinLoaiGhe, input);
                if (loaighe != null && loaighe.Id > 0)
                {
                    return View(loaighe);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CapNhatLoaiGhe(LoaiGheModel.Input.ThongTinLoaiGhe input)
        {
            LoaiGheModel.Output.ThongTinLoaiGhe loaighe = new();
            if (input == null) return RedirectToAction("Index");
            var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.LoaiGhe.CapNhatLoaiGhe, input);
            if (tb.MaSo > 0)
            {
                ViewData["ThongBao"] = tb.NoiDung;
                Utilities.PropertyCopier<LoaiGheModel.Input.ThongTinLoaiGhe, LoaiGheModel.Output.ThongTinLoaiGhe>.Copy(input, loaighe);
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(loaighe);
        }
    }
}
