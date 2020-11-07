using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process
{
    public interface IErrorOperation
    {
        bool IsValid { set; get; }

        string ErrorText { set; get; }
    }
}
