using BG.Infrastructure.Morphology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.DocumentsGenerator.Formatters.Impl
{
    public class PersonTotalNameDeclensionFormatter : IFormatter
    {
        private readonly IDeclension declension;

        public PersonTotalNameDeclensionFormatter(IDeclension declension)
        {
            this.declension = declension;
        }
        public string Format(string str, object param)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            return declension.GetPersonNameDeclension(str, (DeclensionCase)param);
        }
    }
}
