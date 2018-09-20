using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace web_print_agent.Service
{
    public class PrintService
    {
        protected dynamic printOrder;
        protected string msg;
        protected PrintDocument pd;
        protected string fontFamily = "正常";
        protected int Pixel;
        public string Msg { get { return msg; } }

        public bool SetPrintOrder(string printOrder)
        {
            try
            {
                new Utils.Notification().Open("打印通知", "已接到打印指令，正在打印");
                this.printOrder = DynamicJson.Parse(printOrder);
                MyLogService.Print(printOrder);
                return true;
            }
            catch (Exception ex)
            {
                this.msg = ex.Message;
                MyLogService.Error("反序列化打印指令出错", ex);
                new Utils.Notification().Open("打印通知", "反序列化打印指令出错");
                return false;
            }

        }
        public bool TestPrintOrder()
        {
            Double id = printOrder.id;
            pd = new PrintDocument();
            this.setPageSize();
            Model.PrintRe printRe = new Model.PrintRe();
            printRe.id = id;
            try
            {
                Pixel = pd.DefaultPageSettings.PrinterResolution.X;// 获取分辨率
                pd.PrintPage += pd_PrintPage;
                pd.PrintController = new StandardPrintController();             //去掉打印弹出框
                pd.Print();
                printRe.code = 0;
                new Utils.Notification().Open("打印通知", "打印完成");
                printRe.msg = "打印成功";
            }
            catch (Exception ex)
            {
                printRe.code = 1;
                printRe.msg = ex.Message;
                MyLogService.Error("解析打印指令出错", ex);
                new Utils.Notification().Open("打印通知", "解析打印指令出错");
                return false;
            }
            finally {
                this.msg = DynamicJson.Serialize(printRe);
                MyLogService.Print(this.msg);
            }

            return true;
        }
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            var content = printOrder.content;
            foreach (var item in content)
            {
                string type = item.type;
                switch (type.ToLower())
                {
                    case "text":
                        printText(g,item);
                        break;
                    case "line":
                        printLine(g, item);
                        break;
                    case "qrcode":
                        printBarCode(g, item, BarcodeFormat.QR_CODE);
                        break;
                    case "datamatrix":
                        printBarCode(g, item, BarcodeFormat.DATA_MATRIX);
                        break;
                    case "barcode":
                        printBarCode(g, item, BarcodeFormat.CODE_128);
                        break;
                    case "image":
                        printImage(g, item);
                        break;
                    default:
                        MyLogService.Print("无法识别的打印命令：" + type);
                        break;
                }
            }
        }

        private void printImage(Graphics g, dynamic item)
        {
            string text = item.text;
            float width = toInches(item.width);
            float height = toInches(item.height);
            float x = toInches(item.x);
            float y = toInches(item.y);
            System.Net.WebRequest webreq = System.Net.WebRequest.Create(text);
            System.Net.WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            Image image = Image.FromStream(stream);
            stream.Close();
            g.DrawImage(image, x, y, width, height);
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        /// <param name="g"></param>
        /// <param name="item"></param>
        private void printBarCode(Graphics g, dynamic item, BarcodeFormat format)
        {
            float width = toInches(item.width);
            float height = toInches(item.height);
            float x = toInches(item.x);
            float y = toInches(item.y);
            string text = item.text;
            g.DrawImage(newBarcode(format, text,(int)width,(int)height),x, y, width, height);
        }

        /// <summary>
        /// 打印线条
        /// </summary>
        /// <param name="g"></param>
        /// <param name="item"></param>
        private void printLine(Graphics g, dynamic item)
        {
            float x1 = toInches(item.x1);
            float y1 = toInches(item.y1);
            float x2 = toInches(item.x2);
            float y2 = toInches(item.y2);
            Pen pen = new Pen(Color.Black, 1);
            g.DrawLine(pen, x1,y1,x2,y2);
        }

        /// <summary>
        /// 打印文字
        /// </summary>
        /// <param name="g"></param>
        /// <param name="item"></param>
        private void printText(Graphics g, dynamic item)
        {
            string text = item.text;
            int size = toInt(item.size);
            float x = toInches(item.x);
            float y = toInches(item.y);
            g.DrawString(text, new Font(fontFamily, size), System.Drawing.Brushes.Black, x, y);
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="format">条码类型</param>
        /// <param name="text">内容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        protected Bitmap newBarcode(BarcodeFormat format, string text, int width, int height)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = format;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                DisableECI = true,//设置内容编码
                CharacterSet = "UTF-8", //设置二维码的宽度和高度
                Width = width,
                Height = height,
                Margin = 1//设置二维码的边距,单位不是固定像素
            };
            writer.Options = options;
            Bitmap map = writer.Write(text);
            return map;
        }

        /// <summary>
        /// 设置页面尺寸
        /// </summary>
        protected void setPageSize()
        {
            string pageName = printOrder.id.ToString();
            int width = tomInches(printOrder.page.width);
            int height = tomInches(printOrder.page.height);
            PaperSize ps = new PaperSize("Custum", width, height);
            pd.DocumentName = pageName;
            pd.DefaultPageSettings.PaperSize = ps;
            //使用自定义页面设置
            pd.DefaultPageSettings.PaperSize.RawKind = 256;
        }

        public int tomInches(double mm)
        {
            return (int)Math.Round(mm*3.937);
        }

        public float toInches(double num)
        {
            return Convert.ToSingle(num * 3.937008);
        }

        public int toInt(dynamic i)
        {
            return Convert.ToInt32(i);
        }
    }
}
