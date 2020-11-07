using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.DAL.Models;
using BG.Extensions;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using BG.DAL.Attributes;

namespace BG.Domain.DocumentsGenerator.Repositories.Impl
{
    public class ExcelSourceRepository<TData> : ISourceRepository<TData> where TData : class, new()
    {
        readonly string _sourcePath;

        public ExcelSourceRepository(string sourcePath)
        {
            _sourcePath = sourcePath;
        }

        public int GetCount()
        {
            return DataFetch.Count();
        }

        public IEnumerable<TData> GetPage(int pageNumber, int pageSize)
        {
            return DataFetch.Skip(pageNumber * pageSize)
                                           .Take(pageSize).ToList();
        }

        IEnumerable<TData> _data;
        IEnumerable<TData> DataFetch
        {
            get
            {
                if (_data == null)
                {
                    var xlApp = new Application();
                    Workbook xlWorkbook = xlApp.Workbooks.Open(_sourcePath);
                    _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                    Range xlRange = xlWorksheet.UsedRange;

                    //Перваястрока метаданные
                    var metaData = new Dictionary<int, string>();
                    var fields = new TData().GetFields<SourceAttribute>(s => s.Alias).ToArray();
                    for (int j = 1; j <= xlRange.Columns.Count; j++)
                    {
                        var columnName = xlRange.Cells[1, j].Value2.ToString();
                        if (fields.Any(f=>f.Key == columnName))
                            metaData.Add(j, fields.First(f=>f.Key == columnName).Value);
                    }

                    var records = new List<TData>();
                    for (int i = 2; i <= xlRange.Rows.Count; i++)
                    {
                        var values = new Dictionary<int, string>();
                        for (int j = 1; j <= xlRange.Columns.Count; j++)
                        {
                            //new line
                            //if (j == 1)
                            //    Console.Write("\r\n");

                            //write the value to the console
                            //if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            //    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");

                            //add useful things here! 
                            values.Add(j, xlRange.Cells[i, j].Value2.ToString());
                        }

                        var record = new TData();
                        record.SetValues(values.Select(v => new KeyValuePair<string, object>(metaData[v.Key], v.Value)).ToDictionary(k => k.Key, v => v.Value));
                        records.Add(record);
                    }

                    //close and release
                    xlWorkbook.Close();
                    Marshal.ReleaseComObject(xlWorkbook);

                    //quit and release
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlApp);

                    _data = records;
                }

                return _data;
            }
        }

    }
}
