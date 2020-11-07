using Akka.Actor;
using BG.Application.RunConsole.Processes;
using BG.DAL.Models;
using BG.Domain.DocumentsGenerator;
using BG.Infrastructure.Common.Processes;
using BG.Infrastructure.Morphology;
using BG.Infrastructure.Process.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Streams;

namespace BG.Application.RunConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //var test1 = Declension.Declension.GetDeclension("Железняков Юрий Юрьевич", Declension.DeclensionCase.Datel);
                //var test2 = Declension.Declension.GetDeclension("Кузнецова Ольга Юрьевна", Declension.DeclensionCase.Datel);
                //var tgs = new Declension().GetDeclension("Ромашка Вася Викторовна", DeclensionCase.Datel, DeclensionGender.NotDefind, "23", 1);
                //var tgs = new BG.Infrastructure.Morphology.Padeg.Declension().GetApproimentDeclension("ведущий программист", DeclensionCase.Datel);
                //var test2 = new BG.Infrastructure.Morphology.Padeg.Declension().GetPersonNameDeclension("Кузнецова Ольга Юрьевна", DeclensionCase.Datel);
                //var test3 = new BG.Infrastructure.Morphology.Padeg.Declension().GetPersonNameBy("Кузнецова Ольга Юрьевна");

                var process = GetProcess(args);
                if (process == null)
                {
                    Console.WriteLine("Не удалось извлечь аргументы");
                    Console.WriteLine("Завершить выполнение...");
                    Console.ReadLine();
                    return;
                }

                process.Processing += Processing;
                process.Execute(default(CancellationToken)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка: {e}");
            }
            finally
            {
                Console.WriteLine("Завершить выполнение...");
                Console.ReadLine();
            }
        }

        private static void Processing(ProcessEventsArgs obj)
        {
            if (obj.Percent == 0)
                Console.WriteLine($"{obj.Display}");
            else
                Console.WriteLine($"Прогрес: {obj.Percent} %, {obj.Display}");
        }

        static IProcess GetProcess(string[] args)
        {
            IProcess process = null;
            object options = new DocumentsGeneratorOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var @params = (DocumentsGeneratorOptions)options;
                process = Domain.DocumentsGenerator.Bootstrap.Configuration.Configure<Person>(@params.SourcePath, @params.TemplatePath, @params.OutPath);
            }

            return process;
        }

    }
}
