using BG.Infrastructure.Morphology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Extensions;

namespace BG.DAL.Attributes
{
    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class TemplateAttribute : Attribute
    {
        public TemplateAttribute(string alias)
        {
            Alias = alias;
        }

        public string Alias { set; get; }

    }
}
