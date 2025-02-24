using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

namespace TechLibrary.Exception
{
    public class InvalidLoginException : TechLibraryException
    {
        public InvalidLoginException() : base("Email and/or invalid password") { }
        public override List<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
