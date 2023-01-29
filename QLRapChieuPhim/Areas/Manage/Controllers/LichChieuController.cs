using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLRapChieuPhim.Models;
using QLRapChieuPhim.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class LichChieuController : Controller
    {
        public IActionResult Index(int rapid = 0)
        {
            var dsRap = Utilities.SendDataRequest<List<RapModel.Output.ThongTinRap>>(ConstantValues.Rap.DanhSachRap);
            ViewData["DSRAP"] = dsRap;
            var dsLichChieu = new List<LichChieuModel.Output.ThongTinLichChieu>();
            LichChieuModel.Input.DanhSachLichChieuTheoRap input = new();
            if (dsRap.Count > 0)
            {
                if (rapid > 0)
                    input = new LichChieuModel.Input.DanhSachLichChieuTheoRap { RapId = rapid };
                else
                    input = new LichChieuModel.Input.DanhSachLichChieuTheoRap { RapId = dsRap.First().Id };
                dsLichChieu = Utilities.SendDataRequest<List<LichChieuModel.Output.ThongTinLichChieu>>(ConstantValues.LichChieu.DanhSachLichChieuTheoRap, input);
            }
            ViewData["RapId"] = input.RapId;
            return View(dsLichChieu);
        }
        public IActionResult ThemLichChieu(int rapid)
        {
            LichChieuModel.Output.ThongTinLichChieu model = new()
            {
                RapId = rapid,
                NgayChieu = DateTime.Today.Date
            };
            var dsSuatChieu = Utilities.SendDataRequest<List<SuatChieuModel.Output.ThongTinSuatChieu>>(ConstantValues.SuatChieu.DanhSachSuatChieu);
            ViewData["DSSUATCHIEU"] = dsSuatChieu;
            var input = new PhongModel.Input.DanhSachPhongTheoRap { RapId = rapid };
            var dsPhong = Utilities.SendDataRequest<List<PhongModel.Output.ThongTinPhong>>(ConstantValues.Phong.DanhSachPhongTheoRap, input);
            ViewData["DSPHONG"] = dsPhong;
            var dsPhim = Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimDangChieu);
            ViewData["DSPHIM"] = dsPhim;
            return View(model);
        }
        [HttpPost]
        public IActionResult ThemLichChieu(LichChieuModel.Input.ThongTinLichChieu input)
        {
            var input_ = new PhongModel.Input.DanhSachPhongTheoRap { RapId = input.RapId };
            LichChieuModel.Output.ThongTinLichChieu lichchieu = new();
            var dsSuatChieu = Utilities.SendDataRequest<List<SuatChieuModel.Output.ThongTinSuatChieu>>(ConstantValues.SuatChieu.DanhSachSuatChieu);
            var dsPhong = Utilities.SendDataRequest<List<PhongModel.Output.ThongTinPhong>>(ConstantValues.Phong.DanhSachPhongTheoRap, input_);
            var dsPhim = Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimSapChieu);
            dsPhim.AddRange(Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimDangChieu));
            if (input != null)
            {
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.LichChieu.ThemLichChieu, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                    Utilities.PropertyCopier<LichChieuModel.Input.ThongTinLichChieu, LichChieuModel.Output.ThongTinLichChieu>.Copy(input, lichchieu);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            ViewData["DSSUATCHIEU"] = dsSuatChieu;
            ViewData["DSPHONG"] = dsPhong;
            ViewData["DSPHIM"] = dsPhim;
            return View(lichchieu);
        }
        public IActionResult XoaLichChieu(int id)
        {
            if (id > 0)
            {
                var input = new LichChieuModel.Input.XoaLichChieu { Id = id };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.LichChieu.XoaLichChieu, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult CapNhatLichChieu(int id, int rapid)
        {
            if (id > 0)
            {
                var input = new LichChieuModel.Input.DocThongTinLichChieu { Id = id };
                var lichchieu = Utilities.SendDataRequest<LichChieuModel.Output.ThongTinLichChieu>(ConstantValues.LichChieu.ThongTinLichChieu, input);
                if (lichchieu != null && lichchieu.Id > 0)
                {
                    var dsSuatChieu = Utilities.SendDataRequest<List<SuatChieuModel.Output.ThongTinSuatChieu>>(ConstantValues.SuatChieu.DanhSachSuatChieu);
                    var input_ = new PhongModel.Input.DanhSachPhongTheoRap { RapId = rapid };
                    var dsPhong = Utilities.SendDataRequest<List<PhongModel.Output.ThongTinPhong>>(ConstantValues.Phong.DanhSachPhongTheoRap, input_);
                    var dsPhim = Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimSapChieu);
                    dsPhim.AddRange(Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimDangChieu));
                    ViewData["DSSUATCHIEU"] = dsSuatChieu;
                    ViewData["DSPHONG"] = dsPhong;
                    ViewData["DSPHIM"] = dsPhim;
                    return View(lichchieu);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CapNhatLichChieu(LichChieuModel.Input.ThongTinLichChieu input)
        {
            dynamic input_ = new PhongModel.Input.DocThongTinPhong { Id = input.PhongId };
            var phong = Utilities.SendDataRequest<PhongModel.Output.ThongTinPhong>(ConstantValues.Phong.ThongTinPhong, input_);
            input_ = new PhongModel.Input.DanhSachPhongTheoRap { RapId = phong.RapId };
            LichChieuModel.Output.ThongTinLichChieu lichchieu = new();
            var dsSuatChieu = Utilities.SendDataRequest<List<SuatChieuModel.Output.ThongTinSuatChieu>>(ConstantValues.SuatChieu.DanhSachSuatChieu);
            var dsPhong = Utilities.SendDataRequest<List<PhongModel.Output.ThongTinPhong>>(ConstantValues.Phong.DanhSachPhongTheoRap, input_);
            var dsPhim = Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimSapChieu);
            dsPhim.AddRange(Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimDangChieu));
            if (input == null || input.PhimId == 0) return RedirectToAction("Index");
            var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.LichChieu.CapNhatLichChieu, input);
            if (tb.MaSo > 0)
            {
                ViewData["ThongBao"] = tb.NoiDung;
                Utilities.PropertyCopier<LichChieuModel.Input.ThongTinLichChieu, LichChieuModel.Output.ThongTinLichChieu>.Copy(input, lichchieu);
            }
            else
            {
                return RedirectToAction("Index");
            }
            ViewData["DSSUATCHIEU"] = dsSuatChieu;
            ViewData["DSPHONG"] = dsPhong;
            ViewData["DSPHIM"] = dsPhim;
            return View(lichchieu);
        }
    }
}
