using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Logging;

namespace Demo.Core
{
    public class LogHelper
    {
        public static void WriteLog(string txt)
        {
            ILog log = LogManager.GetLogger("log4netlogger");
            log.Error(txt);

        }
    }
}
