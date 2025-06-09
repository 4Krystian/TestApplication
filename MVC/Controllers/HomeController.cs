using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MVC.Code;
using MVC.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(BusinessLogic.Infrastructure.EFDbContext EFDbContext, BusinessLogic.Infrastructure.Config Config, ILogger<BaseController> Logger) : base(EFDbContext, Config, Logger)
        {

        }

        public IActionResult Index()
        {
            BusinessLogic.OrderProcessor orderProcessor = new BusinessLogic.OrderProcessor(EFDbContext, Config);

            var orders = orderProcessor.GetOrdersByEF();


            return View(orders.ToList());
        }

        public IActionResult WebAPI()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{this.Config.WebAPIURL}/Order");

            HttpResponseMessage httpResponseMessage = client.GetAsync("").Result;

            string result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            List<BusinessLogic.Models.OrderWebAPI> orders = BusinessLogic.Tools.DeserializeJson<List<BusinessLogic.Models.OrderWebAPI>>(result);

            return View(orders);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
