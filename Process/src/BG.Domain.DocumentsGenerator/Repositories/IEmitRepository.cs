using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.DocumentsGenerator.Repositories
{
    public interface IEmitRepository:IDisposable
    {
        string TemplatePath { set; get; }

        void Replace(string sourceStr, string targetStr);

        void Save(string toPath);

        void Close();
    }
}
