using Chat.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Chat.Web.Controllers
{
    public class MessagesController : Controller
    {
        private readonly Rabbitmq _rabbitmq;

        public MessagesController(
            IOptions<RabbitMqConfiguration> option)
        {
            _rabbitmq = new Rabbitmq(option);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Send(MessageInputModel message)
        {
            _rabbitmq.Send(message);
        }

        [HttpPost]
        public JsonResult Receive()
        {
            var message = _rabbitmq.Received();

            if (message == null)
                return Json(null);

            return Json(message.Content);
        }
    }
}
