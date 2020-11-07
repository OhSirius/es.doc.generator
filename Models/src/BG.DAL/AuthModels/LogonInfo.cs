using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BG.DAL.AuthModels
{
    [DataContract(Name = "LogonInfo", Namespace = "BG.Authentication")]
    public class LogonInfo
    {
        [DataMember(EmitDefaultValue = false)]
        public Guid LicenseGuid { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public LogonHistory History { set; get; }
    }
}
