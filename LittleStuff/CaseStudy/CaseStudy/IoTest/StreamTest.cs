using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy.IoTest
{
    public class StreamTest
    {

        public static void IoTest()
        { 
          //  FileStreamTest();
            //FileWriterTest();
            ConsoleTest();
        }


      

        public static void FileStreamTest()
        { 
             string curpath = Directory.GetCurrentDirectory();
            
             FileStream fs = new FileStream("k:/IoTest/filestream.txt",FileMode.Create);        //需要目录存在
            byte[] bytes = Encoding.ASCII.GetBytes("hello world!");
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            Console.WriteLine(curpath);
            Console.ReadKey();
        }


        public static void FileWriterTest()
        {
            StreamWriter sw = new StreamWriter("k:/IoTest/FileWriter.txt");    //目录不存在也可以
            
            byte[] bytes = Encoding.ASCII.GetBytes("hello world! FileWriter \n ok ok  \n i am ok");
            sw.WriteLine(Encoding.ASCII.GetString(bytes));            
            sw.Close();

            StreamReader sr = new StreamReader("k:/IoTest/FileWriter.txt", Encoding.UTF8);
            Console.WriteLine(sr.ReadLine());
            Console.ReadKey();

            
        }


        public static void ConsoleTest()
        { 
            string str = "";
            while (true)
            {
                str = Console.ReadLine();
                Console.WriteLine(str);

                if (str == "abc")
                    break;
            }


            str = Console.ReadLine();
            Console.WriteLine(str);
            Console.ReadLine();
        }
    }
}
