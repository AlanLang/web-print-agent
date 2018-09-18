using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_print_agent.Service
{
    public class MyLogService
    {
        static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        static readonly log4net.ILog logprint = log4net.LogManager.GetLogger("logprint");

        public static void Error(string ErrorMsg, Exception ex = null)
        {
            if (ex != null)
            {
                logerror.Error(ErrorMsg, ex);
            }
            else
            {
                logerror.Error(ErrorMsg);
            }
        }
        public static void Info(string Msg)
        {
            loginfo.Info(Msg);
        }

        public static void Print(string Msg)
        {
            logprint.Info(Msg);
        }
    }
}
