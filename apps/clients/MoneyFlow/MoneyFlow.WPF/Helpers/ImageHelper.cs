using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MoneyFlow.WPF.Helpers
{
    public static class ImageHelper
    {
        public static async Task<byte[]> ImageByteArray(string imagePath)
        {
            return await Task.Run(() =>
            {
                using (var image = Image.FromFile(imagePath))
                {
                    using (var memory = new MemoryStream())
                    {
                        image.Save(memory, ImageFormat.Png);

                        return memory.ToArray();
                    }
                }
            });
        }
    }
}
