using System;
using System.IO;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.DrawingCore.Drawing2D;

namespace Easy.Core.Flow.ImageSplicing
{
    class Program
    {
        static void Main(string[] args)
        {
            // 最终输出的文件夹地址和图片名称
            var targetImagePath = Path.Combine(AppContext.BaseDirectory, $"targetImage.jpg");


            // 要处理图像1
            var soureImagePath1 = Path.Combine(AppContext.BaseDirectory, "20201118131620.jpg");
            Image soureImage1 = Image.FromFile(soureImagePath1);///实例化,得到img

            // 要处理图像2
            var soureImagePath2 = Path.Combine(AppContext.BaseDirectory, "20201118134500.jpg");
            Image soureImage2 = Image.FromFile(soureImagePath2);///实例化,得到img


            //获取图片宽高
            int maxHeight = soureImage1.Height;



            // 按照比例将2涨照片同比例缩放到一样的大小 
            var minWidth = soureImage1.Width > soureImage2.Width ? soureImage2.Width : soureImage1.Width;
            // 根据宽度比例缩放后的图像高度
            var soureImageHeight1 = soureImage1.Height * minWidth / soureImage1.Width;
            var soureImageHeight2 = soureImage2.Height * minWidth / soureImage2.Width;



            // 准备一个目标画布
            Image templateImage = new Bitmap(minWidth, maxHeight);
            Graphics Grp = Graphics.FromImage(templateImage);
            Grp.FillRectangle(Brushes.White, new Rectangle(0, 0, minWidth, maxHeight));
            Grp.InterpolationMode = InterpolationMode.High;
            Grp.SmoothingMode = SmoothingMode.HighQuality;
            Grp.Clear(Color.White);


            // 在空白画布上填充
            // 第二个 Rectangle 是因为这个图片要做裁剪，不是根据宽高做比例缩放 要给出裁剪的部分
            Grp.DrawImage(soureImage1, new Rectangle(0, 0, minWidth, soureImageHeight1 - soureImageHeight2), new Rectangle(0, 0, minWidth, soureImageHeight1 - soureImageHeight2), GraphicsUnit.Pixel);
            Grp.DrawImage(soureImage2, new Rectangle(0, soureImageHeight1 - soureImageHeight2, minWidth, soureImageHeight2));


            templateImage.Save(targetImagePath, ImageFormat.Jpeg);


            Console.WriteLine("图像裁剪拼接完成!");
        }

    }
}
