using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BG.DAL.AuthModels
{
    [DataContract(Name = "LogonResult", Namespace = "BG.Authentication")]
    public class LogonResult
    {
        [DataMember(EmitDefaultValue = false)]
        public bool Success { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string Message { set; get; }
    }
}
