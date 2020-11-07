using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BG.Infrastructure.Process.Logging
{
    public enum RevisionOperationTypes { }

    public interface IRevisionEntity
    {
        RevisionOperationTypes RevisionOperationType { set; get; }
        string RevisionComment { set; get; }

        string FormatRevisionFieldName(string fieldName);

        bool IgnoreRevisionEntityFieldName(string fieldName);
    }
}
