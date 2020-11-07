using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Models
{
    [Table("BGLogonHistory")]
    public class LogonHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool Success { get; set; }

        public System.DateTime DateTime { get; set; }

        public string Host { get; set; }

        public string Comment { get; set; }

        public string UserName { get; set; }

        public Nullable<int> LogonType { get; set; }

        public string TokenID { get; set; }

        public string ApplicationName { get; set; }

        public string ApplicationVersion { get; set; }

        public string MachineName { get; set; }

        public string Login { get; set; }

        public string UserDomainName { get; set; }

        public string InternalIP { get; set; }

        public string HardwareId { get; set; }

        public string ProductName { get; set; }

        public string CSDBuildNumber { get; set; }

        public string CSDVersion { get; set; }

        public string CurrentBuild { get; set; }

        public string RegisteredOwner { get; set; }

        public string ProductId { get; set; }

        public Nullable<int> InternalId { get; set; }

        public Nullable<int> LogonApplication { get; set; }

        public string ServerUrl { get; set; }

        public string SessionID { get; set; }

        public string Region { get; set; }

        public string IP { get; set; }

        public Nullable<int> PingTime { get; set; }

        [ForeignKey("Account")]
        public virtual int? AccountId { get; set; }
        public virtual Account Account { get; set; }

    }
}
