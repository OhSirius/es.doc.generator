using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.DAL.Attributes
{
    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class SourceAttribute : Attribute
    {

        public SourceAttribute(string alias)
        {
            Alias = alias;
        }

        public string Alias { set; get; }

    }
}
