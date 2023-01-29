using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class NhanVienModel
    {
        public class NhanVienBase
        {
            public int Id { get; set; }
            [Display(Name ="Họ tên nhân viên")]
            public string HoTen { get; set; }
            [Display(Name = "Giới tính")]
            public bool GioiTinh { get; set; }
            [Display(Name = "Ngày sinh")]
            public DateTime NgaySinh { get; set; }
            [Display(Name = "Địa chỉ")]
            public string DiaChi { get; set; }
            [Display(Name = "Số CMND")]
            public string Cmnd { get; set; }
            [Display(Name = "Mật khẩu")]
            [DataType(DataType.Password)]
            public string MatKhau { get; set; }
            [Display(Name = "Làm việc tại")]
            public int RapId { get; set; }
            [Display(Name = "Quyền hạn")]
            public string QuyenHan { get; set; }
        }
        public class Input
        {
            public class ThongTinDangNhap
            {
                public string TenDangNhap { get; set; }
                public string MatKhau { get; set; }
            }
            public class ThongTinThayDoiMatKhau
            {
                public int Id { get; set; }
                public string Username { get; set; }
                public string MatKhauCu { get; set; }
                public string MatKhauMoi { get; set; }
            }
            public class DanhSachNhanVien
            {
                public bool QuanTri { get; set; }
            }
        }
        public class Output
        {
            public class ThongTinNhanVien : NhanVienBase
            {
                public string AccessToken { get; set; }
                public string ThongBao { get; set; }
            }
        }
    }
}
