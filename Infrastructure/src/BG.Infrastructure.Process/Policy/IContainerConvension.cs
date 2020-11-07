using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.BusinessProcess.Policy
{
    public class ConvensionsCollection : List<IChildConvension>
    {
        public ConvensionsCollection()
        {
        }

        public ConvensionsCollection(IEnumerable<IChildConvension> convs)
            : base(convs)
        {
        }
    }

    public interface IContainerConvension
    {
        //ConvensionsCollection Childs { get; }

        void MakeAction(Action<IChildConvension> action);
    }
}
