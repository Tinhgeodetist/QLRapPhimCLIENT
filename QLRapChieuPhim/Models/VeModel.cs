using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class VeModel
    {
        public class VeBase
        {
            public int Id { get; set; }
            public DateTime NgayBanVe { get; set; }
            public string SoGhe { get; set; }
            public int GiaVe { get; set; }
            public int LichChieuId { get; set; }
            public int NhanVienId { get; set; }
            public int GheId { get; set; }
            public int TinhTrang { get; set; }
        }
        public class Input
        {
            public class ThongTinVe : VeBase { }
            public class DocThongTinVe
            {
                public int Id { get; set; }
            }
            public class DocDanhSachVeTheoLichChieu
            {
                public int LichChieuId { get; set; }
            }
            public class MuaVe
            {
                public List<ThongTinVe> DanhSachVe { get; set; }
                public MuaVe()
                {
                    DanhSachVe = new();
                }
            }
        }
        public class Output
        {
            public class ThongTinVe : VeBase
            {
                public GheModel.GheBase Ghe { get; set; }
                public LichChieuModel.LichChieuBase LichChieu { get; set; }
                public NhanVienModel.NhanVienBase NhanVien { get; set; }

                public ThongTinVe()
                {
                    Ghe = new();
                    LichChieu = new();
                    NhanVien = new();
                }
            }
        }
    }
}
