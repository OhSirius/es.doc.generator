using BG.DAL.Attributes;
using BG.Infrastructure.Morphology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.DocumentsGenerator.Formatters
{
    public interface IFormatterFactory
    {
        IFormatter Select(FilterType type);
    }
}
