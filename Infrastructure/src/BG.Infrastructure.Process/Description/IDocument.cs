using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Description
{
    public interface IDocument
    {
        int Company { get; }
        int State { get; }
        int DocumentOwner { get; }
    }
}
