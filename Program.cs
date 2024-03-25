using System;
using Judge;
using Admin;
using Worker;


namespace mainProcess
{
    public static class Program
    {
        static void Main()
        {
            Chose();
        }

        public static void Chose()
        {
            Console.WriteLine("������������������������������������������������������������������������������������������������������������");
            Console.WriteLine("��        ���������:                                 ��");
            Console.WriteLine("��        ��������Կ:                                 ��");
            Console.WriteLine("��        over��������:                               ��");
            Console.WriteLine("������������������������������������������������������������������������������������������������������������");
            string name;
            string Key;
            do
            {
                name = Console.ReadLine() ?? "";
                Key = Console.ReadLine() ?? "";
                if (name.Equals("over",StringComparison.CurrentCultureIgnoreCase) || Key.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("�������");
                    Environment.Exit(0);
                    //return;  //��������
                }
                if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "wrong")
                {
                    Console.WriteLine("��֤����");
                    continue;
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "admin")
                {
                    Console.WriteLine("\\\\��ӭ��������Ա////");
                    Functions.Chose();
                    //��������̨
                    break;
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "worker")
                {
                    Console.WriteLine("\\\\��ӭ��������Ա////");
                    WorkerFunctions.FunctionChose();
                    //������������
                    break; 
                }
                else
                {
                    Console.WriteLine("��Կ����");
                    continue;
                }
            } while (true);
            
        }
    }
}