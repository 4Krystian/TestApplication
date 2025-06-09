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

            List<BusinessLogic.Entities.Shop> shops = this.DbContext.Shops.ToList();

            if (!shops.Any())
            {
                this.DbContext.Shops.AddRange(Enumerable.Range(1, 10).Select(index => new Shop { Name = $"Sklep {index}" }));

                this.DbContext.SaveChanges();
            }

            List<BusinessLogic.Entities.Customer> customers = this.DbContext.Customers.ToList();

            if (!customers.Any())
            {
                string[] towns = new string[] { "Warszawa", "Wrocław", "Poznań", "Kraków", "Szczecin" };

                string[] postcodes = new string[] { "00001", "50003", "60001", "30000", "70000" };

                string[] address = new string[] { "Polna", "Leśna", "Słoneczna", "Pomarańczowa", "Ogrodowa", "Brzozowa", "Szkolna" };


                this.DbContext.Customers.AddRange(Enumerable.Range(1, towns.Length).Select(index => new Customer 
                { 
                    Town= towns[index-1],
                    PostalCode= postcodes[index-1],
                    Adress = address[random.Next(address.Length-1)]
                }                
                ));

                this.DbContext.SaveChanges();
            }

            List<BusinessLogic.Entities.Item> items = this.DbContext.Items.ToList();

            if(!items.Any())
            {
                this.DbContext.Items.AddRange(Enumerable.Range(1, 50).Select(index => new Item 
                { 
                    EAN= BusinessLogic.Tools.GetRandomString(13),
                    PriceNet= random.Next(50,350)
                }                
                ));

                this.DbContext.SaveChanges();

                items = this.DbContext.Items.ToList();

                decimal defaultVATRate = 1.22M;

                foreach(BusinessLogic.Entities.Item item in items)
                {
                    item.PriceGross = item.PriceNet * defaultVATRate;                        
                }

                this.DbContext.SaveChanges();
            }

            List<BusinessLogic.Entities.PaymentMethod> paymentMethods = this.DbContext.PaymentMethods.ToList();

            if(!paymentMethods.Any())
            {
                this.DbContext.PaymentMethods.Add(new PaymentMethod() { Name = "gotówka" });
                this.DbContext.PaymentMethods.Add(new PaymentMethod() { Name = "karta płatnicza" });
                this.DbContext.PaymentMethods.Add(new PaymentMethod() { Name = "przelew" });

                this.DbContext.SaveChanges();
            }


            List<BusinessLogic.Entities.Order> orders = this.DbContext.Orders.ToList();


            if (!orders.Any())
            {
                shops = this.DbContext.Shops.ToList();

                customers = this.DbContext.Customers.ToList();

                items = this.DbContext.Items.ToList();

                paymentMethods = this.DbContext.PaymentMethods.ToList();

                foreach (var index in Enumerable.Range(1, 250))
                {
                    BusinessLogic.Entities.Order order =  new BusinessLogic.Entities.Order()
                    {
                        Shop = shops[random.Next(shops.Count() - 1)],
                        Customer = customers[random.Next(customers.Count() - 1)],
                        PaymentMethod = paymentMethods[random.Next(paymentMethods.Count() - 1)]
                    };

                    this.DbContext.Orders.Add(order);

                    this.DbContext.OrderItems.AddRange(Enumerable.Range(1, random.Next(1,5)).Select(index => new OrderItem
                    {
                        Order = order,
                        Item = items[random.Next(items.Count() - 1)],
                        Quantity = random.Next(1, 3)
                    }
                    ));

                    this.DbContext.SaveChanges();
                }


            }

            Assert.Pass();
        }
    }
}