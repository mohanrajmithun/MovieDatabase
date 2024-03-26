namespace MovieApi
{
    public class CustomExceptionResponse
    {


        public CustomExceptionResponse(int statuscode, string message, string details = null, string innerexception = null)
        {
            StatusCode = statuscode;
            Message = message;  
            Details = details;
            innerException = innerexception;
        }
        public int StatusCode { get; set; } 

        public string Message { get; set; } 

        public string Details { get; set; }

        public string innerException { get; set; }


    }
}
