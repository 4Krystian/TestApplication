using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Infrastructure
{
	public static class DependencyInjectionRegistry
	{
		public static IServiceCollection AddLibrary(this IServiceCollection services, IConfiguration Configuration)
		{
            string connectionString = Configuration["ConnectionStrings:TestDatabase"];

            services.AddSingleton(Configuration);
            services.AddSingleton(new Config(Configuration));
            services.AddDbContext<EFDbContext>(options => options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(3600).UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            return services;
		}
	}
}