using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDRDExce.Models.DTOs
{
    public class ResponseDto

    {
        public string Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ResponseDto()
        {
            Code = "OK";
        }
        public ResponseDto(string message, object data = null)
        {
            Code = "OK";
            Message = message;
            Data = data;
        }
        public ResponseDto(string code, string message, object data = null)
        {
            Code = code;
            Message = message;
            Data = data;
        }
        public ResponseDto(object data = null)
        {
            Code = "OK";
            Message = "Success";
            Data = data;
        }
    }

}