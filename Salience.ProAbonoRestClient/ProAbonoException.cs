using System;
using System.Linq;

namespace ProAbono
{
    public class ProAbonoException : Exception
    {
        public ProAbonoException(int statusCode, Error[] errors)
            : base(errors.First().Message)
        {
            this.StatusCode = statusCode;
            this.Errors = errors;
            this.Error = errors.First();
        }

        public int StatusCode { get; set; }
        public Error Error { get; set; }
        public Error[] Errors { get; set; }
    }
}