using System;   
using Newtonsoft.Json;
using Admin;



namespace AdminShow
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Production_date { get; set; }
        public DateTime Expiration_date { get; set; }
        public string Category { get; set; }
        public double Purchase_price { get; set; }
        public double Selling_price { get; set; }
        public double Gross_profit_per_unit { get; set; } //ë��
        public int Total_purchase_quantity { get; set; }  //�ܹ�����
        public int Total_sales_quantity { get; set; }  //������
        public int Remaining { get; set; }  //���
    }
    public class ShowProduct
    {
        public static void Chose()
        {
            do
            {
                Console.WriteLine("��ѡ������ʽ:");
                Console.WriteLine("0.Id 1.Name 2.�������� 3.�ۼ� 4.ë��");
                Console.WriteLine("5.������ 6.���� 7.��� over.������һ���˵�");
                string input = Console.ReadLine() ?? "";
                if (input.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortId();
                }
                else if (input.Equals("1",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortName();
                }
                else if (input.Equals("2",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortExpiration_date();
                }
                else if (input.Equals("3",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortSelling_price();
                }
                else if (input.Equals("4",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortGross_profit_per_unit();
                }
                else if (input.Equals("5",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortTotal_purchase_quantity();
                }
                else if (input.Equals("6",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortTotal_sales_quantity();
                }
                else if (input.Equals("7",StringComparison.CurrentCultureIgnoreCase))
                {
                    AllSort.SortRemaining();
                }
                else if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Functions.Chose();
                }
                else
                {
                    Console.WriteLine("�Ƿ�����");
                    Console.WriteLine("--------------------------------------");
                }


            
            } while (true);
        }

    }

    public class AllSort
    {
        public static string FilePath = "C:\\Users\\Matho\\Desktop\\sup\\projecttry\\Data\\date.json";
        
        public static void SortId()
        {
            string jsonContent = File.ReadAllText(FilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Selling_price).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }
        }

        public static void SortName()
        {
            string jsonContent = File.ReadAllText(FilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Name).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }
        }

        public static void SortExpiration_date()
        {
            string jsonContent = File.ReadAllText(FilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Expiration_date).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }        
        }

        public static void SortSelling_price()
        {
            string jsonContent = File.ReadAllText(FilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Selling_price).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }

        public static void SortGross_profit_per_unit()
        {
            string jsonContent = File.ReadAllText(FilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Gross_profit_per_unit).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }

        public static void SortTotal_purchase_quantity()
        {
            string jsonContent = File.ReadAllText(FilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Total_purchase_quantity).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }

        public static void SortTotal_sales_quantity()
        {
            string jsonContent = File.ReadAllText(FilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Total_sales_quantity).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }

        public static void SortRemaining()
        {
            string jsonContent = File.ReadAllText(FilePath);
            List<Product> SortList = JsonConvert.DeserializeObject<List<Product>>(jsonContent);
            List<Product> sortedProducts = SortList.OrderByDescending(p => p.Remaining).ToList();
            foreach (var product in sortedProducts)
            {
                Console.WriteLine($"Id:{product.Id.PadRight(6)} Name:{product.Name.PadRight(6)} ��������:{product.Expiration_date} �ۼ�:{product.Selling_price:F2}");
                Console.WriteLine($"ë��:{product.Gross_profit_per_unit:F2}      �ܽ�����:{product.Total_purchase_quantity:F0}      ������:{product.Total_sales_quantity:F0}     ���:{product.Remaining:F0}");
                Console.WriteLine("------------------------------------------------------------------");
            }  
        }
    }
}