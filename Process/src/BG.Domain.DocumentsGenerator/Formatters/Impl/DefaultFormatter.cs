using BG.Infrastructure.Morphology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.DocumentsGenerator.Formatters.Impl
{
    public class DefaultFormatter : IFormatter
    {
        public DefaultFormatter()
        {
        }
        public string Format(string str, object param)
        {
            return str ?? "";
        }
    }
}
