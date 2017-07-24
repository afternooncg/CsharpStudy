using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CaseStudy.MemTest
{
    class MemTest
    {

        public static void MarshalStructureToPtrTest()
        {
         

            PERSON person;
            person.identicardid = MarshalTestProgram.CodeBytes("123456198001011111", define.MAX_LENGTH_OF_IDENTICARDID);
            person.name = MarshalTestProgram.CodeBytes("jackson", define.MAX_LENGTH_OF_NAME);
            person.country =  MarshalTestProgram.CodeBytes("China", define.MAX_LENGTH_OF_COUNTRY);
            person.nation = MarshalTestProgram.CodeBytes("HanZu", define.MAX_LENGTH_OF_NATION);
            person.birthday = MarshalTestProgram.CodeBytes("19800101", define.MAX_LENGTH_OF_BIRTHDAY);
            person.address = MarshalTestProgram.CodeBytes("Luoshan Road, Shanghai", define.MAX_LENGTH_OF_ADDRESS);

            int nSizeOfPerson = Marshal.SizeOf(person);
            IntPtr intPtr = Marshal.AllocHGlobal(nSizeOfPerson);

            Console.WriteLine("The person infomation is as follows:");
            MarshalTestProgram.ShowPerson(person);

            try
            {
                //将数据从托管对象封送到非托管内存块,该内存块开始地址为intPtr
                Marshal.StructureToPtr(person, intPtr, true);

                //将数据从非托管内存块封送到新分配的指定类型的托管对象anotherPerson
                PERSON anotherPerson = (PERSON)Marshal.PtrToStructure(intPtr, typeof(PERSON));

                Console.WriteLine("The person after copied is as follows:");
                MarshalTestProgram.ShowPerson(anotherPerson);
            }
            catch (ArgumentException)
            {
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);    //free tha memory
            }

            Console.ReadLine();
        }
    }

    public static class define  //define some constant
    {
        public const int MAX_LENGTH_OF_IDENTICARDID = 20;   //maximum length of identicardid
        public const int MAX_LENGTH_OF_NAME = 50;           //maximum length of name
        public const int MAX_LENGTH_OF_COUNTRY = 50;        //maximum length of country
        public const int MAX_LENGTH_OF_NATION = 50;         //maximum length of nation
        public const int MAX_LENGTH_OF_BIRTHDAY = 8;        //maximum length of birthday
        public const int MAX_LENGTH_OF_ADDRESS = 200;       //maximum length of address
    }

    public struct PERSON    //person structure
    {

        
        //MarshalAs:指示如何在托管代码和非托管代码之间封送数据
        //UnmanagedType:指定如何将参数或字段封送到非托管内存块
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = define.MAX_LENGTH_OF_IDENTICARDID)]
        public byte[] identicardid;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = define.MAX_LENGTH_OF_NAME)]
        public byte[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = define.MAX_LENGTH_OF_COUNTRY)]
        public byte[] country;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = define.MAX_LENGTH_OF_NATION)]
        public byte[] nation;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = define.MAX_LENGTH_OF_BIRTHDAY)]
        public byte[] birthday;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = define.MAX_LENGTH_OF_ADDRESS)]
        public byte[] address;
    }

    class MarshalTestProgram
    {
        private static byte _fillChar = 0;      //the fill character

        //convert string to byte array in Ascii with length is len        
        public static byte[] CodeBytes(string str, int len)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = string.Empty;
            }

            byte[] result = new byte[len];
            byte[] strBytes = Encoding.Default.GetBytes(str);

            //copy the array converted into result, and fill the remaining bytes with 0
            for (int i = 0; i < len; i++)
                result[i] = ((i < strBytes.Length) ? strBytes[i] : _fillChar);

            return result;
        }

        //show the person information
        public static void ShowPerson(PERSON person)
        {
            Console.WriteLine("cardid   :" + Encoding.ASCII.GetString(person.identicardid));
            Console.WriteLine("name     :" + Encoding.ASCII.GetString(person.name));
            Console.WriteLine("country  :" + Encoding.ASCII.GetString(person.country));
            Console.WriteLine("nation   :" + Encoding.ASCII.GetString(person.nation));
            Console.WriteLine("birthday :" + Encoding.ASCII.GetString(person.birthday));
            Console.WriteLine("address  :" + Encoding.ASCII.GetString(person.address));
        }

       
    }

}
