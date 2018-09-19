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
        protected string err;
        protected PrintDocument pd;
        protected string fontFamily = "正常";
        public string Err { get { return err; } }

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
                this.err = ex.Message;
                MyLogService.Error("反序列化打印指令出错", ex);
                return false;
            }

        }
        public bool TestPrintOrder()
        {
            Double id = printOrder.id;
            pd = new PrintDocument();
            this.setPageSize();
            pd.PrintPage += pd_PrintPage;
            pd.PrintController = new StandardPrintController();             //去掉打印弹出框
            pd.Print();
            return true;
        }
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawString("物料转运标识单", new Font(fontFamily, 20), System.Drawing.Brushes.Black, 300, 67);
        }

        protected void setPageSize()
        {
            string pageName = printOrder.id.ToString();
            int width = toInches(printOrder.page.width);
            int height = toInches(printOrder.page.height);
            PaperSize ps = new PaperSize("Custum", width, height);
            pd.DocumentName = pageName;
            pd.DefaultPageSettings.PaperSize = ps;
            //使用自定义页面设置
            pd.DefaultPageSettings.PaperSize.RawKind = 256;
        }

        public int toInches(double mm)
        {
            return (int)Math.Round(mm*3.937);
        }
    }
}
