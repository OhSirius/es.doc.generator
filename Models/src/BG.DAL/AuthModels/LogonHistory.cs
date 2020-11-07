using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BG.DAL.AuthModels
{
    [DataContract(Name = "LogonHistory", Namespace = "BG.Authentication")]
    public class LogonHistory
    {
        [DataMember(EmitDefaultValue = false)]
        public string MachineName { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string UserName { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string UserDomainName { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string HardwareId { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string CurrentVersion { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string ApplicationName { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string ProductName { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string ProductId { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string CSDBuildNumber { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string CSDVersion { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string CurrentBuild { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string RegisteredOwner { set; get; }

        [DataMember(EmitDefaultValue = false)]
        public string ServerUrl { set; get; }

    }
}
