using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicGroceriesShopConsole
{
    public class Program
    {
        static void Main()
        {
            Product apple = new Product() { Name = "apple", Price = 50m };
            Product banana = new Product() { Name = "banana", Price = 40m };
            Product tomato = new Product() { Name = "tomato", Price = 30m };
            Product potato = new Product() { Name = "potato", Price = 26m };

            List<Product> products = new List<Product>()
            {
                apple,  banana,
                tomato, potato
            };

            StringBuilder sb = new StringBuilder();

            foreach (var product in products)
            {
                string name = product.Name;

                sb.Append(name + ", ");
            }

            Console.WriteLine("Select product from: " + sb.ToString().TrimEnd());
            Console.WriteLine("To finish the order select: bill");

            string input = Console.ReadLine().ToLower();

            List<Product> listedProducts = new List<Product>();

            while (input != "bill")
            {
                switch (input)
                {
                    case "apple":
                        listedProducts.Add(apple);
                        break;
                    case "banana":
                        listedProducts.Add(banana);
                        break;
                    case "tomato":
                        listedProducts.Add(tomato);
                        break;
                    case "potato":
                        listedProducts.Add(potato);
                        break;
                    default:
                        break;
                }

                input = Console.ReadLine().ToLower();
            }

            Console.WriteLine(Bill(listedProducts));
        }

        private static string Bill(List<Product> listedProducts)
        {
            decimal bill = 0m;
            int productsCount = listedProducts.Count;

            foreach (var product in listedProducts)
            {
                bill += product.Price;
            }

            if (productsCount >= 3)
            {
                List<Product> products = new List<Product>();

                for (int i = 0; i < 3; i++)
                {
                    products.Add(listedProducts[i]);
                }

                listedProducts.RemoveRange(0, 3);

                decimal priceToTakeOff = products.Select(p => p.Price).Min();

                bill = bill - priceToTakeOff;

                productsCount -= 3;
            }

            for (int i = 0; i < listedProducts.Count - 1; i++)
            {
                for (int k = i + 1; k < listedProducts.Count - i; k++)
                {
                    if (listedProducts[i] == listedProducts[k])
                    {
                        bill = bill - listedProducts[i].Price * 0.5m;
                        i++;
                    }
                }
            }

            string result = string.Empty;

            if (bill >= 100)
            {
                int aws = (int)bill / 100;

                int c = (int)bill % 100;

                result = $"{aws} aws and {c} clouds";
            }
            else
            {
                int c = (int)bill % 100;
                result = $"{c} clouds";
            }

            return result;
        }
    }
}