using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Common
{
    public class ConstantValues
    {
        public class SlideBanner
        {
            public const string DanhSachSlideBanner = "api/SlideBanner/DanhSachSlideBanner";
            public const string ThongTinSlideBanner = "api/SlideBanner/ThongTinSlideBanner";
            public const string ThemSlideBanner = "api/SlideBanner/ThemSlideBanner";
            public const string CapNhatSlideBanner = "api/SlideBanner/CapNhatSlideBanner";
            public const string XoaSlideBanner = "api/SlideBanner/XoaSlideBanner";
        }
        public class Rap
        {
            public const string DanhSachRap = "api/Rap/DanhSachRap";
            public const string ThongTinRap = "api/Rap/ThongTinRap";
            public const string ThemRap = "api/Rap/ThemRap";
            public const string CapNhatRap = "api/Rap/CapNhatRap";
            public const string XoaRap = "api/Rap/XoaRap";
        }
        public class Phong
        {
            public const string DanhSachPhongTheoRap = "api/Phong/DanhSachPhongTheoRap";
            public const string ThongTinPhong = "api/Phong/ThongTinPhong";
            public const string ThemPhong = "api/Phong/ThemPhong";
            public const string CapNhatPhong = "api/Phong/CapNhatPhong";
            public const string XoaPhong = "api/Phong/XoaPhong";
        }
        public class ThongTinGhe
        {
            public const string DanhSachThongTinGheTheoPhong = "api/ThongTinGhe/DanhSachThongTinGheTheoPhong";
            public const string ChiTietThongTinGhe = "api/ThongTinGhe/ChiTietThongTinGhe";
            public const string ThemThongTinGhe = "api/ThongTinGhe/ThemThongTinGhe";
            public const string CapNhatThongTinGhe = "api/ThongTinGhe/CapNhatThongTinGhe";
            public const string XoaThongTinGhe = "api/ThongTinGhe/XoaThongTinGhe";
        }
        public class LoaiGhe
        {
            public const string DanhSachLoaiGhe = "api/LoaiGhe/DanhSachLoaiGhe";
            public const string ThongTinLoaiGhe = "api/LoaiGhe/ThongTinLoaiGhe";
            public const string ThemLoaiGhe = "api/LoaiGhe/ThemLoaiGhe";
            public const string CapNhatLoaiGhe = "api/LoaiGhe/CapNhatLoaiGhe";
            public const string XoaLoaiGhe = "api/LoaiGhe/XoaLoaiGhe";
        }
        public class Ghe
        {
            public const string DanhSachGheTheoPhong = "api/Ghe/DanhSachGheTheoPhong";
            public const string DanhSachGheTheoPhongVaLoaiGhe = "api/Ghe/DanhSachGheTheoPhongVaLoaiGhe";
            public const string PhatSinhGheTheoPhong = "api/Ghe/PhatSinhGheTheoPhong";
            public const string DanhSachGheTheoDanhSachId = "api/Ghe/DanhSachGheTheoDanhSachId";
        }
        public class Phim
        {
            public const string PhimDangChieu = "api/Phim/DanhSachPhimDangChieu";
            public const string PhimSapChieu = "api/Phim/DanhSachPhimSapChieu";
            public const string PhimTheoTheLoai = "api/Phim/DanhSachPhimTheoTheLoai";
            public const string ThongTinPhim = "api/Phim/ThongTinPhim";
            public const string ThemPhimMoi = "api/Phim/ThemPhimMoi";
            public const string CapNhatPhim = "api/Phim/CapNhatPhim";
            public const string XoaPhim = "api/Phim/XoaPhim";
        }
        public class TheLoaiPhim
        {
            public const string DanhSachTheLoai = "api/Phim/DanhSachTheLoai";
        }
        public class SuatChieu
        {
            public const string DanhSachSuatChieu = "api/SuatChieu/DanhSachSuatChieu";
            public const string ThemSuatChieu = "api/SuatChieu/ThemSuatChieu";
            public const string CapNhatSuatChieu = "api/SuatChieu/CapNhatSuatChieu";
            public const string ThongTinSuatChieu = "api/SuatChieu/ThongTinSuatChieu";
            public const string XoaSuatChieu = "api/SuatChieu/XoaSuatChieu";
        }
        public class LichChieu
        {
            public const string DanhSachLichChieuTheoRap = "api/LichChieu/DanhSachLichChieuTheoRap";
            public const string DanhSachLichChieuTheoPhimVaNgay = "api/LichChieu/DanhSachLichChieuTheoPhimVaNgay";
            public const string ThongTinLichChieu = "api/LichChieu/ThongTinLichChieu";
            public const string ThemLichChieu = "api/LichChieu/ThemLichChieu";
            public const string CapNhatLichChieu = "api/LichChieu/CapNhatLichChieu";
            public const string XoaLichChieu = "api/LichChieu/XoaLichChieu";
        }
        public class Ve
        {
            public const string MuaVe = "api/Ve/MuaVe";
            public const string DanhSachVeTheoLichChieu = "api/Ve/DanhSachVeTheoLichChieu";
            public const string DanhSachVeKhongDuocMuaTheoLichChieu = "api/Ve/DanhSachVeKhongDuocMuaTheoLichChieu";
        }
        public class ThanhVien
        {
            public const string DangNhap = "api/ThanhVien/DangNhap";
            public const string DangNhapAnDanh = "api/ThanhVien/DangNhapAnDanh";
            public const string DangXuat = "api/ThanhVien/DangXuat";
            public const string DangKyThanhVien = "api/ThanhVien/DangKyThanhVien";
            public const string DoiMatKhau = "api/ThanhVien/ThayDoiMatKhau";
            public const string ThongTinThanhVien = "api/ThanhVien/ThongTinThanhVien";
            public const string ThemThanhVien = "api/ThanhVien/ThemThanhVien";
            public const string KichHoatTaiKhoan = "api/ThanhVien/KichHoatTaiKhoan";
        }
        public class NhanVien
        {
            public const string DanhSachNhanVien = "api/NhanVien/DanhSachNhanVien";
            public const string DangNhap = "api/NhanVien/DangNhap";
            public const string DangXuat = "api/NhanVien/DangXuat";
            public const string DoiMatKhau = "api/NhanVien/ThayDoiMatKhau";
        }
    }
}
