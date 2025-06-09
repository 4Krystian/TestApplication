using BusinessLogic.Infrastructure;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Code
{
    public abstract class BaseController : ControllerBase
    {
        protected BusinessLogic.Infrastructure.EFDbContext EFDbContext
        {
            get;
            private set;
        }

        protected BusinessLogic.Infrastructure.Config Config { private set; get; }

        protected readonly ILogger<BaseController> Logger;

        protected BaseController(BusinessLogic.Infrastructure.EFDbContext EFDbContext, BusinessLogic.Infrastructure.Config Config, ILogger<BaseController> Logger) 
        { 
            this.EFDbContext = EFDbContext;
            this.Config = Config;
            this.Logger = Logger;
        }
    }
}
