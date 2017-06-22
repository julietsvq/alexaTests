using System.Net.Http;
using System.Web.Http;

namespace AlexaTests.Controllers
{
    public class AlexaController : ApiController
    {
        [HttpPost, Route("api/alexa/helloworld")]
        public HttpResponseMessage PostHelloWorld()
        {
                var speechlet = new SessionSpeechlet();
                return speechlet.GetResponse(Request);
        }
    }
}
