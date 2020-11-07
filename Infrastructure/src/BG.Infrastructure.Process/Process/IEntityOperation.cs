using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Process
{
    public interface IEntityOperation
    {
        bool IsValid { get; }

        string ErrorText { get; }
    }
}
