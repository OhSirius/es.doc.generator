using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LicenseDAL.Models
{
    /// <summary>
    /// Продукты
    /// </summary>
    [Table("BGApplication")]
    public class Application
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public string Name { set; get; }
    }
}
