using Google.Protobuf.WellKnownTypes;
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
    public class PhimController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DanhSachPhim(int TheLoaiId, int CurrentPage, int PageSize)
        {
            if (TheLoaiId == 0) TheLoaiId = 1;
            var input = new PhimModel.Input.PhimTheoTheLoai {
                TheLoaiId = TheLoaiId,
                CurrentPage = CurrentPage,
                PageSize = PageSize
            };
            PhimModel.Output.PhimTheoTheLoai model = new PhimModel.Output.PhimTheoTheLoai();
            model = Utilities.SendDataRequest<PhimModel.Output.PhimTheoTheLoai>(ConstantValues.Phim.PhimTheoTheLoai, input);
                        
            return View("DanhSachPhim", model);
        }
        
        public IActionResult ThongTinPhim(int id)
        {
            if (id > 0)
            {
                var input = new PhimModel.Input.DocThongTinPhim { PhimId = id };
                var thongTinPhim = Utilities.SendDataRequest<PhimModel.Output.ThongTinPhim>(ConstantValues.Phim.ThongTinPhim, input);
                
                if (thongTinPhim != null && thongTinPhim.Id > 0)
                {
                    return View(thongTinPhim);
                }
            }
            RedirectToAction("DanhSachPhim");
            return View();
        }

        public IActionResult ThemPhimMoi()
        {
            PhimModel.Output.ThemPhimMoi model = new PhimModel.Output.ThemPhimMoi();
            var chuoi_dich_vu = "api/Phim/DocDanhSachTheLoai";
            var theLoais = Utilities.SendDataRequest<List<TheLoaiPhimModel.Output.ThongTinTheLoaiPhim>>(chuoi_dich_vu);
            chuoi_dich_vu = "api/Phim/DocDanhSachXepHangPhim";
            var xepHangPhims = Utilities.SendDataRequest<List<XepHangPhimModel.Output.ThongTinXepHangPhim>>(chuoi_dich_vu);
            model.DanhSachXepHangPhim = xepHangPhims.Select(x => new XepHangPhimModel.XepHangPhimBase
            {
                Id = x.Id,
                Ten = x.Ten,
                KyHieu = x.KyHieu
            }).ToList();
            model.DanhSachTheLoai = theLoais.Select(x => new TheLoaiPhimModel.TheLoaiPhimBase { Id = x.Id, Ten = x.Ten }).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult ThemPhimMoi(IFormCollection form)
        {
            PhimModel.Output.ThemPhimMoi model = new PhimModel.Output.ThemPhimMoi();
            var chuoi_dich_vu = "";
            try
            {
                if (string.IsNullOrEmpty(form["TenPhim"]))
                    model.ThongBao = "<p>- Tên phim phải khác rỗng</p>";
                if (string.IsNullOrEmpty(form["ThoiLuong"]) || int.Parse(form["ThoiLuong"]) <= 0)
                    model.ThongBao += "<p>- Thời lượng phim phải > 0</p>";
                if (string.IsNullOrEmpty(model.ThongBao))
                {
                    var phimThemMoi = new PhimModel.Output.ThemPhimMoi()
                    {
                        TenPhim = form["TenPhim"].ToString(),
                        TenGoc = form["TenGoc"].ToString(),
                        DienVien = form["DienVien"].ToString(),
                        DaoDien = form["DaoDien"].ToString(),
                        NgonNgu = form["NgonNgu"].ToString(),
                        NgayKhoiChieu = string.IsNullOrEmpty(form["NgayKhoiChieu"]) ? new DateTime(1900, 1, 1) : DateTime.Parse(form["NgayKhoiChieu"]),
                        NhaSanXuat = form["NhaSanXuat"].ToString(),
                        NuocSanXuat = form["NuocSanXuat"].ToString(),
                        NoiDung = form["NoiDung"].ToString(),
                        Poster = form["Poster"].ToString(),
                        ThoiLuong = int.Parse(form["ThoiLuong"].ToString()),
                        Trailer = form["Trailer"].ToString(),
                        XepHangPhimId = int.Parse(form["XepHangPhimId"].ToString()),
                        DanhSachTheLoaiId = form["DanhSachTheLoaiId"].ToString()
                    };
                    chuoi_dich_vu = "api/QuanTri/ThemPhimMoi";
                    var tb = Utilities.SendDataRequest<ThongBaoModel>(chuoi_dich_vu, phimThemMoi);
                    model.ThongBao = "";
                }
            }
            catch (Exception ex)
            {
                model.ThongBao = "Có lỗi xảy ra: " + ex.Message;
            }
            chuoi_dich_vu = "api/Phim/DocDanhSachTheLoai";
            var theLoais = Utilities.SendDataRequest<List<TheLoaiPhimModel.Output.ThongTinTheLoaiPhim>>(chuoi_dich_vu);
            chuoi_dich_vu = "api/Phim/DocDanhSachXepHangPhim";
            var xepHangPhims = Utilities.SendDataRequest<List<XepHangPhimModel.Output.ThongTinXepHangPhim>>(chuoi_dich_vu);
            model.DanhSachXepHangPhim = xepHangPhims.Select(x => new XepHangPhimModel.XepHangPhimBase
            {
                Id = x.Id,
                Ten = x.Ten,
                KyHieu = x.KyHieu
            }).ToList();
            model.DanhSachTheLoai = theLoais.Select(x => new TheLoaiPhimModel.TheLoaiPhimBase { Id = x.Id, Ten = x.Ten }).ToList();
            return View(model);
        }

        public IActionResult CapNhatPhim(int id)
        {
            if (id > 0)
            {
                var chuoi_dich_vu = $"api/Phim/ThongTinPhim?id={id}";
                var phim = Utilities.SendDataRequest<PhimModel.Output.ThongTinPhim>(chuoi_dich_vu);
                chuoi_dich_vu = "api/Phim/DocDanhSachTheLoai";
                var theLoais = Utilities.SendDataRequest<List<TheLoaiPhimModel.Output.ThongTinTheLoaiPhim>>(chuoi_dich_vu);
                chuoi_dich_vu = "api/Phim/DocDanhSachXepHangPhim";
                var xepHangPhims = Utilities.SendDataRequest<List<XepHangPhimModel.Output.ThongTinXepHangPhim>>(chuoi_dich_vu);
                if (phim != null && phim.Id > 0)
                {
                    PhimModel.Output.CapNhatPhim thongTinPhim = new();
                    Utilities.PropertyCopier<PhimModel.Output.ThongTinPhim, PhimModel.Output.CapNhatPhim>.Copy(phim, thongTinPhim);
                    
                    thongTinPhim.DanhSachXepHangPhim = xepHangPhims.Select(x => new XepHangPhimModel.XepHangPhimBase
                    {
                        Id = x.Id,
                        Ten = x.Ten,
                        KyHieu = x.KyHieu
                    }).ToList();
                    thongTinPhim.DanhSachTheLoai = theLoais.Select(x => new TheLoaiPhimModel.TheLoaiPhimBase { Id = x.Id, Ten = x.Ten }).ToList();
                    return View(thongTinPhim);
                }
            }
            RedirectToAction("DanhSachPhim");
            return View();
        }

        [HttpPost]
        public IActionResult CapNhatPhim(IFormCollection form)
        {
            PhimModel.Output.CapNhatPhim model = new PhimModel.Output.CapNhatPhim();
            var chuoi_dich_vu = "";
            try
            {
                if (string.IsNullOrEmpty(form["TenPhim"]))
                    model.ThongBao = "<p>- Tên phim phải khác rỗng</p>";
                if (string.IsNullOrEmpty(form["ThoiLuong"]) || int.Parse(form["ThoiLuong"]) <= 0)
                    model.ThongBao += "<p>- Thời lượng phim phải > 0</p>";
                if (string.IsNullOrEmpty(form["DanhSachTheLoaiId"]))
                    model.ThongBao += "<p>- Phim phải thuộc ít nhất một thể loại</p>";
                if (string.IsNullOrEmpty(model.ThongBao))
                {
                    var phimCapNhat = new PhimModel.Output.ThongTinPhim
                    {
                        Id = int.Parse(form["Id"].ToString()),
                        TenPhim = form["TenPhim"].ToString(),
                        TenGoc = form["TenGoc"].ToString(),
                        DienVien = form["DienVien"].ToString(),
                        DaoDien = form["DaoDien"].ToString(),
                        NgonNgu = form["NgonNgu"].ToString(),
                        NgayKhoiChieu = string.IsNullOrEmpty(form["NgayKhoiChieu"]) ? new DateTime(1900, 1, 1) : DateTime.Parse(form["NgayKhoiChieu"]),
                        NhaSanXuat = form["NhaSanXuat"].ToString(),
                        NuocSanXuat = form["NuocSanXuat"].ToString(),
                        NoiDung = form["NoiDung"].ToString(),
                        Poster = form["Poster"].ToString(),
                        ThoiLuong = int.Parse(form["ThoiLuong"].ToString()),
                        Trailer = form["Trailer"].ToString(),
                        XepHangPhimId = int.Parse(form["XepHangPhimId"].ToString()),
                        DanhSachTheLoaiId = "," + form["DanhSachTheLoaiId"].ToString() + ","
                    };
                    chuoi_dich_vu = "api/QuanTri/CapNhatPhim";
                    var tb = Common.Utilities.SendDataRequest<ThongBaoModel>(chuoi_dich_vu, phimCapNhat);
                    model.ThongBao = tb.NoiDung;
                }
            }
            catch (Exception ex)
            {
                model.ThongBao = "Có lỗi xảy ra: " + ex.Message;
            }

            //Gán thông tin phim cho phim cập nhật trả về view
            //...  
            model.Id = int.Parse(form["Id"].ToString());
            model.TenPhim = form["TenPhim"].ToString();
            model.TenGoc = form["TenGoc"].ToString();
            model.DienVien = form["DienVien"].ToString();
            model.DaoDien = form["DaoDien"].ToString();
            model.NgonNgu = form["NgonNgu"].ToString();
            model.NgayKhoiChieu = string.IsNullOrEmpty(form["NgayKhoiChieu"]) ? new DateTime(1900, 1, 1) : DateTime.Parse(form["NgayKhoiChieu"]);
            model.NhaSanXuat = form["NhaSanXuat"].ToString();
            model.NuocSanXuat = form["NuocSanXuat"].ToString();
            model.NoiDung = form["NoiDung"].ToString();
            model.Poster = form["Poster"].ToString();
            model.ThoiLuong = int.Parse(form["ThoiLuong"].ToString());
            model.Trailer = form["Trailer"].ToString();
            model.XepHangPhimId = int.Parse(form["XepHangPhimId"].ToString());
            model.DanhSachTheLoaiId = !string.IsNullOrEmpty(form["DanhSachTheLoaiId"]) ? "," + form["DanhSachTheLoaiId"].ToString() + "," : "";

            chuoi_dich_vu = "api/Phim/DocDanhSachTheLoai";
            var theLoais = Common.Utilities.SendDataRequest<List<TheLoaiPhimModel.Output.ThongTinTheLoaiPhim>>(chuoi_dich_vu);
            chuoi_dich_vu = "api/Phim/DocDanhSachXepHangPhim";
            var xepHangPhims = Common.Utilities.SendDataRequest<List<XepHangPhimModel.Output.ThongTinXepHangPhim>>(chuoi_dich_vu);
            model.DanhSachXepHangPhim = xepHangPhims.Select(x => new XepHangPhimModel.XepHangPhimBase
            {
                Id = x.Id,
                Ten = x.Ten,
                KyHieu = x.KyHieu
            }).ToList();
            model.DanhSachTheLoai = theLoais.Select(x => new TheLoaiPhimModel.TheLoaiPhimBase { Id = x.Id, Ten = x.Ten }).ToList();
            return View(model);
        }
    }
}
