using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.DAL.Attributes;
using BG.Infrastructure.Morphology;
using BG.Extensions;

namespace BG.Domain.DocumentsGenerator.Formatters.Impl
{
    public class FormatterFactory : IFormatterFactory
    {
        private readonly IDictionary<FilterType, IFormatter> formatters;
        private readonly IFormatter defaultFormatter;

        public FormatterFactory(IDictionary<FilterType, IFormatter> formatters, IFormatter @default)
        {
            this.formatters = formatters;
            defaultFormatter = @default;
        }

        public IFormatter Select(FilterType type)
        {
            Guard.Against<ArgumentException>(formatters.IsNullOrEmpty(), "Не определен список форматтеров");
            //Guard.Against<ArgumentException>(!formatters.ContainsKey(type), $"Отсутствует форматер для {type}");

            return !formatters.ContainsKey(type) ? defaultFormatter : formatters[type];
        }
    }
}
