using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;


namespace App.Infrastructure.Helpers
{
    public static class PhotoUploader
    {
        public static async Task<string> SaveFileAsync(IFormFile file, string rootPath, string subFolder, int? width = null, int? height = null)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file.");
            }

            var folderPath = Path.Combine(rootPath, subFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var image = Image.FromStream(stream))
                {
                    Image resizedImage = image;

                    if (width.HasValue && height.HasValue)
                    {
                        resizedImage = ResizeImage(image, width.Value, height.Value);
                    }

                    resizedImage.Save(filePath, ImageFormat.Png);
                }
            }

            return fileName;
        }

        private static Image ResizeImage(Image image, int width, int height)
        {
            var newImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return newImage;
        }
    }
}
