using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Models
{
    public class HomeFrontendModel
    {
        public List<SlideBannerModel.SlideBannerBase> DanhSachBanner { get; set; }
        public List<PhimModel.Output.ThongTinPhim> DanhSachPhimDangChieu { get; set; }
        public HomeFrontendModel()
        {
            DanhSachBanner = new();
            DanhSachPhimDangChieu = new();
        }
    }
}
