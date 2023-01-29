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
    public class PhongController : Controller
    {
        public IActionResult Index(int rapid = 0)
        {
            var dsRap = Utilities.SendDataRequest<List<RapModel.Output.ThongTinRap>>(ConstantValues.Rap.DanhSachRap);
            ViewData["DSRAP"] = dsRap;
            var dsPhong = new List<PhongModel.Output.ThongTinPhong>();
            if (dsRap.Count > 0)
            {
                PhongModel.Input.DanhSachPhongTheoRap input;
                if (rapid > 0)
                    input = new PhongModel.Input.DanhSachPhongTheoRap { RapId = rapid };
                else
                    input = new PhongModel.Input.DanhSachPhongTheoRap { RapId = dsRap.First().Id };
                dsPhong = Utilities.SendDataRequest<List<PhongModel.Output.ThongTinPhong>>(ConstantValues.Phong.DanhSachPhongTheoRap, input);
            }
            ViewData["RapId"] = rapid;
            return View(dsPhong);
        }

        public IActionResult ThemPhong(PhongModel.Input.ThongTinPhong input)
        {
            PhongModel.Output.ThongTinPhong phong = new();
            var dsRap = Utilities.SendDataRequest<List<RapModel.Output.ThongTinRap>>(ConstantValues.Rap.DanhSachRap);
            if (input != null && !string.IsNullOrEmpty(input.TenPhong))
            {                
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.Phong.ThemPhong, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                    Utilities.PropertyCopier<PhongModel.Input.ThongTinPhong, PhongModel.Output.ThongTinPhong>.Copy(input, phong);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            ViewData["DSRAP"] = dsRap;
            return View(phong);
        }

        public IActionResult CapNhatPhong(int id)
        {
            if (id > 0)
            {
                var input = new PhongModel.Input.DocThongTinPhong { Id = id };                
                var phong = Utilities.SendDataRequest<PhongModel.Output.ThongTinPhong>(ConstantValues.Phong.ThongTinPhong, input);
                if (phong != null && phong.Id > 0)
                {
                    var dsRap = Utilities.SendDataRequest<List<RapModel.Output.ThongTinRap>>(ConstantValues.Rap.DanhSachRap);
                    ViewData["DSRAP"] = dsRap;
                    return View(phong);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CapNhatPhong(PhongModel.Input.ThongTinPhong input)
        {
            PhongModel.Output.ThongTinPhong phong = new();
            if (input == null || input.RapId == 0) return RedirectToAction("Index");
            var dsRap = Utilities.SendDataRequest<List<RapModel.Output.ThongTinRap>>(ConstantValues.Rap.DanhSachRap);
            var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.Phong.CapNhatPhong, input);
            if (tb.MaSo > 0)
            {
                ViewData["ThongBao"] = tb.NoiDung;
                Utilities.PropertyCopier<PhongModel.Input.ThongTinPhong, PhongModel.Output.ThongTinPhong>.Copy(input, phong);
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewData["DSRAP"] = dsRap;
            return View(phong);
        }

        public IActionResult XoaPhong(int id)
        {
            if (id > 0)
            {
                var input = new PhongModel.Input.XoaPhong { Id = id };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.Phong.XoaPhong, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                }
            }
            return RedirectToAction("Index");
        }

        #region Xử lý ghế
        public IActionResult ThongTinGhe(int id)
        {
            if (id <= 0) return RedirectToAction("Index");
            var input = new ThongTinGheModel.Input.DanhSachThongTinGheTheoPhong { PhongId = id };
            var dsThongTinGhe = Utilities.SendDataRequest<List<ThongTinGheModel.Output.ThongTinGhe>>(ConstantValues.ThongTinGhe.DanhSachThongTinGheTheoPhong, input);
            var input_phong = new PhongModel.Input.ThongTinPhong { Id = id };
            var phong = Utilities.SendDataRequest<PhongModel.Output.ThongTinPhong>(ConstantValues.Phong.ThongTinPhong, input_phong);
            ViewData["Phong"] = phong;
            return View(dsThongTinGhe);
        }
        public IActionResult ThemThongTinGhe(int id)
        {
            if (id <= 0) return RedirectToAction("Index");
            var thongTinGhe = new ThongTinGheModel.Output.ThongTinGhe();
            var input_phong = new PhongModel.Input.ThongTinPhong { Id = id};
            var phong = Utilities.SendDataRequest<PhongModel.Output.ThongTinPhong>(ConstantValues.Phong.ThongTinPhong, input_phong);
            var dsLoaiGhe = Utilities.SendDataRequest<List<LoaiGheModel.Output.ThongTinLoaiGhe>>(ConstantValues.LoaiGhe.DanhSachLoaiGhe);
            ViewData["Phong"] = phong;
            ViewData["DSLoaiGhe"] = dsLoaiGhe;
            thongTinGhe.PhongId = id;
            return View(thongTinGhe);
        }
        [HttpPost]
        public IActionResult ThemThongTinGhe(ThongTinGheModel.Input.ThongTinGhe input)
        {
            var thongTinGhe = new ThongTinGheModel.Output.ThongTinGhe();
            if (input != null && !string.IsNullOrEmpty(input.KyHieu))
            {
                input.Id = 0;
                if (string.IsNullOrEmpty(input.ViTriGheVip)) input.ViTriGheVip = "";
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.ThongTinGhe.ThemThongTinGhe, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                    Utilities.PropertyCopier<ThongTinGheModel.Input.ThongTinGhe, ThongTinGheModel.Output.ThongTinGhe>.Copy(input, thongTinGhe);
                    var input_phong = new PhongModel.Input.ThongTinPhong { Id = input.PhongId };
                    var phong = Utilities.SendDataRequest<PhongModel.Output.ThongTinPhong>(ConstantValues.Phong.ThongTinPhong, input_phong);
                    var dsLoaiGhe = Utilities.SendDataRequest<List<LoaiGheModel.Output.ThongTinLoaiGhe>>(ConstantValues.LoaiGhe.DanhSachLoaiGhe);
                    ViewData["Phong"] = phong;
                    ViewData["DSLoaiGhe"] = dsLoaiGhe;
                }
                else
                {
                    return RedirectToAction("ThongTinGhe", new { id = input.PhongId});
                }
            }
            return View(thongTinGhe);
        }
        public IActionResult CapNhatThongTinGhe(int id)
        {
            if (id > 0)
            {
                var input = new ThongTinGheModel.Input.DocThongTinGhe { Id = id };
                var thongTinGhe = Utilities.SendDataRequest<ThongTinGheModel.Output.ThongTinGhe>(ConstantValues.ThongTinGhe.ChiTietThongTinGhe, input);
                if (thongTinGhe != null && thongTinGhe.Id > 0)
                {
                    var input_phong = new PhongModel.Input.ThongTinPhong { Id = thongTinGhe.PhongId };
                    var phong = Utilities.SendDataRequest<PhongModel.Output.ThongTinPhong>(ConstantValues.Phong.ThongTinPhong, input_phong);
                    var dsLoaiGhe = Utilities.SendDataRequest<List<LoaiGheModel.Output.ThongTinLoaiGhe>>(ConstantValues.LoaiGhe.DanhSachLoaiGhe);
                    ViewData["Phong"] = phong;
                    ViewData["DSLoaiGhe"] = dsLoaiGhe;
                    return View(thongTinGhe);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CapNhatThongTinGhe(ThongTinGheModel.Input.ThongTinGhe input)
        {
            ThongTinGheModel.Output.ThongTinGhe thongTin = new();
            if (input == null || input.PhongId == 0) return RedirectToAction("Index");
            if (string.IsNullOrEmpty(input.ViTriGheVip)) input.ViTriGheVip = "";
            var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.ThongTinGhe.CapNhatThongTinGhe, input);
            if (tb.MaSo > 0)
            {
                ViewData["ThongBao"] = tb.NoiDung;
                Utilities.PropertyCopier<ThongTinGheModel.Input.ThongTinGhe, ThongTinGheModel.Output.ThongTinGhe>.Copy(input, thongTin);
                var input_phong = new PhongModel.Input.ThongTinPhong { Id = thongTin.PhongId };
                var phong = Utilities.SendDataRequest<PhongModel.Output.ThongTinPhong>(ConstantValues.Phong.ThongTinPhong, input_phong);
                var dsLoaiGhe = Utilities.SendDataRequest<List<LoaiGheModel.Output.ThongTinLoaiGhe>>(ConstantValues.LoaiGhe.DanhSachLoaiGhe);
                ViewData["Phong"] = phong;
                ViewData["DSLoaiGhe"] = dsLoaiGhe;
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(thongTin);
        }
        public IActionResult XoaThongTinGhe(int id)
        {
            if (id > 0)
            {
                var input = new ThongTinGheModel.Input.XoaThongTinGhe { Id = id };
                var thongTinGhe = Utilities.SendDataRequest<ThongTinGheModel.Output.ThongTinGhe>(ConstantValues.ThongTinGhe.ChiTietThongTinGhe, input);
                var input_xoa = new ThongTinGheModel.Input.XoaThongTinGhe { Id = id };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.ThongTinGhe.XoaThongTinGhe, input_xoa);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                }
                return RedirectToAction("ThongTinGhe", new { id = thongTinGhe.Id });
            }
            return RedirectToAction("Index");
        }
        //Phát sinh vị trí các ghế ngồi theo id phòng chiếu
        public IActionResult PhatSinhGhe(int id)
        {
            var input_ghe = new GheModel.Input.DanhSachGheTheoPhong { PhongId = id };
            var dsGhe = Utilities.SendDataRequest<List<GheModel.Output.ThongTinGhe>>(ConstantValues.Ghe.DanhSachGheTheoPhong, input_ghe);
            if (dsGhe == null || dsGhe.Count == 0)
            {
                var input = new GheModel.Input.PhatSinhGheTheoPhong { PhongId = id };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.Ghe.PhatSinhGheTheoPhong, input);
                if (tb.MaSo > 0)
                    TempData["ThongBao"] = tb.NoiDung;
                else
                    TempData["ThongBao"] = "Đã phát sinh ghế thành công";
            }
            else
            {
                TempData["ThongBao"] = "Phòng này đã có ghế, không phát sinh được.";
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
