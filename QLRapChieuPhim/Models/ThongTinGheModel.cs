using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class ThongTinGheModel
    {
        public class ThongTinGheBase
        {
            public int Id { get; set; }
            [Display(Name = "Ký hiệu dãy ghế (A, B, ...)")]
            [Required(ErrorMessage ="Ký hiệu dãy phải khác rỗng")]
            public string KyHieu { get; set; }
            [Display(Name ="Số lượng ghế trong dãy")]
            [DataType(DataType.Currency)]
            [Range(1, int.MaxValue, ErrorMessage = "Số lượng ghế phải > 0")]
            public int SoGhe { get; set; }
            [Display(Name = "Thứ tự dãy bắt đầu trong phòng chiếu")]
            [DataType(DataType.Currency)]
            [Range(1, int.MaxValue, ErrorMessage = "Thứ tự dãy phải > 0")]
            public int Day { get; set; }
            [Display(Name = "Dãy ghế bắt đầu từ hàng")]
            [DataType(DataType.Currency)]
            [Range(1, int.MaxValue, ErrorMessage = "Thứ tự hàng phải > 0")]
            public int Hang { get; set; }
            [Display(Name = "Phòng chiếu")]
            public int PhongId { get; set; }
            [Display(Name = "Loại ghế")]
            public int LoaiGheId { get; set; }
            [Display(Name = "Vị trí ghế Vip trong dãy (vd: 5-22)")]
            public string ViTriGheVip { get; set; }
        }
        public class Input
        {
            public class DanhSachThongTinGheTheoPhong
            {
                public int PhongId { get; set; }
            }
            public class DocThongTinGhe
            {
                public int Id { get; set; }
            }
            public class ThongTinGhe : ThongTinGheBase { }
            public class XoaThongTinGhe
            {
                public int Id { get; set; }
            }
        }
        public class Output
        {
            public class ThongTinGhe : ThongTinGheBase
            {
                public PhongInfo Phong { get; set; }
                public LoaiGheInfo LoaiGhe { get; set; }
                public ThongTinGhe()
                {
                    Phong = new();
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
