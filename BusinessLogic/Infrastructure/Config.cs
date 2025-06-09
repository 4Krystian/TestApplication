using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Infrastructure
{
	public class Config
	{
		public string DBConnectionString { get; private set; }

        public string WebAPIURL { get; private set; }

        public Config()
		{

		}

		public Config(IConfiguration Configuration)
		{

            this.DBConnectionString = Configuration["ConnectionStrings:TestDatabase"];

            this.WebAPIURL = Configuration["AppSettings:WebAPIURL"];
        }
	}
}
