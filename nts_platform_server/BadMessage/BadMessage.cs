using System;
namespace nts_platform_server.BadMessage
{
    public class BadMessage
    {
        public BadMessage()
        {
        }

        public object InvalidCredentialsEmail()
        {
            return new { message = "Invalid Credentials Email" };
        }

        public object InvalidCredentialsPassword()
        {
            return new { message = "Invalid Credentials Password" };
        }
    }
}
