using BG.Infrastructure.Common.Configuration;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Common.Logging
{
    public static class NLogUtils
    {

        private static Logger _GetLoggerFromModuleConfig(String moduleFileName, String loggerName)
        {
            return new LogFactory(new XmlLoggingConfiguration(CustomServiceConfiguration.GetConfigFileName(moduleFileName))).GetLogger(loggerName);
        }

        public static Logger GetLoggerFromModuleConfig(String moduleFileName, String loggerName)
        {
            return _GetLoggerFromModuleConfig(moduleFileName, loggerName);
        }

        public static Logger GetLoggerFromModuleConfig(String loggerName = null)
        {
            StackTrace st = new StackTrace();
            StackFrame f = st.GetFrame(1);
            return GetLoggerFromModuleConfig(
               System.IO.Path.GetFileName(f.GetMethod().DeclaringType.Assembly.Location),
               loggerName ?? f.GetMethod().DeclaringType.Name
            );
        }

    }

}
