using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using WebAPI.Code;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : BaseController
    {
        public OrderController(BusinessLogic.Infrastructure.EFDbContext EFDbContext, BusinessLogic.Infrastructure.Config Config, ILogger<BaseController> Logger): base(EFDbContext, Config, Logger)
        {

        }

        [HttpGet(Name = "GetOrders")]
        public IEnumerable<BusinessLogic.Models.OrderWebAPI> GetOrders()
        {
            BusinessLogic.OrderProcessor orderProcessor = new BusinessLogic.OrderProcessor(EFDbContext, Config);

            return orderProcessor.GetOrdersBySQL();
        }
    }
}
