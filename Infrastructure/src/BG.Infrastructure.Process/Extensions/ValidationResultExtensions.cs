using NCommon.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BG.Extensions;

namespace BG.Infrastructure.Process.Extensions
{
    public static class ValidationResultExtensions
    {
        public static string GetValidationMessage(this ValidationResult result)
        {
            if (result == null || result.Errors.IsNullOrEmpty())
                return null;

            var sb = new StringBuilder();
            result.Errors.ForEach((error, i) => sb.AppendLine(string.Format("{0}. {1}", i + 1, error)));
            return sb.ToString();
        }

    }
}
