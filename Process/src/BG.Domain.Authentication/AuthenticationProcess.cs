using BG.Domain.Authentication.Repositories;
using BG.Infrastructure.Process.NCommon.Data;
using BG.Infrastructure.Process.Process;
using BG.Infrastructure.Process.Transactions;
using BG.LicenseDAL.Models;
using NCommon.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BG.Extensions;
using BG.Domain.Authentication.Configurations;

namespace BG.Domain.Authentication
{
    public class AuthenticationProcess : IAuthenticationProcess
    {
        readonly Logger _logger;
        readonly ILogonHistoryRepository _logonHistoryRepository;
        readonly IBusinessTransactionObservable _transaction;
        readonly IUnitOfWorkScopeFactory _scopeFactory;
        readonly IAccountRepository _accountRepository;
        readonly IAuthenticationConfiguration _configuration;

        public AuthenticationProcess(IAuthenticationConfiguration configuration, IUnitOfWorkScopeFactory scopeFactory, Logger logger, IAccountRepository accountRepository, ILogonHistoryRepository logonHistoryRepository, IBusinessTransactionObservable transaction)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _logonHistoryRepository = logonHistoryRepository;
            _accountRepository = accountRepository;
            _transaction = transaction;
            _configuration = configuration;
        }

        public string ErrorMessage { private set; get; }

        public bool IsValid { private set; get; }

        public void Logon(Guid? licenseGuid, LogonHistory history)
        {
            IUnitOfWorkScope scope = null;
            Account account = null;
            try
            {
                scope = _scopeFactory.Create();

                IsValid = true;
                _logger.Info($"Начало запроса авторизации для {licenseGuid}");
                if (!licenseGuid.HasValue)
                {
                    SetError("Не определен гуид лицензии");
                }
                else
                {
                    account = _accountRepository.GetByLicenseId(licenseGuid.Value);
                    if (account == null)
                    {
                        SetError($"Не найдена УЗ для {licenseGuid}");
                    }
                    else
                    {
                        var license = account.Licenses.FirstOrDefault(l => l.Guid == licenseGuid);
                        if (license == null)
                        {
                            SetError($"Не найдена лицензия по гуиду {licenseGuid}");
                        }
                        else if (!license.Access)
                        {
                            SetError($"Для УЗ {account.Id} заблокирован доступ для лицензии {licenseGuid}");
                        }
                    }
                }

                history.Account = account;
                _logonHistoryRepository.Save(history);

                scope.Commit();
                _logger.Info($"Завершение запроса авторизации для {licenseGuid} и УЗ {account.Return(a=>a.Id)} и логона {history.Return(h=>h.Id)}");
            }
            catch (Exception e)
            {
                SetError(e.ToString(), true);
            }
            finally
            {
                scope.Do(s => s.Dispose());
            }
        }

        void SetError(string message, bool isException = false)
        {
            IsValid = false;
            if (_configuration.DebugMode)
                ErrorMessage = message;
            else
                ErrorMessage = isException ? "Произошла ошибка на сервере. Просьба попробовать позже." : "Не пройдена аутентификация. Возможно лицензионный ключ истек";
            _logger.Log(isException ? LogLevel.Error : LogLevel.Info, $"Завершение запроса авторизации: {message}");
        }


    }
}
