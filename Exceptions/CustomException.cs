namespace BDRDExce.Exceptions
{
    public class CustomException : Exception
    {
        public string Code { get; set; }
        public CustomException(string message) : base(message)
        {
            Code = "ERR_001";
        }
        public CustomException(string code, string message) : base(message)
        {
            Code = code;
        }
    }
}