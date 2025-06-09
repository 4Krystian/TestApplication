using BusinessLogic.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class OrderProcessor
    {
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

        public OrderProcessor(BusinessLogic.Infrastructure.EFDbContext EFDbContext, BusinessLogic.Infrastructure.Config Config)
        {
            this.EFDbContext = EFDbContext;
            this.Config = Config;
        }

        public IEnumerable<BusinessLogic.Models.OrderWebAPI> GetOrdersBySQL()
        {

            List<BusinessLogic.Models.OrderWebAPI> orders = new List<BusinessLogic.Models.OrderWebAPI>();

            using (SqlConnection sqlConnection = new SqlConnection(this.Config.DBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Clear();

                    cmd.CommandText = @"Select Orders.OrderId as OrderId,
                                        Shops.Name as OrderShopName,
                                        Shops.ShopId as OrderShopId,
                                        Customers.Town as OrderTown,
                                        [OrderItems].TotalNetValue as TotalNetValue
                                        from Orders
                                        inner join Shops on Shops.ShopId = Orders.ShopId
                                        inner join Customers on Customers.CustomerId = Orders.CustomerId
                                        
                                        cross apply (
                                                     Select 
                                                     IsNull(Sum(OrderItems.Quantity * Items.PriceNet),0) As TotalNetValue
                                                     From OrderItems 
                                                        inner join Items on Items.ItemId = OrderItems.ItemId
                                                     Where OrderItems.OrderId = Orders.OrderId
                                                    ) [OrderItems]

                                        where
                                              Customers.Town like '%w%'
                                              and Orders.OrderId % 2 = 0";

                    sqlConnection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        orders.Add(new Models.OrderWebAPI() { 
                                OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                ShopId = reader.GetInt32(reader.GetOrdinal("OrderShopId")),
                                Shop = reader.GetString(reader.GetOrdinal("OrderShopName")),
                                Town = reader.GetString(reader.GetOrdinal("OrderTown")),
                                TotalNetValue  = reader.GetDecimal(reader.GetOrdinal("TotalNetValue"))
                        }                        
                        );
                    }
                }
            }

            return orders;
        }

        public IEnumerable<BusinessLogic.Models.OrderEF> GetOrdersByEF()
        {

            List<BusinessLogic.Models.OrderEF> orderModels = new List<BusinessLogic.Models.OrderEF>();

            List<BusinessLogic.Entities.Order> orders = this.EFDbContext.Orders
                                                          .Include(o => o.PaymentMethod)
                                                          .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
                                                          .Where(o => o.OrderItems.Sum(oi => oi.Quantity * oi.Item.PriceNet) > 200)
                                                          .ToList();

            var ordersByPayment = orders.GroupBy(o => o.PaymentMethod);


            foreach (var payment in ordersByPayment)
            {
                orderModels.Add(new Models.OrderEF()
                {
                    OrderQuantity = payment.Count(),
                    PaymentMethod = payment.Key.Name,
                    TotalGrossValue = payment.SelectMany(p => p.OrderItems).Sum(oi => oi.Quantity * oi.Item.PriceNet)
                });
            }


            return orderModels.OrderBy(o => o.PaymentMethod);
        }
    }
}
