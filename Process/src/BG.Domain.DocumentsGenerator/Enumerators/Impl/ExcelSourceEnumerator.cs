using Microsoft.Office.Interop.Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.DAL.Attributes;
using BG.Extensions;
using System.Runtime.InteropServices;
using System.Threading;

namespace BG.Domain.DocumentsGenerator.Enumerators.Impl
{
    public class ExcelSourceEnumerator<TData> : IDataEnumerator<TData> where TData : class, new()
    {
        readonly string _sourcePath;
        Application xlApp;
        Workbook xlWorkbook;
        Range xlRange;
        Dictionary<int, string> metaData;
        int currentRowId = 2;

        TData _currentRecord;

        bool _initialized = false;

        public ExcelSourceEnumerator(string sourcePath)
        {
            _sourcePath = sourcePath;
            Reset();
        }

        public TData Current => _currentRecord;

        object IEnumerator.Current => _currentRecord;

        public int Count
        {
            get
            {
                Initialize();
                return xlRange?.Rows.Count - 1 ?? 0;
            }
        }

        public void Dispose()
        {
            //close and release
            if (xlWorkbook != null)
            {
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);
                xlWorkbook = null;
                xlRange = null;
            }

            //quit and release
            if (xlApp != null)
            {
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
                xlApp = null;
            }

        }

        public bool MoveNext()
        {
            lock (this)
            {
                Initialize();

                if (currentRowId > xlRange.Rows.Count)// || currentRowId > 10)
                {
                    _currentRecord = null;
                    //Thread.Sleep(1000);
                    return false;
                }

                var record = new TData();
                var values = new Dictionary<int, string>();
                for (int j = 1; j <= xlRange.Columns.Count; j++)
                {
                    values.Add(j, xlRange.Cells[currentRowId, j].Value2.ToString());
                }
                record.SetValues(values.Select(v => new KeyValuePair<string, object>(metaData[v.Key], v.Value)).ToDictionary(k => k.Key, v => v.Value));
                currentRowId++;
                _currentRecord = record;
                //Thread.Sleep(1000);

                return true;
            }
        }

        void Initialize()
        {
            if (_initialized)
                return;

            xlApp = new Application();
            xlWorkbook = xlApp.Workbooks.Open(_sourcePath);
            _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            xlRange = xlWorksheet.UsedRange;

            //Первая строка метаданные
            metaData = new Dictionary<int, string>();
            var fields = new TData().GetFields<SourceAttribute>(s => s.Alias).ToArray();
            for (int j = 1; j <= xlRange.Columns.Count; j++)
            {
                var columnName = xlRange.Cells[1, j].Value2.ToString();
                if (fields.Any(f => f.Key == columnName))
                    metaData.Add(j, fields.First(f => f.Key == columnName).Value);
            }

            _initialized = true;
        }

        public void Reset()
        {
            Dispose();
            _initialized = false;
        }
    }
}
