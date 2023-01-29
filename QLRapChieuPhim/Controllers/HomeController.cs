using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLRapChieuPhim.Common;
using QLRapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QLRapChieuPhim.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Banner
            //var response = Utilities.SendDataRequest("api/SlideBanner/DanhSachSlideBanner");
            //var dsSlide = JsonConvert.DeserializeObject<List<SlideBannerModel.Output.ThongTinSlideBanner>>(response.ToString());
            var thanhvien = HttpContext.Session.Get<ThanhVienModel.Output.ThongTinThanhVien>("ThanhVien");
            if(thanhvien == null)
            {
                thanhvien = Utilities.SendDataRequest<ThanhVienModel.Output.ThongTinThanhVien>(ConstantValues.ThanhVien.DangNhapAnDanh);
                if (thanhvien != null)
                {
                    HttpContext.Session.Set<ThanhVienModel.Output.ThongTinThanhVien>("ThanhVien", thanhvien);
                }
            }
            var input = new SlideBannerModel.Input.DocDanhSachSlideBanner { QuanTri = false };
            var dsSlide = Utilities.SendDataRequest<List<SlideBannerModel.Output.ThongTinSlideBanner>>(ConstantValues.SlideBanner.DanhSachSlideBanner, input);
            HomeFrontendModel model = new();
            model.DanhSachBanner = dsSlide.Where(x => x.KichHoat)
                                            .Select(x => new SlideBannerModel.SlideBannerBase
                                            {
                                                Id = x.Id,
                                                Ten = x.Ten,
                                                Hinh = x.Hinh,
                                                LienKet = x.LienKet,
                                                KichHoat = x.KichHoat
                                            }).ToList();

            //Phim Đang Chiếu            
            model.DanhSachPhimDangChieu = Utilities.SendDataRequest<List<PhimModel.Output.ThongTinPhim>>(ConstantValues.Phim.PhimDangChieu);
            return View(model);
        }

        public IActionResult HeThongRap()
        {
            var dsRap = Utilities.SendDataRequest<List<RapModel.Output.ThongTinRap>>(ConstantValues.Rap.DanhSachRap);
            return View(dsRap);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
