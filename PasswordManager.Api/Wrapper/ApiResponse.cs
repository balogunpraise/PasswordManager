using PasswordManager.Api.Dtos;

namespace PasswordManager.Api.Wrapper
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public ApiResponse(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(StatusCode);
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Not Authorized",
                201 => "No Content",
                404 => "Resource Not Found",
                500 => "Server Error",
                _=> string.Empty
            };
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
        public ApiResponse(int statusCode, T data, string message = null) 
            : base(statusCode, message)
        {
            Data = data;
        }
    }
}
