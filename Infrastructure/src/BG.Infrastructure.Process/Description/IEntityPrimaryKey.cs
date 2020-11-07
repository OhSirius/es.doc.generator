using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Description
{
    public interface IEntityPrimaryKey
    {
        object EntityPrimaryKey { set; get; }
    }
}
