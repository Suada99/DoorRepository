using System.Net;

namespace Application.Services
{
    public class CommandError
    {
        public HttpStatusCode HttpCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
