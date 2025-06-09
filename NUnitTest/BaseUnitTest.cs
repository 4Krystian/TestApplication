using BusinessLogic.Entities;
using BusinessLogic.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Linq;

namespace NUnitTest
{
    public class BaseTest
    {
        [SetUp]
        public void Setup()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();

            DbContextOptionsBuilder<BusinessLogic.Infrastructure.EFDbContext> optionsBuilder = new DbContextOptionsBuilder<BusinessLogic.Infrastructure.EFDbContext>();

            optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:TestDatabase"],
                                        sqlServerOptions => sqlServerOptions.CommandTimeout(3600)).EnableSensitiveDataLogging();

            this.EFDbContext = new BusinessLogic.Infrastructure.EFDbContext(optionsBuilder.Options);

            this.Config =  new BusinessLogic.Infrastructure.Config(Configuration);
        }

        protected BusinessLogic.Infrastructure.EFDbContext EFDbContext
        {
            get;
            private set;
        }

        protected BusinessLogic.Infrastructure.Config Config
        {
            get;
            private set;
        }


        protected IConfiguration Configuration
        {
            get;
            private set;
        }




        [TearDown]
        public void Close()
        {
            this.EFDbContext.Dispose();
        }
    }
}