using QueryingRavenDB3._5.Models;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryingRavenDB3._5
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please, enter a company id (0 to exit): ");

                int companyId;

                if (!int.TryParse(Console.ReadLine(), out companyId))
                {
                    Console.WriteLine("Order # is invalid. ");
                    continue;
                }

                if (companyId == 0) break;

                QueryCompanyOrders(companyId);
            }

            Console.WriteLine("Goodbye!");
        }

        private static void QueryCompanyOrders(int companyId)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var orders = (
                    from order in session.Query<Order>()
                                            .Include(o => o.Company)
                    where order.Company == $"companies/{companyId}"
                    select order
                    ).ToList();

                var company = session.Load<Company>(companyId);

                if (company == null)
                {
                    Console.WriteLine("Company not found");

                    return;
                }

                Console.WriteLine($"Orders for {company.Name}");

                foreach (var order in orders)
                {
                    Console.WriteLine($"{order.Id} - {order.OrderedAt}");
                }
            }
        }
    }
}
