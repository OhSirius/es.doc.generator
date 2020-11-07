﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Domain.DocumentsGenerator.Formatters
{
    public interface IFormatter
    {
        string Format(string str, object param);
    }
}
