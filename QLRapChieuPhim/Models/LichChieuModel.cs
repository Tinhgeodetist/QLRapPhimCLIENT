using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class LichChieuModel
    {
        public class LichChieuBase
        {
            [ScaffoldColumn(false)]
            public int Id { get; set; }
            public int SuatChieuId { get; set; }
            public int PhongId { get; set; }
            public int PhimId { get; set; }
            [Display(Name = "Ngày chiếu")]
            [DataType(DataType.Date)]
            public DateTime NgayChieu { get; set; }
        }
        public class Input
        {
            public class DanhSachLichChieuTheoRap
            {
                public int RapId { get; set; }
            }
            public class DocThongTinLichChieu
            {
                public int Id { get; set; }
            }
            public class DanhSachLichChieuTheoPhimVaNgay
            {
                public int PhimId { get; set; }
                public DateTime NgayChieu { get; set; }
            }
            public class ThongTinLichChieu : LichChieuBase
            {
                public int RapId { get; set; }
                public string TenRap { get; set; }
            }
            public class XoaLichChieu
            {
                public int Id { get; set; }
            }
        }
        public class Output
        {
            public class ThongTinLichChieu : LichChieuBase
            {
                public int RapId { get; set; }
                public string TenRap { get; set; }
                [Display(Name = "Suất chiếu")]
                public SuatChieuModel.Output.ThongTinSuatChieu SuatChieu { get; set; }
                [Display(Name = "Phòng chiếu")]
                public PhongModel.Output.ThongTinPhong Phong { get; set; }
                [Display(Name = "Phim chiếu")]
                public PhimModel.Output.ThongTinPhim Phim { get; set; }
                public ThongTinLichChieu()
                {
                    SuatChieu = new();
                    Phong = new();
                    Phim = new();
                }
            }
        }
    }
}
