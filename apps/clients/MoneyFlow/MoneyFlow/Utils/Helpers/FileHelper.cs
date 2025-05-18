using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MoneyFlow.Utils.Helpers
{
    public static class FileHelper
    {
        public static byte[] ImageToBytes(Image image)
        {
            using (MemoryStream memoryStream = new())
            {
                image.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
    }
}
