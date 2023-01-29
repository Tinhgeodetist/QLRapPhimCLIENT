using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class PhongModel
    {
        public class PhongBase
        {
            public int Id { get; set; }
            [Display(Name ="Tên phòng")]
            [Required(ErrorMessage ="Tên phòng phải khác rỗng")]
            public string TenPhong { get; set; }
            [Display(Name = "Số lượng ghế")]
            [Required(ErrorMessage = "Số lượng ghế phải khác rỗng")]
            [Range(1, int.MaxValue, ErrorMessage ="Số lượng ghế phải > 0")]
            [DataType(DataType.Currency)]
            public int SoLuongGhe { get; set; }
            [Display(Name = "Ghi chú")]
            public string GhiChu { get; set; }
            [Display(Name = "Rạp")]
            [Range(1, int.MaxValue, ErrorMessage = "Chưa chọn rạp")]
            public int RapId { get; set; }
            [Display(Name = "Tổng số dãy ghế")]
            [Range(1, int.MaxValue, ErrorMessage = "Tổng số dãy ghế phải > 0")]
            public int TongSoDay { get; set; }
            [Display(Name = "Tổng số hàng ghế")]
            [Range(1, int.MaxValue, ErrorMessage = "Tổng số hàng ghế phải > 0")]
            public int TongSoHang { get; set; }
        }
        public class Input
        {
            public class DanhSachPhongTheoRap
            {
                public int RapId { get; set; }
            }
            public class DocThongTinPhong
            {
                public int Id { get; set; }
            }
            public class ThongTinPhong : PhongBase { }
            public class XoaPhong
            {
                public int Id { get; set; }
            }
        }
        public class Output
        {
            public class ThongTinPhong : PhongBase
            {
                [Display(Name = "Rạp")]
                public RapInfo Rap { get; set; }
                public ThongTinPhong()
                {
                    Rap = new();
                }
            }
        }
        public class RapInfo
        {
            public int Id { get; set; }
            public string TenRap { get; set; }
            public string DiaChi { get; set; }
            public string DienThoai { get; set; }
            public string Email { get; set; }
            public int NguoiQuanLyId { get; set; }
            public double ViDo { get; set; }
            public double KinhDo { get; set; }
        }
    }
}
