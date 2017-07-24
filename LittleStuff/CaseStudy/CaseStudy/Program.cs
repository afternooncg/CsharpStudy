using CaseStudy.IoTest;
using CaseStudy.ThreadTest.MonitorTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            //Threads
            // CaseStudy.ThreadTest.MonitorTest.MonsterCase.RunTestEnterAndExit();
            //CaseStudy.ThreadTest.MonitorTest.MonitorCase.RunTestWaitAndPluse();
           // CaseStudy.ThreadTest.MonitorTest.MonitorCase.RunTestTryEnter();

           // MonitorWithTime.RunTest();

           // StreamTest.IoTest();

            MemTest.MemTest.MarshalStructureToPtrTest();
            
        }
    }
}
