using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SingeltonExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            //Singelton
            int singelton1Result = fooSingelton1();
            int singelton2Result = fooSingelton2();

            Console.WriteLine(singelton1Result);
            Console.WriteLine(singelton2Result);
        }

        public static int fooSingelton1()
        {
            ReaderWriterLock rwl = new ReaderWriterLock();
            Task[] taskArray = new Task[100];
            rwl.AcquireWriterLock(Timeout.Infinite);
            for (int i = 0; i < 100; i++)
            {
                taskArray[i] = Task.Factory.StartNew((prwl) =>
                {
                    ((ReaderWriterLock)prwl).AcquireReaderLock(Timeout.Infinite);
                    Singelton1 s = Singelton1.Instance;
                    ((ReaderWriterLock)prwl).ReleaseReaderLock();
                }, rwl);
            }
            rwl.ReleaseWriterLock();
            Task.WaitAll(taskArray);
            return Singelton1.Instance.Count;
        }
        public static int fooSingelton2()
        {
            ReaderWriterLock rwl = new ReaderWriterLock();
            Task[] taskArray = new Task[100];
            rwl.AcquireWriterLock(Timeout.Infinite);
            for (int i = 0; i < 100; i++)
            {
                taskArray[i] = Task.Factory.StartNew((prwl) =>
                {
                    ((ReaderWriterLock)prwl).AcquireReaderLock(Timeout.Infinite);
                    Singelton2 s = Singelton2.Instance;
                    ((ReaderWriterLock)prwl).ReleaseReaderLock();
                }, rwl);
            }
            rwl.ReleaseWriterLock();
            Task.WaitAll(taskArray);
            return Singelton2.Instance.Count;
        }
    }

    public sealed class Singelton1
    {
        private static int count = 0;
        private static Singelton1 instance = null;
        private Singelton1()
        {
            count++;
        }
        public static Singelton1 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singelton1();
                }
                return instance;
            }
        }
        public int Count
        {
            get
            {
                return count;
            }
        }
    }
    public sealed class Singelton2
    {
        private static int count = 0;
        private Singelton2()
        {
            count++;
        }
        public static Singelton2 Instance
        {
            get
            {
                return Nested.instance;
            }
        }
        public int Count
        {
            get
            {
                return count;
            }
        }
        private class Nested
        {
            internal static readonly Singelton2 instance = new Singelton2();
            static Nested() { }
        }
    }
}
