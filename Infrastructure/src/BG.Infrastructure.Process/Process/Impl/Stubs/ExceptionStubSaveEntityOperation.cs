﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.Infrastructure.Process.Process.Impl.Stubs
{
    public class ExceptionStubSaveEntityOperation<TEntity> : ISaveEntityOperation<TEntity> where TEntity:class 
    {
        public TEntity SaveEntity(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool IsValid { get; private set; }
        public string ErrorText { get; private set; }
    }
}
