using BG.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.DocumentsGenerator.Generators
{
    public class Result
    {
        public Result(string display)
        {
            Display = display;
        }

        public string Display { set;get; }
    }

    public interface IGenerator<TTemplateData> where TTemplateData:class
    {
        Result Run(TTemplateData data);

        Task<Result> RunAsync(TTemplateData data);
    }
}
