using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Models
{
    /// <summary>
    /// УЗ клиента
    /// </summary>
    [Table("BGAccount")]
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public Guid Guid { set; get; }

        public DateTime CreateDate { set; get; }

        public string Email { set; get; }

        public virtual ICollection<License> Licenses { set; get; }
    }
}
