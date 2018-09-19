using Codeplex.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Service
{
    public class PrintService
    {
        protected Model.PrintOrder printOrder;
        protected string err;
        public string Err { get { return err; } }

        public bool SetPrintOrder(string printOrder)
        {
            try
            {
                this.printOrder = JsonConvert.DeserializeObject<Model.PrintOrder>(printOrder);
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
            return true;
        }
    }
}
