using Codeplex.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                this.printOrder = DynamicJson.Parse(printOrder);
                MyLogService.Print(printOrder);
                return true;
            }
            catch (Exception ex)
            {
                this.msg = ex.Message;
                MyLogService.Error("反序列化打印指令出错", ex);
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
                printRe.msg = "打印成功";
            }
            catch (Exception ex)
            {
                printRe.code = 1;
                printRe.msg = ex.Message;
                MyLogService.Error("解析打印指令出错", ex);
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
                switch (type)
                {
                    case "text":
                        printText(g,item);
                        break;
                    case "line":
                        printLine(g, item);
                        break;
                    default:
                        break;
                }
            }
        }

        private void printLine(Graphics g, dynamic item)
        {
            float x1 = toInches(item.x1);
            float y1 = toInches(item.y1);
            float x2 = toInches(item.x2);
            float y2 = toInches(item.y2);
            Pen pen = new Pen(Color.Black, 1);
            g.DrawLine(pen, x1,y1,x2,y2);
        }

        private void printText(Graphics g, dynamic item)
        {
            string text = item.text;
            int size = toInt(item.size);
            float x = toInches(item.x);
            float y = toInches(item.y);
            g.DrawString(text, new Font(fontFamily, size), System.Drawing.Brushes.Black, x, y);
        }

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
