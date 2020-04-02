using FYP.Data;
using FYP.Models.ViewModels;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Controllers.Api
{
    public class ImageApiController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImageApiController(
            ApplicationDbContext db,
            IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Image/UploadImage")]
        public ImageUploadOutput UploadImage()
        {
            ImageUploadOutput output = new ImageUploadOutput();

            var file = Request.Form.Files;
            foreach (var item in file)
            {
                if (item != null && item.Length > 0)
                {
                    string id = Guid.NewGuid().ToString();
                    string[] ext = item.FileName.Split('.');
                    string part1 = "wwwroot";
                    string part2 = "UploadImage\\" + id + "." + ext[ext.Length - 1];
                    string fileName = Path.Combine(part1, part2);
                    string url = _hostingEnvironment.ContentRootPath;
                    string path = Path.Combine(url, fileName);

                    ISupportedImageFormat format = new PngFormat { Quality = 100, IsIndexed = false };
                    System.Drawing.Image img = System.Drawing.Image.FromStream(item.OpenReadStream(), true, true);
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(img)
                                .Format(format)
                                .Save(fs);
                        }
                        output.Url = "UploadImage/" + id + "." + ext[ext.Length - 1];
                    }
                }
            }
            output.Result = "OK";

            return output;
        }
    }
}
