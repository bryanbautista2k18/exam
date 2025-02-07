using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace api.DTOs 
{
    public interface IData
    {}
    
    public class Message
    {
        public string Type { get; private set; }
        public string Text { get; private set; }

        public Message(string type, string text)
        {
            Type = type;
            Text = text;
        }
    }

    public class Error 
    {
        public string Field { get; private set; }
        public string Text { get; private set; }

        public Error(string field, string text)
        {
            Field = field;
            Text = text;
        }
    }

    public class ResponseDTO<TData> where TData : class?
    {
        public bool Succeeded { get; }
        public Message? Message { get; }
        public IReadOnlyDictionary<string, TData>? Data { get; }
        public IReadOnlyList<Error>? Errors { get; }

        private ResponseDTO(
            bool succeeded, 
            Message? message = null, 
            Dictionary<string, TData>? data = null, 
            List<Error>? errors = null
        )
        {
            Succeeded = succeeded;
            Message = message;
            Data = data != null ? new ReadOnlyDictionary<string, TData>(data) : null;
            Errors = errors != null ? new ReadOnlyCollection<Error>(errors) : null;
        }

        public static ResponseDTO<TData> Success(
            Message? message = null,
            Dictionary<string, TData>? data = null
        )
        {
            return new ResponseDTO<TData>(true, message, data);
        } 

        public static ResponseDTO<TData> Failure(
            Message? message = null,
            List<Error>? errors = null
        )
        {
            return new ResponseDTO<TData>(false, message, null, errors);
        }
    }
}