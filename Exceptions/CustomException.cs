using BDRDExce.Models.DTOs;

namespace BDRDExce.Exceptions
{
    public class CustomException : Exception
    {
        // Custom properties
        public string Code { get; set; }
        public object Data { get; set; }

        // Constructor that sets a default error code
        public CustomException(string message) : base(message)
        {
            Code = "ERR_001";
        }

        // Constructor that allows setting a custom error code and message
        public CustomException(string code, string message) : base(message)
        {
            Code = code;
        }

        // Constructor that allows setting a custom error code, message, and additional data
        public CustomException(string code, string message, object data = null) : base(message)
        {
            Code = code;
            Data = data;
        }
        public CustomException(string message, object data = null) : base(message)
        {
            Data = data;
        }
        public ResponseDto ToResponseDto()
        {
            return new ResponseDto
            {
                Code = Code,
                Message = Message,
                Data = Data
            };
        }
    }
}
