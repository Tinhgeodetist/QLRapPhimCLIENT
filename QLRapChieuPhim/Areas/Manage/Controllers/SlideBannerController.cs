using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLRapChieuPhim.Areas.Manage.Models.Authorization;
using QLRapChieuPhim.Common;
using QLRapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QLRapChieuPhim.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize("Quantri")]
    public class SlideBannerController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SlideBannerController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        public IActionResult Index()
        {
            var input = new SlideBannerModel.Input.DocDanhSachSlideBanner { QuanTri = true };
            var dsBanner = Utilities.SendDataRequest<List<SlideBannerModel.Output.ThongTinSlideBanner>>(ConstantValues.SlideBanner.DanhSachSlideBanner, input);
            return View(dsBanner);
        }
        public IActionResult ThemSlideBanner(string ten, IFormFile hinh, string lienket, bool kichhoat)
        {
            SlideBannerModel.SlideBannerBase slideBanner = new();
            if (hinh != null)
            {
                var filename = hinh.FileName;
                var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "images\\slides\\banner", filename);
                var imagepath = "/images/slides/banner/" + filename;
                var banner = new SlideBannerModel.Output.ThongTinSlideBanner
                {
                    Hinh = imagepath,
                    Ten = ten,
                    LienKet = lienket == null ? "" : lienket,
                    KichHoat = kichhoat
                };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.SlideBanner.ThemSlideBanner, banner);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                    Utilities.PropertyCopier<SlideBannerModel.Output.ThongTinSlideBanner, SlideBannerModel.SlideBannerBase>.Copy(banner, slideBanner);
                }
                else
                {
                    hinh.CopyTo(new FileStream(filepath, FileMode.Create));
                    return RedirectToAction("Index");
                }

            }
            return View(slideBanner);
        }

        public IActionResult CapNhatSlideBanner(int id)
        {
            if (id > 0)
            {
                var input = new SlideBannerModel.Input.DocThongTinSlideBanner { Id = id };
                var banner = Utilities.SendDataRequest<SlideBannerModel.Output.ThongTinSlideBanner>(ConstantValues.SlideBanner.ThongTinSlideBanner, input);
                if (banner != null && banner.Id > 0)
                {
                    return View(banner);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CapNhatSlideBanner(IFormFile hinh, string ten, int id, string lienket, bool kichhoat)
        {
            var input = new SlideBannerModel.Input.DocThongTinSlideBanner { Id = id };
            var banner = Utilities.SendDataRequest<SlideBannerModel.Output.ThongTinSlideBanner>(ConstantValues.SlideBanner.ThongTinSlideBanner, input);
            SlideBannerModel.SlideBannerBase slideBanner = new();
            if (banner != null && banner.Id > 0)
            {
                banner.Ten = ten;
                banner.KichHoat = kichhoat;
                banner.LienKet = lienket == null ? "" : lienket;

                var filepath = "";
                if (hinh != null)
                {
                    var filename = hinh.FileName;
                    filepath = Path.Combine(_hostingEnvironment.WebRootPath, "images\\slides\\banner", filename);
                    var imagepath = "/images/slides/banner/" + filename;
                    banner.Hinh = imagepath;
                }                
                
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.SlideBanner.CapNhatSlideBanner, banner);

                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                    Utilities.PropertyCopier<SlideBannerModel.Output.ThongTinSlideBanner, SlideBannerModel.SlideBannerBase>.Copy(banner, slideBanner);
                }
                else
                {
                    if (filepath != "") hinh.CopyTo(new FileStream(filepath, FileMode.Create));
                    return RedirectToAction("Index");
                }
            }
            return View(slideBanner);
        }
        
        public IActionResult XoaSlideBanner(int id)
        {
            if (id > 0)
            {
                var input = new SlideBannerModel.Input.XoaSlideBanner { Id = id };
                var tb = Utilities.SendDataRequest<ThongBaoModel>(ConstantValues.SlideBanner.XoaSlideBanner, input);
                if (tb.MaSo > 0)
                {
                    ViewData["ThongBao"] = tb.NoiDung;
                }
                else
                {
                    if (!string.IsNullOrEmpty(tb.NoiDung))
                    {
                        var filepath = _hostingEnvironment.WebRootPath + tb.NoiDung.Replace("/", "\\");
                        if (filepath != "") System.IO.File.Delete(filepath);
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
