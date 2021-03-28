using System;
using System.Net;

namespace Services.Exceptions.BadRequest
{
    public class BaseBadRequestException : Exception
    {
        public int HttpCode { get {
                return Convert.ToInt32(HttpStatusCode.BadRequest);
            } 
        }
        public string CustomMessage { get; set; }
    }
}
