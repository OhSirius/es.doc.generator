using BG.Infrastructure.Morphology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.DAL.Attributes
{
    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class DeclensionAttribute: Attribute
    {
        public DeclensionAttribute(DeclensionCase declensionCase)
        {
            Declension = declensionCase;
        }

        public DeclensionCase Declension { set; get; }
    }
}
