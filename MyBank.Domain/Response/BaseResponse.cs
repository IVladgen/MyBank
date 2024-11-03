using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Domain.Response
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess => Errors.Count == 0;
        public List<string> Errors { get; private set; }
        public BaseResponse() => Errors = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T Data { get; private set; }

        public void AddData(T data)
        {
            Data = data;
        }

    }
}
