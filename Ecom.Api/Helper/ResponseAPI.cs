using Microsoft.AspNetCore.Http.HttpResults;

namespace Ecom.Api.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message??GetMessageFromStatusCode(statusCode);
        }
        private string GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "Done",
                400=> "Bad Request",
                401=>"Un Authorized",
                404 => "Not Found",
                500 =>"Server Error",
                _=>null!,
            };
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
