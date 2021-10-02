using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Maxi.Models.Infrastructures
{
    public class CurrentProcess
    {
        public CurrentProcess()
        {
            Errors = new List<ErrorMessage>();
            Items = new Dictionary<string, object>();
        }
        public long AccountId { get; set; }
        public long TeamId { get; set; }
        public int LevelId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<ErrorMessage> Errors { get; }

        public void AddError(string errorMessage, params object[] traceKeys)
        {
            Errors.Add(new ErrorMessage
            {
                Message = errorMessage,
                TraceKeys = traceKeys != null ? traceKeys.ToList() : null
            });
        }

        public bool HasError { get { return Errors.Count > 0; } }

        public ErrorMessage ToError()
        {
            if (HasError)
                return Errors[0];

            return null;
        }
        public List<ErrorMessage> ToErrors()
        {
            if (HasError)
                return Errors;

            return null;
        }

        public Dictionary<string, object> Items { get; }

        public void AddItem(string key, object value)
        {
            Items.Add(key, value);
        }
        public T GetItem<T>(string key)
        {
            return Items.ContainsKey(key) ? (T)Items[key] : TypeExtensions.GetDefaultValue<T>();
        }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }
        public List<object> TraceKeys { get; set; }
    }
    public static class TypeExtensions
    {
        public static T GetDefaultValue<T>()
        {
            return (T)GetDefaultValue(typeof(T));
        }
        public static object GetDefaultValue(this Type type)
        {
            return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
        }
    }

    public class ResponseJsonModel
    {
        public ErrorJsonModel error { get; set; }
        public bool success { get; set; }
    }
    public class ResponseJsonModel<T> : ResponseJsonModel
    {
        public T data { get; set; }
    }
    public class ResponseActionJsonModel : ResponseJsonModel
    {
        public bool? success { get; set; }
    }

    public class ErrorJsonModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<object> trace_keys { get; set; }
    }
}
