using Microsoft.AspNetCore.Hosting;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Services
{
    public static class QRCodeService
    {
        public static string GenerateQRCode(IWebHostEnvironment environment, string message)
        {
            string root = environment.WebRootPath + "\\qrcode";
            string fileName = Guid.NewGuid().ToString()+".png";
            string path = Path.Combine(root, fileName);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(message, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    qrCodeImage.Save(memory, ImageFormat.Png);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            return fileName;
        }
    }
}
