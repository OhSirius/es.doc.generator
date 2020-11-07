using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Application.RunConsole.Processes
{
    class DocumentsGeneratorOptions
    {
        [Option('s', "source", Required = true, HelpText = "Файл источник")]
        public string SourcePath { get; set; }

        [Option('t', "template", HelpText = "Файл шаблона")]
        public string TemplatePath { get; set; }

        [Option('o', "out", HelpText = "Папка с результатами")]
        public string OutPath { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
