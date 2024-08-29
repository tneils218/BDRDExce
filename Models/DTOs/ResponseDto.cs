using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDRDExce.Models.DTOs
{
    public class ResponseDto

    {
        public string Code { get; set; } = "OK";
        public string Message { get; set; }
        public object Data { get; set; }
        public ResponseDto()
        {
        }
        public ResponseDto(string message, object data = null)
        {
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
            Message = "Success";
            Data = data;
        }
    }

}