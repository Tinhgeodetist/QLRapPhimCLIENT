using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class GheModel
    {
        public class GheBase
        {
            public int Id { get; set; }
            public string SoGhe { get; set; }
            public int LoaiGheId { get; set; }
            public int PhongId { get; set; }
            public int Day { get; set; }
            public int Hang { get; set; }
            public bool GheVip { get; set; }
        }
        public class Input
        {
            public class DanhSachGheTheoPhong
            {
                public int PhongId { get; set; }
            }
            public class DanhSachGheTheoPhongVaLoaiGhe
            {
                public int PhongId { get; set; }
                public int LoaiGheId { get; set; }
            }
            public class DanhSachGheTheoDanhSachId
            {
                public List<int> DanhSachId { get; set; }
                public DanhSachGheTheoDanhSachId()
                {
                    DanhSachId = new();
                }
            }
            public class DocThongTinGhe
            {
                public int Id { get; set; }
            }
            public class ThongTinGhe : GheBase { }
            public class XoaThongTinGhe
            {
                public int Id { get; set; }
            }
            public class PhatSinhGheTheoPhong
            {
                public int PhongId { get; set; }
            }
        }
        public class Output
        {
            public class ThongTinGhe : GheBase
            {
                public PhongInfo Phong { get; set; }
                public LoaiGheInfo LoaiGhe { get; set; }
                public ThongTinGhe()
                {
                    Phong = new();
                    LoaiGhe = new();
                }
            }
        }
        public class PhongInfo
        {
            public int Id { get; set; }
            public string TenPhong { get; set; }
            public int SoLuongGhe { get; set; }
            public string GhiChu { get; set; }
            public int RapId { get; set; }
            public int TongSoDay { get; set; }
            public int TongSoHang { get; set; }
        }
        public class LoaiGheInfo
        {
            public int Id { get; set; }
            public string TenLoaiGhe { get; set; }
            public int GiaVe { get; set; }
        }
    }
}
