using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.DocumentsGenerator.Enumerators
{
    public interface IDataEnumerator<TData> : IEnumerator<TData>
    {
        int Count { get; }
    }
}
