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
    /// Лицензии для УЗ клиентов
    /// </summary>
    [Table("BGLicense")]
    public class License
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public Guid Guid { set; get; }

        [Required]
        [ForeignKey("Account")]
        public int AccountId { set; get; }

        public Account Account { set; get; }

        public DateTime CreateDate { set; get; }

        public DateTime ChangeDate { set; get; }

        [Required]
        [ForeignKey("Application")]
        public int ApplicationId { set; get; }
        public Application Application { set; get; }

        [Required]
        [ForeignKey("Type")]
        public int TypeId { set; get; }
        public LicenseType Type { set; get; }

        /// <summary>
        /// Количество компьтеров для одновременного доступа
        /// </summary>
        public int Count { set; get; }

        /// <summary>
        /// Существует доступ к продукту
        /// </summary>
        public bool Access { set; get; }

    }
}
