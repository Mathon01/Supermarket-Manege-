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
        public double Gross_profit_per_unit { get; set; } //����
        public int Total_purchase_quantity { get; set; }  //������
        public int Total_sales_quantity { get; set; }  //������
        public int Remaining { get; set; }  //ʣ������
    }
    
    public static class WorkerFunctions
    {
        public static void FunctionChose()
        {
            Console.WriteLine("��ѡ����Ҫִ�еĹ���");
            string chose ="";
            do
            {   
                Console.WriteLine("0.������һ�� 1.���� 2.���� break.�°�����");
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
                    Console.Write("����Ƿ� ��������");
                }
            } while (true);

        }
        public static string jsonFilePath = "C:\\Users\\Matho\\Desktop\\sup\\projecttry\\Data\\date.json" ;
        //��Ʒ����
        public static Dictionary<string, int> bill= new Dictionary<string, int>() ;
        
        public static string tmp_jsonDate = File.ReadAllText(jsonFilePath ,new UTF8Encoding(false));
        public static List<Product> have = JsonConvert.DeserializeObject<List<Product>>(tmp_jsonDate);
        public static void AddBillMain()
        {

            do
            {
                Console.WriteLine("�������ƷID over.����  price.����");
                string input = Console.ReadLine() ?? "";
                
                if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                    FunctionChose();
                
                if (input.Equals("price",StringComparison.CurrentCultureIgnoreCase))
                    TotalPrice(bill, have);
                
                int productId;
                if (!int.TryParse(input,out productId))
                {
                    Console.Write("ID���Ϸ�:");
                    continue;
                }
                //��Ʒ���ڼ��
                if (InOrOut(input))
                {
                    if (bill.ContainsKey(input)) 
                    {
                        bill[input]++;  
                        Console.WriteLine("�˵������");
                    }
                    else
                    {
                        bill.Add(input,1); 
                        Console.WriteLine("�˵������");
                    }
                }
                else
                    Console.WriteLine("��ǰ��ƷIdδ¼��,����ϵ������Ա//�˼���Ʒδ�����˵�");
                
                
            } while (true);

        }

        //Vip�ͻ������ļ�·��
        public static string CostomerFilePath = "C:\\Users\\Matho\\Desktop\\sup\\projecttry\\Data\\costomer.json";
        public static void TotalPrice(Dictionary<string, int> bill, List<Product> have)
        {
            double Discount = 1;  //�ۿ�
            string isVip = null;    
            do
            {
                Console.WriteLine("������VIP_ID: ,����������y,û��������n:");
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
                    Console.WriteLine("��������򲻴��� ��������:");
                }

            } while (true); 
            
            double sum = 0;
            Console.WriteLine("�����˵�����:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("ID      ����      ����      ����");

            String changeNumOfProduct = File.ReadAllText(jsonFilePath ,new UTF8Encoding(false));
            List<Product> change = JsonConvert.DeserializeObject<List<Product>>(changeNumOfProduct);
            foreach (var item in bill)
            {
                string productId = item.Key; // ��ѯ ID
                int quantity = item.Value;   // ��ȡ����

                
                Product product = change.Find(p => p.Id.Equals(productId, StringComparison.CurrentCultureIgnoreCase));

                if (product != null)
                {
                    product.Total_sales_quantity +=quantity;
                    product.Remaining -= quantity;
                    double totalPriceForProduct = product.Selling_price * quantity;
                    sum += totalPriceForProduct;
                    Console.Write($"{productId,-8}"); // ID �п��Ϊ 6�������
                    
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

                    Console.Write($"{product.Selling_price.ToString("C"),-10}"); // ���۸�ʽ��Ϊ���ң��п��Ϊ 8�������
                    Console.WriteLine($"{quantity,3}"); // �����п��Ϊ 6�������
                }
                
            }
            string updatedJson = JsonConvert.SerializeObject(change, Formatting.Indented);
            File.WriteAllText(jsonFilePath, updatedJson, new UTF8Encoding(false));
            Console.WriteLine("--------------------------------");
            // ����
            if (Discount!=1)
            {
                Console.WriteLine("����VIP�˿�:");
                Console.WriteLine(string.Format("�����ۿ�:{0,23:P}",Discount));
            }
            Console.WriteLine("Ӧ����{0,26:F2}", sum);
            Console.WriteLine("ʵ����{0,26:F2}",sum*Discount);
            FunctionChose();
        }
        
        //�����Ʒ
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
                Console.WriteLine("�������Ʒ��Ϣ:");
                Console.Write("Id(��λ����):"); //���п���ż�� Ҳ���Բ��� �Ͼ�ʵ����������������ͬ��ŵĲ�Ʒ������
                while (true)
                {
                    addproduct.Id = Console.ReadLine() ?? "";
                    if (int.TryParse(addproduct.Id,out int tmp) && addproduct.Id.Length == 4)
                        break;
                    else
                        Console.Write("���벻�Ϸ� ��������Id:");
                    
                }
                Console.Write("�������Ʒ����:");
                addproduct.Name = Console.ReadLine() ?? "";

                while (true)
                {
                    Console.Write("�������Ʒ��������(yyyy-mm-dd):");
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
                            Console.WriteLine("��������ڱ������ڻ���ڵ�ǰϵͳ����,����������!");
                            continue;
                        }
                    }
                    else
                        Console.WriteLine("��������ڸ�ʽ����ȷ,����������!");
                }
                    
                while (true)
                {
                    Console.Write("���������ʱ�� (yyyy-MM-dd):");
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
                            Console.WriteLine("��������ڱ���������������,����������!");
                            continue;
                        }
                    }
                    else
                        Console.WriteLine("��������ڸ�ʽ����ȷ,����������!");
                }
                    
                Console.Write("�������Ʒ����: ");
                addproduct.Category = Console.ReadLine() ?? "";

                while (true)
                {
                    Console.Write("�������Ʒ����: ");
                    if (double.TryParse(Console.ReadLine(), out price))
                    {
                        addproduct.Purchase_price = price;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("����Ƿ�!��������!");
                        continue;
                    }
                }
                    
                while (true)
                {
                    Console.Write("�������Ʒ�ۼ�: ");
                    if (double.TryParse(Console.ReadLine(), out price))
                    {
                        addproduct.Selling_price = price;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("����Ƿ�!��������!");
                        continue;
                    }
                }
                
                while (true)
                {
                    Console.Write("�������Ʒ����:");
                    input = Console.ReadLine() ?? "";
                    if (int.TryParse(input,out num))
                    {
                        addproduct.Total_purchase_quantity += num;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("����Ƿ�!��������!");
                        continue;
                    }
                }
                //��������
                addproduct.Gross_profit_per_unit = Math.Round(addproduct.Selling_price - addproduct.Purchase_price,2);
                add.Add(addproduct);

                Console.Write("�Ƿ����¼��(y/n): ");
                input = Console.ReadLine() ?? "";



            } while (input.Equals("y",StringComparison.CurrentCultureIgnoreCase));
            //д���ļ�
            string jsonIn = JsonConvert.SerializeObject(add, Formatting.Indented);

            File.WriteAllText(jsonFilePath, jsonIn , new UTF8Encoding(false));

            Console.WriteLine("��Ʒ��ӳɹ�!");
        }

        //��ʱ�˵����Ƿ����        
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

        //ȷ����ݺ��ۿ�
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
        
        //�쿨
        public static void CreatVipCostomer()
        {
            string input = null;
            //���л��ļ�����
            string CostomerFile = File.ReadAllText(CostomerFilePath ,new UTF8Encoding(false));
            List<Property> OriginalCostomer = JsonConvert.DeserializeObject<List<Property>>(CostomerFile);     
            //�������
            //List<Property> add = new List<Property>();
            Property additem = new Property();
            do
            {
                Console.WriteLine("������8λ���ڵ����ִ���VIP�˻�,�˳�������break");
                Console.Write("�����봴���˻�ID:");

                input = Console.ReadLine() ?? "";
                if (input.Equals("break",StringComparison.CurrentCultureIgnoreCase))
                    return ;

                if (OriginalCostomer.Any(c => c.Md5 == Encrypt.Md5(input)))
                {
                    Console.WriteLine("��ǰID�ѱ�ʹ�� �����!");
                    Console.WriteLine("��������������������������������������������������������������������������������������������������������");
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
                    Console.WriteLine("��������˻�ID���Ϸ� ����������8λ���ڵ�����!");
                    Console.WriteLine("��������������������������������������������������������������������������������������������������������");
                    continue;
                }
            } while (true);

            string updatedJson = JsonConvert.SerializeObject(OriginalCostomer, Formatting.Indented);
            File.WriteAllText(CostomerFilePath, updatedJson , new UTF8Encoding(false));
            Console.WriteLine("�˻���ӳɹ�");
            
        }
    }

    public class Encrypt //����
    {
        public static string Md5(string original)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // ʹ��Сдʮ�����Ʊ�ʾ
                }
                return sb.ToString();
            }
        }
    }
}