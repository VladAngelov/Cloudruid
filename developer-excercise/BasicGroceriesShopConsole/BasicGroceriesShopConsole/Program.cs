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
            Console.WriteLine($"{Constants.GREETING}");
            Console.WriteLine($"{Constants.ADMIN_INFO}");
            Console.WriteLine($"{Constants.SHOPPING}");
            Console.Write($"{Constants.CONTROL_TYPE}");

            List<Product> allProducts = new List<Product>();

            string userControl = Console.ReadLine().ToLower();

            bool run = true;

            while (run)
            {
                switch (userControl)
                {
                    case "admin":
                        allProducts = AddingProducts(allProducts); 
                        Console.Write($"{Constants.CONTROL_TYPE}");
                        userControl = Console.ReadLine().ToLower();
                        break;
                    case "shopping":
                        Console.WriteLine(Shopping(allProducts));
                        run = false;
                        break;
                    default:
                        Console.Write($"{Constants.ERROR}");
                        run = false;
                        break;
                }
            }
        }

        private static List<Product> AddingProducts(List<Product> allProducts)
        {
            Console.WriteLine($"{Constants.ADD_PRODUCT_INFO}");

            string[] productToAdd = Console.ReadLine()
                .Split(", ", StringSplitOptions.RemoveEmptyEntries);

            while (productToAdd[0] != "done")
            {
                if (!String.IsNullOrEmpty(productToAdd[1]) &&
                    int.TryParse(productToAdd[0], out int price))
                {
                    Product product = new Product()
                    {
                        Name = productToAdd[1],
                        Price = price
                    };

                    if (!allProducts.Contains(product))
                    {
                        allProducts.Add(product);

                        Console.WriteLine($"{Constants.ADD_PRODUCT_SUCCESS}");
                    }
                    else
                    {
                        Console.WriteLine($"{Constants.ALREADY_ADDED_PRODUCT}");
                    }
                }
                else
                {
                    Console.WriteLine($"{Constants.ERROR}");
                }

                productToAdd = Console.ReadLine()
                .Split(", ", StringSplitOptions.RemoveEmptyEntries);
            }

            return allProducts;
        }

        private static string Shopping(List<Product> allProducts)
        {
            StringBuilder sb = new StringBuilder();

            List<string> productNames = new List<string>();

            foreach (var product in allProducts)
            {
                string name = product.Name;
                productNames.Add(name);
            }

            string products = String.Join(',', allProducts.Select(x => x.Name));

            Console.WriteLine($"{Constants.SELECT_PRODUCT} {products}");
            Console.WriteLine($"{Constants.FINISHING_ORDER}");

            string input = Console.ReadLine().ToLower();

            List<Product> listedProducts = new List<Product>();

            while (true)
            {
                if (input == "bill")
                {
                    return Bill(listedProducts);
                }
                else
                {
                    if (productNames.Contains(input))
                    {
                        Product product = allProducts
                            .Where(x => x.Name == input)
                            .FirstOrDefault();

                        listedProducts.Add(product);
                    }
                    else
                    {
                        Console.WriteLine($"{Constants.MISSING} {input}");
                    }

                    input = Console.ReadLine().ToLower();
                }
            }
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
                for (int k = i + 1; k <= listedProducts.Count - 1; k++)
                {
                    if (listedProducts[i] == listedProducts[k])
                    {
                        bill = bill - listedProducts[i].Price * 0.5m;
                        listedProducts.Remove(listedProducts[k]);
                        break;
                    }
                }
            }

            string result = string.Empty;

            if (bill >= 100)
            {
                int aws = (int)bill / 100;

                int clouds = (int)bill % 100;

                result = $"{aws} {Constants.BIG_VALUE} {clouds} {Constants.SMALL_VALUE}";
            }
            else
            {
                int c = (int)bill % 100;

                result = $"{c} {Constants.SMALL_VALUE}";
            }

            return result;
        }
    }
}