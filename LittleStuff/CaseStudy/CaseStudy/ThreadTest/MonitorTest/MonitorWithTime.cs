using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaseStudy.ThreadTest.MonitorTest
{
    class MonitorWithTime
    {
        static object lockObj = new object();

        static void A()
        {
            Console.WriteLine("A enter...");
            lock (lockObj) //进入就绪队列 
            {
                Console.WriteLine("A get lock...");
                Thread.Sleep(5000);
                Console.WriteLine("A release lock...");
                Monitor.Pulse(lockObj);
                Monitor.Wait(lockObj); //自我流放到等待队列 
                Console.WriteLine("A exit...");
            }
        }

        static void B()
        {
            Console.WriteLine("B enter...");
            Thread.Sleep(500);
            lock (lockObj) //进入就绪队列 
            {
                Console.WriteLine("B lock...");
                Monitor.Wait(lockObj); //自我流放到等待队列 
                Console.WriteLine("B return again...");
               Monitor.Pulse(lockObj);
            }
            Console.WriteLine("B exit...");
        }

        static void C()
        {
            Console.WriteLine("C enter...");
            Thread.Sleep(800);
            lock (lockObj) //进入就绪队列 
            {

                Console.WriteLine("C lock...");
                Console.WriteLine("C release lock...");
                Monitor.Pulse(lockObj);
                Monitor.Pulse(lockObj);
              //  Monitor.Wait(lockObj);
                //Console.WriteLine("C wait for call ...");
            }

            Console.WriteLine("C exit...");
        }

        public static void RunTest()
        {
            new Thread(A).Start();
            new Thread(B).Start();
            new Thread(C).Start();
            Console.ReadLine();
        }

    }

}
