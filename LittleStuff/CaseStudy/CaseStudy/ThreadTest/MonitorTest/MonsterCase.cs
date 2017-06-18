using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CaseStudy.ThreadTest.MonitorTest
{
    public class Monster
    {
        public Monster(int blood)
        {
            this.Blood = blood;
            Console.WriteLine(string.Format("我是怪物，我有 {0} 滴血!\r\n", blood));
        }
        public int Blood { get; set; }
    }

    public class Player
    {
        //姓名
        public string Name { get; set; }

        //武器
        public string Weapon { get; set; }

        //攻击力
        public int Power { get; set; }

        //物理攻击  用Enter Exist 实现等同Lock的效果
        public void PhysAttack(Object monster)
        {
            
            Monster m = monster as Monster;
            while (m.Blood > 0)
            {
                Monitor.Enter(monster);
                Console.WriteLine("当前玩家 【{0}】,使用{1}攻击怪物！", this.Name, this.Weapon);
                if (m.Blood >= this.Power)
                {
                    m.Blood -= this.Power;
                }
                else
                {
                    m.Blood = 0;
                }
                Console.WriteLine("怪物剩余血量:{0}\r\n", m.Blood);
                Monitor.Exit(monster);
            }            
        }



        //魔法攻击
        public void MigcAttack(Object monster)
        {
            Monster m = monster as Monster;
            Monitor.Enter(monster);
            Console.WriteLine("当前玩家 {0} 进入战斗\r\n", this.Name);
            while (m.Blood > 0)
            {
               // Monitor.Wait(monster); 有可能导致死锁

                Monitor.Wait(monster, 5000);
                Console.WriteLine("当前玩家 {0} 获得攻击权限", this.Name);
                Console.WriteLine("当前玩家 {0},使用 魔法 攻击怪物！", this.Name, this.Weapon);
                m.Blood = (m.Blood >= this.Power) ? m.Blood - this.Power : 0;
                Console.WriteLine("怪物剩余血量:{0}\r\n", m.Blood);
                Thread.Sleep(500);
                Monitor.Pulse(monster);
            }
            Monitor.Exit(monster);
        }

        //闪电攻击
        public void LightAttack(Object monster)
        {
            Monster m = monster as Monster;
            Monitor.Enter(monster);
            Console.WriteLine("当前玩家 {0} 进入战斗\r\n", this.Name);
            while (m.Blood > 0)
            {
                Monitor.Pulse(monster);
                Console.WriteLine("当前玩家 {0} 获得攻击权限", this.Name);
                Console.WriteLine("当前玩家 {0},使用 闪电 攻击怪物！", this.Name);
                m.Blood = (m.Blood >= this.Power) ? m.Blood - this.Power : 0;
                Console.WriteLine("怪物剩余血量:{0}\r\n", m.Blood);
                Thread.Sleep(500);
                Monitor.Wait(monster);
            }
            Monitor.Exit(monster);
        }
    }



    public class Calculate
    {
        public void Add()
        {
            while (true)
            {
                if (Monitor.TryEnter(this, 1000)) //注意这里，如果1s内获得锁，则进入if,超时后进入else
                //Monitor.Enter(this);
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format("线程{0}获得锁:进入了加法运算", Thread.CurrentThread.Name));
                    Console.WriteLine("开始加法运算 1s 钟");
                    Thread.Sleep(1000);
                    Console.WriteLine(string.Format("线程{0}释放锁:离开了加法运算\r\n", Thread.CurrentThread.Name));
                    Monitor.Exit(this);
                }
                
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\r\n 由于减法运算未完成，未进入加法运算");
                }
            }
        }

        public void Sub()
        {
            while (true)
            {
                Monitor.Enter(this);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(string.Format("线程{0}获得锁:进入了减法运算", Thread.CurrentThread.Name));
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("开始减法运算 2s 钟");
                Thread.Sleep(2000);   //让减法运算长一点，可以演示效果
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(string.Format("线程{0}释放锁:离开了减法运算\r\n", Thread.CurrentThread.Name));
                Monitor.Exit(this);
                Thread.Sleep(2000);
            }
        }
    }

    public class MonitorCase
    {
        public static void RunTestEnterAndExit()
        {
            Monster monster = new Monster(1000);
            Player YouXia = new Player() { Name = "游侠", Weapon = "宝剑", Power = 150 };
            Player YeManRen = new Player() { Name = "野蛮人", Weapon = "链锤", Power = 250 };
            Thread t1 = new Thread(new ParameterizedThreadStart(YouXia.PhysAttack));
            t1.Start(monster);
            Thread t2 = new Thread(new ParameterizedThreadStart(YeManRen.PhysAttack));
            t2.Start(monster);
            t1.Join();
            t2.Join();
            Console.ReadKey();
        }


        public static void RunTestWaitAndPluse()
        {
            Monster monster = new Monster(1500);

            Player Cike = new Player() { Name = "刺客", Power = 250 };
            Player Mofashi = new Player() { Name = "魔法师", Power = 350 };

            Thread t2 = new Thread(new ParameterizedThreadStart(Mofashi.MigcAttack));
            t2.Start(monster);

            Thread t1 = new Thread(new ParameterizedThreadStart(Cike.LightAttack));
            t1.Start(monster);

           

            t1.Join();
            t2.Join();
            Console.ReadKey();
        
        }

        public static void RunTestTryEnter()
        {
            Calculate c = new Calculate();

            Thread t1 = new Thread(new ThreadStart(c.Add));
            t1.Start();

            Thread t2 = new Thread(new ThreadStart(c.Sub));
            t2.Start();

            t1.Join();
            t2.Join();
            Console.ReadKey();
        
        }
    }

}
