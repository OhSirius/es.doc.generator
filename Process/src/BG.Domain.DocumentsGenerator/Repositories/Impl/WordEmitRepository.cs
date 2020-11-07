using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Threading;
using Path = System.IO.Path;

namespace BG.Domain.DocumentsGenerator.Repositories.Impl
{
    public class WordEmitRepository : IEmitRepository
    {
        static object _locker = new object();
        static Application _winword;

        bool _initialized = false;

        Guid _guid = Guid.NewGuid();

        public WordEmitRepository()
        {
        }

        public string TemplatePath { set; get; }

        string ToPath { set; get; }

        public void Replace(string sourceStr, string targetStr)
        {
            Guard.AssertNotEmpty(sourceStr, "Не определен источник");
            Guard.AssertNotEmpty(targetStr, "Не определен целевой");

            Initialize();

            var document = Open();
            Find findObject = document.Content.Find; //Application.Selection.Find;
            findObject.ClearFormatting();
            findObject.Text = sourceStr;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = targetStr;

            //Create a missing variable for missing value
            object missing = System.Reflection.Missing.Value;

            object replaceAll = WdReplace.wdReplaceAll;
            findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing);

        }

        public void Save(string toPath)
        {
            Guard.AssertNotEmpty(toPath, "Не определен целевой файл");

            Initialize();

            Document document = null;
            object missing = System.Reflection.Missing.Value;

            try
            {
                document = Open();

                //Create a missing variable for missing value

                document.SaveAs2(toPath);
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if (document != null)
                {
                    document.Close(ref missing, ref missing, ref missing);
                    document = null;
                    File.Delete(GetTempFile());

                }

            }
        }

        void Initialize()
        {
            if (_initialized)
                return;

            lock (_locker)
            {
                if (_winword == null)
                {
                    _winword = new Application();
                    //Set animation status for word application
                    _winword.ShowAnimation = false;

                    //Set status for word application is to be visible or not.
                    _winword.Visible = false;

                }

                _initialized = true;
            }
        }

        string GetTempFile()
        {
            return Path.Combine(Path.GetDirectoryName(TemplatePath), $"Temp_{_guid.GetHashCode()}_{Thread.CurrentThread.ManagedThreadId}");
        }

        //[ThreadStatic]
        Document _document;
        Document Open()
        {
            Guard.AssertNotEmpty(TemplatePath, "Не определен путь к исходному файлу");

            if (_document != null)
                return _document;

            //Create a missing variable for missing value
            object missing = System.Reflection.Missing.Value;

            //if (File.Exists(GetTempFile()))
            //{
            //    File.SetAttributes(GetTempFile(), FileAttributes.Normal);
            //}

            File.Copy(TemplatePath, GetTempFile(), true);
            //Create a new document
            //Microsoft.Office.Interop.Word.Document document = winword.Documents.Open(@"d:\temp1.docx", ref missing, ref missing, ref missing, ref missing);
            lock (_locker)
            {
                _document = _winword.Documents.Open(GetTempFile(), ref missing, ref missing, ref missing, ref missing);
            }

            //Microsoft.Office.Interop.Word.Range rng = document.Range(.Paragraphs[2].Range;
            return _document;

        }

        public void Close()
        {
            if (File.Exists(GetTempFile()))
                File.Delete(GetTempFile());
        }

        public void Dispose()
        {
            //Create a missing variable for missing value
            object missing = System.Reflection.Missing.Value;

            if (_document != null)
            {
                _document.Close(ref missing, ref missing, ref missing);
                _document = null;
            }

            lock (_locker)
            {
                if (_winword != null)
                {
                    _winword.Quit(ref missing, ref missing, ref missing);
                    _winword = null;
                }
            }
        }
    }
}
