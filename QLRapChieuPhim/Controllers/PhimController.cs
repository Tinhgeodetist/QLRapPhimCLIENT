using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLRapChieuPhim.Common;
using QLRapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Controllers
{
    public class PhimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PhimDangChieu()
        {
            if(HttpContext.Session.Get<ThanhVienModel.Output.ThongTinThanhVien>("ThanhVien") == null)
                return RedirectToAction("Index", "Home");
            var model = Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimDangChieu);
            ViewData["loaiPhim"] = "PhimDangChieu";
            return View("DanhSachPhim", model);
        }

        public IActionResult PhimSapChieu()
        {
            if (HttpContext.Session.Get<ThanhVienModel.Output.ThongTinThanhVien>("ThanhVien") == null)
                return RedirectToAction("Index", "Home");
            var model = Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimSapChieu);
            ViewData["loaiPhim"] = "PhimSapChieu";
            return View("DanhSachPhim", model);
        }

        public IActionResult ThongTinPhim(int id)
        {
            if (HttpContext.Session.Get<ThanhVienModel.Output.ThongTinThanhVien>("ThanhVien") == null)
                return RedirectToAction("Index", "Home");
            if (id > 0)
            {
                PhimModel.Input.DocThongTinPhim input = new() { PhimId = id };
                var thongTinPhim = Utilities.SendDataRequest<PhimModel.Output.ThongTinPhim>(ConstantValues.Phim.ThongTinPhim, input);
                return View(thongTinPhim);
            }
            RedirectToAction("DanhSachPhim");
            return View();
        }
        [Authorize]
        public IActionResult MuaVe(int id, string ngaychieu)
        {
            //id: id của phim đang chọn
            if (HttpContext.Session.Get<ThanhVienModel.Output.ThongTinThanhVien>("ThanhVien") == null)
                return RedirectToAction("Index", "Home");
            //Lấy thông tin Phim
            DateTime ngayChieuPhim = DateTime.Today.Date;
            if (!string.IsNullOrEmpty(ngaychieu))
                DateTime.TryParse(ngaychieu, out ngayChieuPhim);
            ViewData["NgayChieu"] = ngayChieuPhim;
            var input_phim = new PhimModel.Input.DocThongTinPhim { PhimId = id };
            var phim = Utilities.SendDataRequest<PhimModel.Output.ThongTinPhim>(ConstantValues.Phim.ThongTinPhim, input_phim);
            if(phim == null || string.IsNullOrEmpty(phim.TenPhim)) return RedirectToAction("Index", "Home");
            ViewData["Phim"] = phim;
            //Lấy danh sách lịch chiếu của ngày hiện hành
            LichChieuModel.Input.DanhSachLichChieuTheoPhimVaNgay input = new() { PhimId = id, NgayChieu = ngayChieuPhim };
            var dsLichChieu = Utilities.SendDataRequest<List<LichChieuModel.Output.ThongTinLichChieu>>(ConstantValues.LichChieu.DanhSachLichChieuTheoPhimVaNgay, input);
            if (dsLichChieu != null && dsLichChieu.Count > 0)
            {
                var dsRap = dsLichChieu.Select(x => x.Phong.Rap).GroupBy(x=> x.Id).Select(x=>x.FirstOrDefault()).ToList();
                ViewData["DSRap"] = dsRap;
            }
            else
                ViewData["DSRap"] = new List<PhongModel.RapInfo>();
            return View(dsLichChieu);
        }
        [Authorize]
        public IActionResult ChonViTriGhe(LichChieuModel.Input.ThongTinLichChieu input)
        {
            //Lấy thông tin Phim
            var input_lichchieu = new LichChieuModel.Input.DocThongTinLichChieu { Id = input.Id };
            var lichChieu = Utilities.SendDataRequest<LichChieuModel.Output.ThongTinLichChieu>(ConstantValues.LichChieu.ThongTinLichChieu, input_lichchieu);
            //var input_phim = new PhimModel.Input.DocThongTinPhim { PhimId = input.PhimId };
            //var phim = Utilities.SendDataRequest<PhimModel.Output.ThongTinPhim>(ConstantValues.Phim.ThongTinPhim, input_phim);
            if (lichChieu == null || lichChieu.Id <= 0) return RedirectToAction("Index", "Home");
            lichChieu.TenRap = input.TenRap;
            ViewData["LichChieu"] = lichChieu;
            var input_ghe = new GheModel.Input.DanhSachGheTheoPhong { PhongId = lichChieu.PhongId };
            var dsGhe = Utilities.SendDataRequest<List<GheModel.Output.ThongTinGhe>>(ConstantValues.Ghe.DanhSachGheTheoPhong, input_ghe);
            var input_ve = new VeModel.Input.DocDanhSachVeTheoLichChieu { LichChieuId = lichChieu.Id };
            var dsVeKhongChon = Utilities.SendDataRequest<List<VeModel.Output.ThongTinVe>>(ConstantValues.Ve.DanhSachVeKhongDuocMuaTheoLichChieu, input_ve);
            ViewData["dsVeKhongChon"] = dsVeKhongChon;
            return View(dsGhe);
        }
        [Authorize]
        public IActionResult XacNhanMuaVe(string danhsachidghe, int lichchieuid)
        {
            var tb = new ThongBaoModel { MaSo = 0, NoiDung = "" };
            if(string.IsNullOrEmpty(danhsachidghe) || lichchieuid <=0)
            {
                tb.MaSo = 1;
                tb.NoiDung = "Thông tin mua vé không hợp lệ.";
            }
            else
            {
                var dsId = danhsachidghe.Split(',').Select(x => int.Parse(x)).ToList();
                var input_ghe = new GheModel.Input.DanhSachGheTheoDanhSachId { DanhSachId = dsId };
                var dsGhe = Utilities.SendDataRequest<List<GheModel.Output.ThongTinGhe>>(ConstantValues.Ghe.DanhSachGheTheoDanhSachId, input_ghe);
                if (dsGhe.Count != dsId.Count)
                {
                    tb.MaSo = 1;
                    tb.NoiDung = "Thông tin mua vé không hợp lệ.";
                }
                else
                {
                    var input_ve = new VeModel.Input.MuaVe();
                    var sove = "";
                    foreach(var ghe in dsGhe)
                    {
                        var vemua = new VeModel.Input.ThongTinVe
                        {
                            Id = 0,
                            GheId = ghe.Id,
                            GiaVe = ghe.GheVip ? (int)(ghe.LoaiGhe.GiaVe * 1.5) : ghe.LoaiGhe.GiaVe,
                            LichChieuId = lichchieuid,
                            NgayBanVe = DateTime.Today.Date,
                            NhanVienId = 0,
                            SoGhe = ghe.SoGhe,
                            TinhTrang = 2
                        };
                        input_ve.DanhSachVe.Add(vemua);
                        sove += ghe.SoGhe + ", ";
                    }
                    tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.Ve.MuaVe, input_ve);
                    var input_lich = new LichChieuModel.Input.DocThongTinLichChieu { Id = lichchieuid };
                    var lichChieu = Utilities.SendDataRequest<LichChieuModel.Output.ThongTinLichChieu>(ConstantValues.LichChieu.ThongTinLichChieu, input_lich);
                    ViewData["LichChieu"] = lichChieu;
                    if (sove != "")
                        sove = sove.Substring(0, sove.Length - 2);
                    ViewData["SoVe"] = sove;
                }
            }
            
            return View(tb);
        }
    }
}
