using BG.Infrastructure.Process.NCommon.Data;
using BG.LicenseDAL.Models;
using NCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Extensions;
using System.Web;

namespace BG.Domain.Authentication.Repositories.Impl
{
    public class LogonHistoryRepository : ILogonHistoryRepository
    {
        readonly IUnitOfWorkScopeFactory _scopeFactory;
        readonly IRepository<LogonHistory> _repository;

        public LogonHistoryRepository(IUnitOfWorkScopeFactory scopeFactory, IRepository<LogonHistory> repository)
        {
            _scopeFactory = scopeFactory;
            _repository = repository;
        }

        public void Save(LogonHistory history)
        {
            Guard.AssertNotNull(history, "Не определен логон для сохранения");

            using (var scope = _scopeFactory.Create())
            {
                history.DateTime = DateTime.Now;
                history.Host = HttpContext.Current.GetUserHostName();
                history.IP = HttpContext.Current.GetVisitorIPAddress();//Processor.GetClientIP(HttpContext.Current);
                                                                       //history.Comment = string.Format("{0}; IP={1}; {2}", customComment,
                                                                       //    (HttpContext.Current.Request != null ? HttpContext.Current.Request.UserHostAddress : ""),
                                                                       //    comment);

                //history.LogonApplicationID = LogonApplication.iCRM;
                history.InternalIP = HttpContext.Current.GetUserHostAddress();

                _repository.Add(history);
                scope.Commit();
            }
        }
    }
}
