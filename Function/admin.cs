using System;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using AdminShow;
using mainProcess;
using Newtonsoft.Json;

namespace Admin
{
    class IdentityData
    {
        public string Identity { get; set; }
        public string Name { get; set; }
        public string key { get; set; }
    }
    static class Functions
    {
        
        static string IdentityFilePath = "C:\\Users\\Matho\\Desktop\\sup\\projecttry\\Data\\Identity.json" ;
        
        public static void Chose()
        {
            string judge ;
            do
            {
                Console.WriteLine("��ѡ�����:");
                Console.WriteLine("0.������ 1.���ɾ�� 2.��Ʒ���ݲ鿴 over.������һ��");
                judge = Console.ReadLine() ?? "";
                if (judge.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                {
                    AddIdentity();
                }
                else if (judge.Equals("1",StringComparison.CurrentCultureIgnoreCase))
                {
                    DelIdentity();
                }
                else if (judge.Equals("2",StringComparison.CurrentCultureIgnoreCase))
                {
                    ShowProduct.Chose();
                }
                else if (judge.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Program.Chose();
                }
                else
                {
                    Console.WriteLine("������Ч:");
                }
            } while (true);
        }
        
        
        //����û�
        static void AddIdentity()  
        {
            char type;
            while (true)
            {
                Console.WriteLine("��ѡ������������: a.����Ա  w.����Ա over.��������");
                string input = Console.ReadLine() ?? "";
                if (input.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Chose();
                }
                if (input.Length != 1)
                {
                    Console.WriteLine("��Чѡ�� ��������:");
                    continue;
                }
                type = (char)input[0];
                if (type == 'a')
                {
                    Console.WriteLine("��ѡ������� [����Ա] ");
                    AddInfo(type);
                }
                else if (type == 'w')
                {
                    Console.Write("��ѡ������� [����Ա] ");
                    AddInfo(type);
                }
                else if (type == 'b')
                {
                    break;
                }
                else
                {
                    Console.WriteLine("��Чѡ�� ��������:");
                }
            }
        }

        //����û���Ϣ
        static void AddInfo(char IDtype)
        {
            Console.WriteLine("���������ֺ��������Կո����:");
            string input = Console.ReadLine() ?? "";
            string[] parts = input.Split(' ');
            if (parts.Length != 2)
            {
                Console.WriteLine("��ʽ����,��������:");
                return; 
            }
            IdentityData addtmp = new IdentityData();
        
            addtmp.Identity = (IDtype=='a') ? "admin" : "worker" ;    
            addtmp.Name = parts[0];
            addtmp.key = parts[1];    
            WriteIntoFile(addtmp);        
        }

        //��ӵ��ļ�
        static void WriteIntoFile(IdentityData data)
        {
            List<IdentityData> identityList = JsonConvert.DeserializeObject<List<IdentityData>>(File.ReadAllText(IdentityFilePath,new UTF8Encoding(false)));
            // ���µ������Ϣ��ӵ����е������Ϣ�б���
            identityList.Add(data);
            // ���л�
            string json = JsonConvert.SerializeObject(identityList, Formatting.Indented); 
            // �����л�
            File.WriteAllText(IdentityFilePath, json ,new UTF8Encoding(false));
            Console.WriteLine("\\\\��ӳɹ�////");
            Chose();
        }

        //ɾ���û�
        static void DelIdentity()
        {
            // string jsonContent = File.ReadAllText(IdentityFilePath);  //���л�
            List<IdentityData> identityList = JsonConvert.DeserializeObject<List<IdentityData>>(File.ReadAllText(IdentityFilePath,new UTF8Encoding(false)));  //�����л�

            
            
            do
            {
                foreach (var item in identityList) //�г���������
                {
                    Console.WriteLine($"Identiy:{item.Identity.PadRight(7)} Name:{item.Name.PadRight(10)} Key:{item.key.PadRight(6)}");
                }
                Console.Write("������ɾ���û������ƺ�����:");
                string delInput = Console.ReadLine();
                string[] delData = delInput.Split(' ');
                if (delData.Length != 2)
                {
                    Console.WriteLine("���ݴ���,����������!");
                    continue;
                }
                var delItem = identityList.FirstOrDefault(x => x.Name == delData[0] && x.key == delData[1]);
                if (delItem!=null)
                {
                    identityList.Remove(delItem);
                    File.WriteAllText(IdentityFilePath, JsonConvert.SerializeObject(identityList,Formatting.Indented) ,new UTF8Encoding(false));
                    Console.WriteLine($"�ɹ��Ƴ��û�{delData[0]}");
                }
                    
                else
                    Console.WriteLine($"{delData[0]}�û������ڣ�");
                Console.WriteLine("��ѡ��������Ĳ���: 0.����ɾ���û� �����:������һ����");
                delInput = Console.ReadLine();
                if (delInput.Equals("0",StringComparison.CurrentCultureIgnoreCase))
                    continue;
                else
                    Chose();
                
            } while (true);
            
        }

    }

}