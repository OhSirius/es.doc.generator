using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.DAL.Models;
using BG.Extensions;
using BG.DAL.Attributes;
using BG.Domain.DocumentsGenerator.Repositories;
using BG.DAL.Interfaces;
using System.IO;
using BG.Infrastructure.Morphology;
using BG.Domain.DocumentsGenerator.Formatters;
using System.Threading;

namespace BG.Domain.DocumentsGenerator.Generators.Impl
{
    public class Generator<TTemplateData> : IGenerator<TTemplateData> where TTemplateData : class, new()
    {
        static object _locker = new object();
        readonly string _templatePath;
        private readonly IFormatterFactory _formatterFactory;
        readonly IEmitRepository _emitRepository;
        readonly string _outPath;

        //public Generator(IEmitRepository emitRepository, string outPath, IFormatterFactory formatterFactory, string templatePath)
        public Generator(IEmitRepository emitRepository, Tuple<string, string> paths, IFormatterFactory formatterFactory)
        {
            _templatePath = paths.Item2;// templatePath;
            _formatterFactory = formatterFactory;
            _emitRepository = emitRepository;
            _outPath = paths.Item1; //outPath;
        }

        public Result Run(TTemplateData data)
        {
            Guard.AssertNotNull(data, "Не определен объект");
            Guard.AssertNotEmpty(_outPath, "Не определен путь вывода");

            try
            {
                _emitRepository.TemplatePath = _templatePath;
                var values = data.GetValues<TemplateAttribute>(a => a.Alias).ToArray();

                if (values.IsNullOrEmpty())
                    return new Result("Не определены значения");

                foreach (var value in values)
                    foreach (var filter in GetFilters(value.Key))
                    {
                        _emitRepository.Replace(GetPattern(value.Key, filter.Type, filter.Declension), _formatterFactory.Select(filter.Type).Format(value.Value as string, filter.Declension));
                    }

                var fileName = GetFileName(data);
                _emitRepository.Save(fileName);

                return new Result($"Создан файл: {fileName}");
            }
            catch(Exception ex)
            {
                if (!(ex is System.Runtime.InteropServices.COMException))
                    throw ex;

                return new Result("Ошибка");
            }
            finally
            {
                _emitRepository.Close();
            }
        }

        public async Task<Result> RunAsync(TTemplateData data)
        {
            return await Task.Run(() => Run(data));
        }

        string GetFileName(TTemplateData data) => (data as IDisplayEntity).Return(d => Path.Combine(_outPath, d.Display.Replace("\"","")), $"{Guid.NewGuid()}");

        string GetPattern(string alias, FilterType filterType, DeclensionCase declensionCase)
        {
            string pattern = alias;
            if (filterType != FilterType.None)
                pattern += $"|{filterType.GetDecription()}";

            if (declensionCase != DeclensionCase.NotDefind)
                pattern += $"|{declensionCase.GetDecription()}";

            return pattern;
        }

        FilterAttribute[] GetFilters(string alias)
        {
            if (!Descriptions.ContainsKey(alias) || Descriptions[alias].IsNullOrEmpty())
                return new FilterAttribute[] { new FilterAttribute(FilterType.None, DeclensionCase.NotDefind) };

            return Descriptions[alias];
        }

        static IDictionary<string, FilterAttribute[]> _descriptions;
        static IDictionary<string, FilterAttribute[]> Descriptions
        {
            get
            {
                if (_descriptions != null)
                    return _descriptions;

                lock (_locker)
                    if (_descriptions == null)
                        _descriptions = new TTemplateData().GetAttributes<TemplateAttribute, FilterAttribute, FilterAttribute>(a => a.Alias, d => d).ToDictionary(k => k.Key, v => v.Value);

                return _descriptions;
            }
        }


    }
}
