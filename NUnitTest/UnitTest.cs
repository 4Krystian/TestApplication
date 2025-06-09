using BusinessLogic.Entities;
using System.Net;

namespace NUnitTest
{
    public class Tests: BaseTest
    {
        [Test]
        public void InitializeDataBase()
        {
            Random random = new Random();

            List<BusinessLogic.Entities.Shop> shops = this.EFDbContext.Shops.ToList();

            if (!shops.Any())
            {
                this.EFDbContext.Shops.AddRange(Enumerable.Range(1, 10).Select(index => new Shop { Name = $"Sklep {index}" }));

                this.EFDbContext.SaveChanges();
            }

            List<BusinessLogic.Entities.Customer> customers = this.EFDbContext.Customers.ToList();

            if (!customers.Any())
            {
                string[] towns = new string[] { "Warszawa", "Wrocław", "Poznań", "Kraków", "Szczecin" };

                string[] postcodes = new string[] { "00001", "50003", "60001", "30000", "70000" };

                string[] address = new string[] { "Polna", "Leśna", "Słoneczna", "Pomarańczowa", "Ogrodowa", "Brzozowa", "Szkolna" };


                this.EFDbContext.Customers.AddRange(Enumerable.Range(1, towns.Length).Select(index => new Customer 
                { 
                    Town= towns[index-1],
                    PostalCode= postcodes[index-1],
                    Adress = address[random.Next(address.Length-1)]
                }                
                ));

                this.EFDbContext.SaveChanges();
            }

            List<BusinessLogic.Entities.Item> items = this.EFDbContext.Items.ToList();

            if(!items.Any())
            {
                this.EFDbContext.Items.AddRange(Enumerable.Range(1, 50).Select(index => new Item 
                { 
                    EAN= BusinessLogic.Tools.GetRandomString(13),
                    PriceNet= random.Next(50,350)
                }                
                ));

                this.EFDbContext.SaveChanges();

                items = this.EFDbContext.Items.ToList();

                decimal defaultVATRate = 1.22M;

                foreach(BusinessLogic.Entities.Item item in items)
                {
                    item.PriceGross = item.PriceNet * defaultVATRate;                        
                }

                this.EFDbContext.SaveChanges();
            }

            List<BusinessLogic.Entities.PaymentMethod> paymentMethods = this.EFDbContext.PaymentMethods.ToList();

            if(!paymentMethods.Any())
            {
                this.EFDbContext.PaymentMethods.Add(new PaymentMethod() { Name = "gotówka" });
                this.EFDbContext.PaymentMethods.Add(new PaymentMethod() { Name = "karta płatnicza" });
                this.EFDbContext.PaymentMethods.Add(new PaymentMethod() { Name = "przelew" });

                this.EFDbContext.SaveChanges();
            }


            List<BusinessLogic.Entities.Order> orders = this.EFDbContext.Orders.ToList();


            if (!orders.Any())
            {
                shops = this.EFDbContext.Shops.ToList();

                customers = this.EFDbContext.Customers.ToList();

                items = this.EFDbContext.Items.ToList();

                paymentMethods = this.EFDbContext.PaymentMethods.ToList();

                foreach (var index in Enumerable.Range(1, 250))
                {
                    BusinessLogic.Entities.Order order =  new BusinessLogic.Entities.Order()
                    {
                        Shop = shops[random.Next(shops.Count() - 1)],
                        Customer = customers[random.Next(customers.Count() - 1)],
                        PaymentMethod = paymentMethods[random.Next(paymentMethods.Count() - 1)]
                    };

                    this.EFDbContext.Orders.Add(order);

                    this.EFDbContext.OrderItems.AddRange(Enumerable.Range(1, random.Next(1,5)).Select(index => new OrderItem
                    {
                        Order = order,
                        Item = items[random.Next(items.Count() - 1)],
                        Quantity = random.Next(1, 3)
                    }
                    ));

                    this.EFDbContext.SaveChanges();
                }


            }

            Assert.Pass();
        }

        [Test]
        public void GetOrdersBySQL()
        {
            BusinessLogic.OrderProcessor orderProcessor = new BusinessLogic.OrderProcessor(EFDbContext, Config);

            var orders = orderProcessor.GetOrdersBySQL();   

            Assert.IsNotNull(orders);   
        }

        [Test]
        public void GetOrdersByEF()
        {
            BusinessLogic.OrderProcessor orderProcessor = new BusinessLogic.OrderProcessor(EFDbContext, Config);

            var orders = orderProcessor.GetOrdersByEF();

            Assert.IsNotNull(orders);
        }
    }
}