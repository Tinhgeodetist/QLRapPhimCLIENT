using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class RapModel
    {
        public class RapBase
        {
            public int Id { get; set; }
            [Display(Name = "Tên rạp")]
            [Required(ErrorMessage ="Tên rạp phải khác rỗng")]
            public string TenRap { get; set; }
            [Display(Name = "Địa chỉ")]
            [Required(ErrorMessage = "Địa chỉ phải khác rỗng")]
            public string DiaChi { get; set; }
            [Display(Name = "Điện thoại")]
            public string DienThoai { get; set; }
            [Display(Name = "Email")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Display(Name = "Người phụ trách")]
            public int NguoiQuanLyId { get; set; }
            [Display(Name = "Vĩ độ (bản đồ)")]
            [DataType(DataType.Currency)]
            public double ViDo { get; set; }
            [Display(Name = "Kinh độ (bản đồ)")]
            [DataType(DataType.Currency)]
            public double KinhDo { get; set; }
        }
        public class Input {
            public class ThongTinRap : RapBase { }
            public class DocThongTinRap
            {
                public int Id { get; set; }
            }
            public class XoaRap
            {
                public int Id { get; set; }
            }
        }
        public class Output
        {
            public class ThongTinRap : RapBase
            {
            }
        }
    }
}
