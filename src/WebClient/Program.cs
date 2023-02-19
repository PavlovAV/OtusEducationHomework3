using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            bool exitCode = false;
            while (!exitCode)
            {
                Console.WriteLine("");
                Console.WriteLine("Ввведите 1 для получения пользователя по id, 2 для создания пользователя, 0 - exit");
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '1':
                        Console.WriteLine();
                        Console.WriteLine("Введите ID клиента (или пусто для вывода всех клиентов): ");
                        var id = Console.ReadLine();
                        var result = await client.GetStringAsync("https://localhost:5001/customers/"+id);
                        Console.WriteLine(result);
                        break;
                    case '2':
                        Console.WriteLine();
                        var rnd = new Random();
                        var rCustomer = RandomCustomer();
                        var newCustomer = new Customer() { Id = rnd.Next(1,10), Firstname = rCustomer.Firstname, Lastname = rCustomer.Lastname };
                        var response = await client.PostAsync("https://localhost:5001/customers/", JsonContent.Create(newCustomer));
                        var result1 = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            result1 = await client.GetStringAsync("https://localhost:5001/customers/" + result1);
                        }
                        Console.WriteLine(result1);

                        break;
                    case '0':
                        exitCode = true;
                        break;
                }


            }

        }

        private static CustomerCreateRequest RandomCustomer()
        {
            var rnd = new Random();
            return new CustomerCreateRequest("FirstName"+rnd.Next(1,100), "LastName" + rnd.Next(1,100));
        }
    }
}