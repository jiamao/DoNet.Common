//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 日志操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DoNet.Common.Drawing
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 矩阵法获取像素
        /// </summary>
        /// <param name="srcBitmap"></param>
        /// <returns></returns>
        public static Color[][] ImageToColors(Image srcBitmap)
        {
            int wide = srcBitmap.Width;
            int height = srcBitmap.Height;
            Bitmap tmpbmp = srcBitmap as Bitmap;

            Color[][] colors = new Color[wide][];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color[height];
            }
            //unsafe //启动不安全代码
            //{

                for (int x = 0; x < wide; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        colors[x][y] = tmpbmp.GetPixel(x, y);
                    }

                }
            //}

            return colors;

        }

        /// <summary>
        /// 获取图片像素内存法
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Color[][] ImageToColorsByMemory(Image source)
        {
            Bitmap srcBitmap = source as Bitmap;

            int wide = srcBitmap.Width;

            int height = srcBitmap.Height;

            Rectangle rect = new Rectangle(0, 0, wide, height);

            // 将Bitmap锁定到系统内存中, 获得BitmapData

            BitmapData srcBmData = srcBitmap.LockBits(rect,

                      ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            //创建Bitmap

            //Bitmap dstBitmap = new Bitmap(wide, height);//

            //BitmapData dstBmData = dstBitmap.LockBits(rect,

            // ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

            // 位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行

            System.IntPtr srcPtr = srcBmData.Scan0;

            // System.IntPtr dstPtr = dstBmData.Scan0;

            // 将Bitmap对象的信息存放到byte数组中

            int src_bytes = srcBmData.Stride * height;

            byte[] srcValues = new byte[src_bytes];

            //int dst_bytes = dstBmData.Stride * height;

            //byte[] dstValues = new byte[dst_bytes];

            //复制GRB信息到byte数组

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcValues, 0, src_bytes);

            //System.Runtime.InteropServices.Marshal.Copy(dstPtr, dstValues, 0, dst_bytes);

            // 根据Y=0.299*R+0.114*G+0.587B,Y为亮度
            Color[][] colors = new Color[wide][];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color[height];
            }
            for (int i = 0; i < height; i++)

                for (int j = 0; j < wide; j++)
                {

                    //只处理每行中图像像素数据,舍弃未用空间

                    //注意位图结构中RGB按BGR的顺序存储

                    int k = 3 * j;

                    //byte temp = (byte)(srcValues[i * srcBmData.Stride + k + 2] * .299

                    //+ srcValues[i * srcBmData.Stride + k + 1] * .587 + srcValues[i * srcBmData.Stride + k] * .114);

                    //dstValues[i * dstBmData.Stride + j] = temp;

                    colors[j][i] = Color.FromArgb(srcValues[i * srcBmData.Stride + k + 2], srcValues[i * srcBmData.Stride + k + 1], srcValues[i * srcBmData.Stride + k]);

                }

            //将更改过的byte[]拷贝到原位图

            //System.Runtime.InteropServices.Marshal.Copy(dstValues, 0, dstPtr, dst_bytes);

            // 解锁位图
            srcBitmap.UnlockBits(srcBmData);

            //dstBitmap.UnlockBits(dstBmData);

            return colors;
        }

        /// <summary>
        /// 通过像素生成图片
        /// </summary>
        /// <param name="colors"></param>
        /// <returns></returns>
        public static Image CreateBitmapFromColors(Color[][] colors)
        {
            Bitmap bmp = new Bitmap(colors.Length, colors[0].Length);
            //Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            //BitmapData dstBmData = bmp.LockBits(rect,
            //          ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //System.IntPtr dstScan = dstBmData.Scan0;
            //unsafe //启动不安全代码
            //{
                //byte* dstP = (byte*)(void*)dstScan;

                //int dstOffset = dstBmData.Stride - bmp.Width;                

                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        // *dstP = (byte)(.299 * colors[x][y].R + .587 * colors[x][y].G + .114 * colors[x][y].B);
                        bmp.SetPixel(x, y, colors[x][y]);
                    }

                    //dstP += dstOffset;
                }
                //bmp.UnlockBits(dstBmData);
            //}
            return bmp;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="img"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Image img, int w, int h)
        {
            float wf = (float)w / img.Width;
            float hf = (float)h / img.Height;
            //保持取最小的比例,,以例 图像不扭曲
            if (wf > hf) w = (int)(img.Width * hf);
            else h = (int)(img.Height * wf);

            Bitmap bmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bmp);
            Color bgcolor = Color.White;
            g.Clear(bgcolor);
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //开始画图
            g.DrawImage(img, new Rectangle(0, 0, w, h), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);

            g.Dispose();

            return bmp;
        }
    }
}
