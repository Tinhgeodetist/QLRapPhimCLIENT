using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class LoaiGheModel
    {
        public class LoaiGheBase
        {
            public int Id { get; set; }
            public string TenLoaiGhe { get; set; }
            public int GiaVe { get; set; }
        }
        public class Input
        {
            public class DocThongTinLoaiGhe
            {
                public int Id { get; set; }
            }
            public class ThongTinLoaiGhe : LoaiGheBase { }
            public class XoaLoaiGhe
            {
                public int Id { get; set; }
            }
        }
        public class Output
        {
            public class ThongTinLoaiGhe : LoaiGheBase
            {
                
            }
        }
        
    }
}
