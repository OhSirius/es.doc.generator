using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Logging.Adapters.EF.Revisions
{
    /// <summary>
    /// Хранит соответствие названий полей объекта и полей ревизии: 
    ///     key - поле объекта
    ///     value - поле ревизии
    /// </summary>
    public class EntityRevisionDescription
    {
        public string EntityName { set; get; }
        //public object EntityID { set; get; }

        //public Dictionary<string, object> Values { set; get; }
        public Dictionary<string, string> Fields { set; get; }
    }

    public class EntityRevisionDescriptionCollection : Dictionary<Type, EntityRevisionDescription>
    {

    }
}
