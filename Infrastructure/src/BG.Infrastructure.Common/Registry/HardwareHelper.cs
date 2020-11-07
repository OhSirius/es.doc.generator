using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Common.Registry
{
    public static class HardwareHelper
    {
        public static string GetId()
        {
            //string hash = "";
            //ManagementObjectSearcher searcher = null;
            //try
            //{
            //    string hardwareId = "";

            //    //процессор
            //    searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            //    foreach (ManagementObject queryObj in searcher.Get())
            //    {
            //        hardwareId = queryObj["ProcessorId"].ToString();
            //        break;
            //    }

            //    //мать
            //    searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_Card");

            //    foreach (ManagementObject queryObj in searcher.Get())
            //    {
            //        hardwareId = string.Format("{0} {1} {2}", hardwareId, queryObj["Manufacturer"].ToString().Trim(),
            //                                   queryObj["Product"].ToString().Trim());
            //        break;
            //    }
            //    if (hardwareId != "")
            //        hash = FastString.GetStringHashAsString(hardwareId);
            //    searcher.Dispose();
            //}
            //catch { }
            //finally
            //{
            //    if (searcher != null)
            //        searcher.Dispose();
            //}
            //return hash;

            string hardwareId = "unknown";
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor"))
                {
                    foreach (var o in searcher.Get())
                    {
                        var queryObj = (ManagementObject)o;
                        hardwareId = queryObj["ProcessorId"].ToString();
                    }
                }
            }
            catch
            {
                return "Exception";
            }
            return hardwareId;
        }
    }
}
