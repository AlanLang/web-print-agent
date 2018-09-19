using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Model
{
    public class PrintOrder
    {
        public double id { get; set; }
        public PrintPage page { get; set; }
        public List<PrintContent> content { get; set; }
    }

    public class PrintPage
    {
        public double width { get; set; }
        public double height { get; set; }
    }

    public class PrintContent
    {
        public string type { get; set; }
        public string text { get; set; }
        public double size { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public double x { get; set; }
        public double y { get; set; }
    }
}
