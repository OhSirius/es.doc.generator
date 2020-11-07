using BG.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.DocumentsGenerator.Repositories
{
    /// <summary>
    /// Репозиторий извлечения данных страницами
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface ISourceRepository<TData> where TData:class
    {
        int GetCount();

        IEnumerable<TData> GetPage(int pageNumber, int pageSize);
    }
}
