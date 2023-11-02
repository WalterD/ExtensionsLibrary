using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ExtensionsLibrary
{
    public static class ImageExtensions
    {
        public static byte[] ConvertToBytes(this Image image, ImageFormat imageFormat)
        {
            // convert image to bytes
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                // if the requested image is jpeg, save it with maximum quality
                if (imageFormat == ImageFormat.Jpeg)
                {
                    // set image quality to its maximum value, 100
                    long quality = 100L;
                    using (var encoderParameters = new EncoderParameters(1))
                    using (var encoderParameter = new EncoderParameter(Encoder.Quality, quality))
                    {
                        ImageCodecInfo codecInfo = ImageCodecInfo.GetImageDecoders().First(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                        encoderParameters.Param[0] = encoderParameter;
                        image.Save(ms, codecInfo, encoderParameters);
                    }
                }
                else
                {
                    image.Save(ms, imageFormat);
                }

                bytes = ms.ToArray();
            }

            return bytes;
        }

        public static Stream ToStream(this Image image, ImageFormat format)
        {
            var stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }
    }
}
