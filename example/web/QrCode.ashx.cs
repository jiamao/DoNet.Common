using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

using Gma.QrCodeNet.Encoding;
namespace JM.Common.Examples.web
{
    /// <summary>
    /// QrCode 的摘要说明
    /// </summary>
    public class QrCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/png";

            CreateQr(context);
        }

        private void CreateQr(HttpContext context)
        {
            var text = context.Request["text"];
            if (string.IsNullOrWhiteSpace(text)) text = "myip:" + DoNet.Common.Net.BaseNet.GetWebIPAddress();

            var encoder = new QrEncoder(ErrorCorrectionLevel.H);
            var qrcode = encoder.Encode(text);

            var render = new Gma.QrCodeNet.Encoding.Windows.Controls.Renderer(5, Brushes.Black, Brushes.White);
            var size = render.Measure(qrcode.Matrix.Width);

            var img = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(img))
            {
                render.Draw(g, qrcode.Matrix);
                var ms = new System.IO.MemoryStream();
                img.Save(ms, ImageFormat.Png);
                context.Response.BinaryWrite(ms.ToArray());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}