using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace DoNet.Common.IO
{
    /// <summary>
    /// 打印机
    /// </summary>
    public class Printer
    {
        public struct PrintModel
        {
            public Font font;
            public string value;
            public string textalgin;
            public Brush brush;
            public Point location;
        }

        public PrintDocument PrinterDocument
        {
            get;
            set;
        }
        PrintDialog printdialog = new PrintDialog();
        PrintPreviewDialog printpredialog = new PrintPreviewDialog();
        
        public Printer()
        {
            PrinterDocument = new PrintDocument();
            PrinterDocument.PrintPage += new PrintPageEventHandler(print_print);
            printdialog.Document = PrinterDocument;
            printpredialog.Document = PrinterDocument;
          
            printpredialog.PrintPreviewControl.Zoom = 1;
            
            PrinterItems = new List<PrintModel>();
        }

        
        /// <summary>
        /// 所有要打印的对象
        /// </summary>
        public List<PrintModel> PrinterItems
        {
            get;
            set;
        }

        Image _imgtmp;
        public Image PrintImage
        {
            get { return _imgtmp; }
            set { _imgtmp = value.Clone() as Image; }
        }

        /// <summary>
        /// 打印的宽度
        /// </summary>
        public int Width
        {
            get;
            set;
        }
        
        /// <summary>
        /// rs 打印
        /// </summary>
        public void print()
        {
            PrinterDocument.Print();
            //printdialog.ShowDialog();
        }
        /// <summary>
        /// 打印预览
        /// </summary>
        public void print_Preview()
        {          
            printpredialog.ShowDialog();
        }
        private void print_print(object sender,PrintPageEventArgs pe)
        {
            int print_Y = 3;//打印当前行的Y坐标
            int print_X = 8;//打印当前行的Y坐标
            if (Width <= 0) Width = PrinterDocument.DefaultPageSettings.PaperSize.Width - 
                                    PrinterDocument.DefaultPageSettings.Margins.Left -
                                    PrinterDocument.DefaultPageSettings.Margins.Right;
            if (PrintImage != null)
            {
                if (pe.PageBounds.Width < PrintImage.Width)
                {
                   //Rectangle rect = new Rectangle(0, 0, 100, 100); 
                //bmp.RotateFlip(RotateFlipType.Rotate90FlipNone); 
                   PrintImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                   pe.Graphics.DrawImage(PrintImage, 2, 2);
                }
                else
                {
                    pe.Graphics.DrawImage(PrintImage, 1, 1);
                }
            }
            foreach (var pp in PrinterItems)
            {
                SizeF sf = pe.Graphics.MeasureString(pp.value, pp.font);
                if (pp.textalgin == "left")
                {
                    pe.Graphics.DrawString(pp.value, pp.font, pp.brush, new Point(print_X,print_Y));//左对齐

                    
                }
                else if (pp.textalgin == "center")
                {
                    int w = (int)sf.Width;
                    pe.Graphics.DrawString(pp.value, pp.font, pp.brush, new Point((Width - w) / 2, print_Y));//左对齐

                }
                else
                {
                    int w = (int)sf.Width;
                    pe.Graphics.DrawString(pp.value, pp.font, pp.brush, new Point(Width - w, print_Y));//右对齐

                }
                print_Y += (int)sf.Height + 3;//移至下一行

            }
        }
    }
}
