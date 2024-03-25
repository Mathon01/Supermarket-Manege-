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
            Console.WriteLine("┌────────────────────────────────────────────────────┐");
            Console.WriteLine("│        请输入身份:                                 │");
            Console.WriteLine("│        请输入密钥:                                 │");
            Console.WriteLine("│        over结束程序:                               │");
            Console.WriteLine("└────────────────────────────────────────────────────┘");
            string name;
            string Key;
            do
            {
                name = Console.ReadLine() ?? "";
                Key = Console.ReadLine() ?? "";
                if (name.Equals("over",StringComparison.CurrentCultureIgnoreCase) || Key.Equals("over",StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("程序结束");
                    Environment.Exit(0);
                    //return;  //结束程序
                }
                if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "wrong")
                {
                    Console.WriteLine("认证错误");
                    continue;
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "admin")
                {
                    Console.WriteLine("\\\\欢迎回来管理员////");
                    Functions.Chose();
                    //进入管理后台
                    break;
                }
                else if (IdentityJudge.IsWorkerOrAdmin(name,Key) == "worker")
                {
                    Console.WriteLine("\\\\欢迎回来收银员////");
                    WorkerFunctions.FunctionChose();
                    //进入收银工作
                    break; 
                }
                else
                {
                    Console.WriteLine("密钥错误");
                    continue;
                }
            } while (true);
            
        }
    }
}