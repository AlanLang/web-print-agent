using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace web_print_agent.Utils
{
    public class LocalPrinter
    {
        private static PrintDocument fPrintDocument = new PrintDocument();
        public const string NOPRINTER = "未设置打印机";
        //获取本机默认打印机名称
        public static String GetDefaultPrinter()
        {
            string printerName = fPrintDocument.PrinterSettings.PrinterName;
            return string.IsNullOrWhiteSpace(printerName) ? NOPRINTER : printerName;
        }
        public static List<String> GetLocalPrinters()
        {
            List<String> fPrinters = new List<String>();
            fPrinters.Add(GetDefaultPrinter()); //默认打印机始终出现在列表的第一项
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }
            return fPrinters;
        }

        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name); //调用win api将指定名称的打印机设置为默认打印机
    }
}
