using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Models
{
    /// <summary>
    /// Типы лицензий:
    ///   1. LiteDocumentsGenerator
    ///   2. LiteDocumentsGenerator
    /// </summary>
    [Table("BGLicenseType")]
    public class LicenseType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public string Name { set; get; }

        //[Required]
        //[ForeignKey("Application")]
        //public int? ApplicationId { set; get; }

        public Application Application { set; get; }

    }
}
