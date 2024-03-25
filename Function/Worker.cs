using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;


namespace Worker
{
    public class Property
    {
        public string Md5 {get; set;}
        public int Score {get; set;}
    }
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Production_date { get; set; }
        public DateTime Expiration_date { get; set; }
        public string Category { get; set; }
        public double Purchase_price { get; set; }
        public double Selling_price { get; set; }
        public double Gross_profit_per_unit { get; set; } //利润
        public int Total_purchase_quantity { get; set; }  //进货量
        public int Total_sales_quantity { get; set; }  //总销量
        public int Remaining { get; set; }  //剩余数量
    }
    
    public static class WorkerFunctions
    {
        public static void FunctionChose()
        {
            Console.WriteLine("请选择您要执行的功能");
            string chose ="";
            do
            {   
                Console.WriteLine("0.返回上一级 1.结账 2.进货 break.下班走人");
                chose = Console.ReadLine() ?? "";
                if (chose.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                {
                    mainProcess.Program.Chose();
                }
                else if (chose.Equals("1",StringComparison.CurrentCultureIgnoreCase))
                {
                    AddBillMain();
                }
                else if (chose.Equals("2",StringComparison.CurrentCultureIgnoreCase))
                {
                    AddProduct();
                }
                else if (chose.Equals("break",StringComparison.CurrentCultureIgnoreCase))
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.Write("输入非法 重新输入");
                }
            } while (true);

        }
        public static string jsonFilePath = "C:\\Users\\Matho\\Desktop\\sup\\projecttry\\Data\\date.json" ;
        //商品数据
        public static Dictionary<string, int> bill= new Dictionary<string, int>() ;
        
        public static string tmp_jsonDate = File.ReadAllText(jsonFilePath ,new UTF8Encoding(false));
        public static List<Product> have = JsonConvert.DeserializeObject<List<Product>>(tmp_jsonDate);
        public static void AddBillMain()
        {

            do
            {
                Console.WriteLine("请输入产品ID over.结束  price.结账");
                string input = Console.ReadLine() ?? "";
                
                if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                    FunctionChose();
                
                if (input.Equals("price",StringComparison.CurrentCultureIgnoreCase))
                    TotalPrice(bill, have);
                
                int productId;
                if (!int.TryParse(input,out productId))
                {
                    Console.Write("ID不合法:");
                    continue;
                }
                //商品存在检测
                if (InOrOut(input))
                {
                    if (bill.ContainsKey(input)) 
                    {
                        bill[input]++;  
                        Console.WriteLine("账单已添加");
                    }
                    else
                    {
                        bill.Add(input,1); 
                        Console.WriteLine("账单已添加");
                    }
                }
                else
                    Console.WriteLine("当前商品Id未录入,请联系工作人员//此件商品未计入账单");
                
                
            } while (true);

        }

        //Vip客户数据文件路径
        public static string CostomerFilePath = "C:\\Users\\Matho\\Desktop\\sup\\projecttry\\Data\\costomer.json";
        public static void TotalPrice(Dictionary<string, int> bill, List<Product> have)
        {
            double Discount = 1;  //折扣
            string isVip = null;    
            do
            {
                Console.WriteLine("请输入VIP_ID: ,办理请输入y,没有请输入n:");
                isVip = Console.ReadLine() ?? "";
                if (isVip.Equals("y",StringComparison.CurrentCultureIgnoreCase))
                {
                    CreatVipCostomer();
                    continue;
                }
                else if (isVip.Equals("n",StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
                else if (IsVip(isVip ,out Discount))
                {
                    break;
                }
                else 
                {
                    Console.WriteLine("输入有误或不存在 重新输入:");
                }

            } while (true); 
            
            double sum = 0;
            Console.WriteLine("您的账单如下:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("ID      名称      单价      数量");

            String changeNumOfProduct = File.ReadAllText(jsonFilePath ,new UTF8Encoding(false));
            List<Product> change = JsonConvert.DeserializeObject<List<Product>>(changeNumOfProduct);
            foreach (var item in bill)
            {
                string productId = item.Key; // 查询 ID
                int quantity = item.Value;   // 获取数量

                
                Product product = change.Find(p => p.Id.Equals(productId, StringComparison.CurrentCultureIgnoreCase));

                if (product != null)
                {
                    product.Total_sales_quantity +=quantity;
                    product.Remaining -= quantity;
                    double totalPriceForProduct = product.Selling_price * quantity;
                    sum += totalPriceForProduct;
                    Console.Write($"{productId,-8}"); // ID 列宽度为 6，左对齐
                    
                    if (product.Name.Length==2)
                        Console.Write($"{product.Name,-8}"); 
                    else if (product.Name.Length==3)
                        Console.Write($"{product.Name,-7}"); 
                    else if (product.Name.Length==4)
                        Console.Write($"{product.Name,-6}"); 
                    else if (product.Name.Length==5)
                        Console.Write($"{product.Name,-5}");
                    else if (product.Name.Length==6)
                        Console.Write($"{product.Name,-4}");

                    Console.Write($"{product.Selling_price.ToString("C"),-10}"); // 单价格式化为货币，列宽度为 8，左对齐
                    Console.WriteLine($"{quantity,3}"); // 数量列宽度为 6，左对齐
                }
                
            }
            string updatedJson = JsonConvert.SerializeObject(change, Formatting.Indented);
            File.WriteAllText(jsonFilePath, updatedJson, new UTF8Encoding(false));
            Console.WriteLine("--------------------------------");
            // 结算
            if (Discount!=1)
            {
                Console.WriteLine("您好VIP顾客:");
                Console.WriteLine(string.Format("您的折扣:{0,23:P}",Discount));
            }
            Console.WriteLine("应付：{0,26:F2}", sum);
            Console.WriteLine("实付：{0,26:F2}",sum*Discount);
            FunctionChose();
        }
        
        //添加商品
        public static void AddProduct()
        {
            string input="y";
            DateTime outtime;
            double price=0;
            int num=0;
            List<Product> add = new List<Product>();

            if (File.Exists(jsonFilePath))
            {
                string jsonExisting = File.ReadAllText(jsonFilePath ,new UTF8Encoding(false));
                add = JsonConvert.DeserializeObject<List<Product>>(jsonExisting);
            }
            do
            {
                Product addproduct = new Product();
                Console.WriteLine("请输入产品信息:");
                Console.Write("Id(四位长度):"); //已有库存编号检测 也可以不加 毕竟实际生活里有两个相同编号的产品不可能
                while (true)
                {
                    addproduct.Id = Console.ReadLine() ?? "";
                    if (int.TryParse(addproduct.Id,out int tmp) && addproduct.Id.Length == 4)
                        break;
                    else
                        Console.Write("输入不合法 重新输入Id:");
                    
                }
                Console.Write("请输入产品名称:");
                addproduct.Name = Console.ReadLine() ?? "";

                while (true)
                {
                    Console.Write("请输入产品生产日期(yyyy-mm-dd):");
                    input = Console.ReadLine() ?? "";
                    if (DateTime.TryParse(input,out outtime))
                    {
                        if (DateTime.Compare(outtime,DateTime.Now)<=0)
                        {
                            addproduct.Production_date = outtime;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("输入的日期必须早于或等于当前系统日期,请重新输入!");
                            continue;
                        }
                    }
                    else
                        Console.WriteLine("输入的日期格式不正确,请重新输入!");
                }
                    
                while (true)
                {
                    Console.Write("请输入过期时间 (yyyy-MM-dd):");
                    input = Console.ReadLine() ?? "";
                    if (DateTime.TryParse(input,out outtime))
                    {
                        if (DateTime.Compare(outtime,addproduct.Production_date) > 0)
                        {
                            addproduct.Production_date = outtime;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("输入的日期必须晚于生产日期,请重新输入!");
                            continue;
                        }
                    }
                    else
                        Console.WriteLine("输入的日期格式不正确,请重新输入!");
                }
                    
                Console.Write("请输入产品种类: ");
                addproduct.Category = Console.ReadLine() ?? "";

                while (true)
                {
                    Console.Write("请输入产品进价: ");
                    if (double.TryParse(Console.ReadLine(), out price))
                    {
                        addproduct.Purchase_price = price;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("输入非法!重新输入!");
                        continue;
                    }
                }
                    
                while (true)
                {
                    Console.Write("请输入产品售价: ");
                    if (double.TryParse(Console.ReadLine(), out price))
                    {
                        addproduct.Selling_price = price;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("输入非法!重新输入!");
                        continue;
                    }
                }
                
                while (true)
                {
                    Console.Write("请输入产品数量:");
                    input = Console.ReadLine() ?? "";
                    if (int.TryParse(input,out num))
                    {
                        addproduct.Total_purchase_quantity += num;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("输入非法!重新输入!");
                        continue;
                    }
                }
                //计算利润
                addproduct.Gross_profit_per_unit = Math.Round(addproduct.Selling_price - addproduct.Purchase_price,2);
                add.Add(addproduct);

                Console.Write("是否继续录入(y/n): ");
                input = Console.ReadLine() ?? "";



            } while (input.Equals("y",StringComparison.CurrentCultureIgnoreCase));
            //写入文件
            string jsonIn = JsonConvert.SerializeObject(add, Formatting.Indented);

            File.WriteAllText(jsonFilePath, jsonIn , new UTF8Encoding(false));

            Console.WriteLine("商品添加成功!");
        }

        //临时账单中是否存在        
        static bool InOrOut(string real)
        {
            foreach (var item in have)
            {
                if (item.Id.Equals(real,StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        //确定身份和折扣
        public static bool IsVip(string check , out double Discount)
        {
            string CostomerFileContent = File.ReadAllText(CostomerFilePath ,new UTF8Encoding(false));
            List<Property> checkList = JsonConvert.DeserializeObject<List<Property>>(CostomerFileContent);
            Discount = 1;
            foreach (var item in checkList)
            {
                if (item.Md5.Equals(Encrypt.Md5(check),StringComparison.CurrentCultureIgnoreCase))
                {
                    if (item.Score<=500)
                    {
                        Discount = 0.95;
                        return true;
                    }
                    else if (item.Score <= 800)
                    {
                        Discount = 0.9;
                        return true;
                    }
                    else if (item.Score<=1000)
                    {
                        Discount = 0.87;
                        return true;
                    }
                    else
                    {
                        Discount = 0.8;
                        return true;
                    }
                }
            }
             
            return false;
        }
        
        //办卡
        public static void CreatVipCostomer()
        {
            string input = null;
            //序列化文件数据
            string CostomerFile = File.ReadAllText(CostomerFilePath ,new UTF8Encoding(false));
            List<Property> OriginalCostomer = JsonConvert.DeserializeObject<List<Property>>(CostomerFile);     
            //添加数据
            //List<Property> add = new List<Property>();
            Property additem = new Property();
            do
            {
                Console.WriteLine("请输入8位以内的数字创建VIP账户,退出请输入break");
                Console.Write("请输入创建账户ID:");

                input = Console.ReadLine() ?? "";
                if (input.Equals("break",StringComparison.CurrentCultureIgnoreCase))
                    return ;

                if (OriginalCostomer.Any(c => c.Md5 == Encrypt.Md5(input)))
                {
                    Console.WriteLine("当前ID已被使用 请更换!");
                    Console.WriteLine("────────────────────────────────────────────────────");
                    continue;
                }

                if(int.TryParse(input, out int accountId) && input.Length<9)
                {
                    additem.Md5 = Encrypt.Md5(input);
                    additem.Score = 0;
                    OriginalCostomer.Add(additem);
                    break;
                }
                else
                {
                    Console.WriteLine("您输入的账户ID不合法 请重新输入8位以内的数字!");
                    Console.WriteLine("────────────────────────────────────────────────────");
                    continue;
                }
            } while (true);

            string updatedJson = JsonConvert.SerializeObject(OriginalCostomer, Formatting.Indented);
            File.WriteAllText(CostomerFilePath, updatedJson , new UTF8Encoding(false));
            Console.WriteLine("账户添加成功");
            
        }
    }

    public class Encrypt //加密
    {
        public static string Md5(string original)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // 使用小写十六进制表示
                }
                return sb.ToString();
            }
        }
    }
}