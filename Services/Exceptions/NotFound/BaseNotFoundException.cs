using System;
using System.Net;

namespace Services.Exceptions.BadRequest
{
    public class BaseNotFoundException : Exception
    {
        public int HttpCode { get { 
                return Convert.ToInt32(HttpStatusCode.NotFound); 
            } 
        }
        public string CustomMessage { get; set; }
    }
}
