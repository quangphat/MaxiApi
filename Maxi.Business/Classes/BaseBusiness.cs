using Maxi.Models.Infrastructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maxi.Business.Classes
{
    public abstract class BaseBusiness
    {
        public readonly CurrentProcess _process;

        public BaseBusiness(CurrentProcess process)
        {
            _process = process;
        }

        protected T ToResponse<T>(T data, string error = null)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                return data;
            }
            AddError(error);
            return data;
        }

        public void AddError(string errorMessage, params object[] traceKeys)
        {
            _process.AddError(errorMessage, traceKeys);
        }

    }
}
