namespace MultiThreadingMonitorClassInCSharp
{
    internal class Program
    {
        private static readonly object lockPrintNumbers = new object();

        #region Example -  4c to Understand Wait() and Pulse() Methods of Monitor Class
        //Upto the limit numbers will be printed on the Console
        const int numberLimit = 20;
        static readonly object _lockMonitor = new object();
        #endregion
        static void Main(string[] args)
        {
            #region Example - 1 - using Monitor.Enter()
            //Thread[] threads = new Thread[3];
            //for (int i = 0; i < 3; i++)
            //{
            //    threads[i] = new Thread(PrintNumbers)
            //    {
            //        Name = "Child Thread " + i
            //    };
            //}
            //foreach (Thread t in threads)
            //{
            //    t.Start();
            //}
            #endregion

            #region Example - 2 - using Monitor.Enter(lockObject, ref IsLockTaken)
            //Thread[] threads = new Thread[3];
            //for (int i = 0; i < 3; i++)
            //{
            //    threads[i] = new Thread(PrintNumbers)
            //    {
            //        Name = "Child Thread " + i
            //    };
            //}
            //foreach (Thread t in threads)
            //{
            //    t.Start();
            //}
            #endregion

            #region Example - 3 - using Monitor.TryEnter(Object, TimeSpan,Boolean)
            //Thread[] threads = new Thread[3];
            //for (int i = 0; i < 3; i++)
            //{
            //    threads[i] = new Thread(PrintNumbers)
            //    {
            //        Name = "Child Thread " + i
            //    };
            //}
            //foreach (Thread t in threads)
            //{
            //    t.Start();
            //}
            #endregion

            #region Example - 4a to Understand Wait() and Pulse() Methods of Monitor Class
            Thread evenThread = new Thread(PrintEvenNumbers);
            Thread oddThread = new Thread(PrintOddNumbers);

            //First Start the Even thread.
            evenThread.Start();

            //Puase for 10 ms, to make sure Even thread has started 
            //or else Odd thread may start first resulting different sequence.
            Thread.Sleep(100);

            //Next, Start the Odd thread.
            oddThread.Start();

            //Wait for all the childs threads to complete
            oddThread.Join();
            evenThread.Join();

            Console.WriteLine("\nMain Method Completed.");
            Console.ReadKey();
            #endregion
        }


        #region Example - 1 - using Monitor.Enter()
        //public static void PrintNumbers()
        //{
        //    Console.WriteLine(Thread.CurrentThread.Name + " Trying to enter into the critical section...");
        //    try
        //    {
        //        Monitor.Enter(lockPrintNumbers);
        //        Console.WriteLine(Thread.CurrentThread.Name + " Entered into the critical section.");
        //        for (int i = 0; i < 5; i++)
        //        {
        //            Thread.Sleep(100);
        //            Console.Write(i + ",");
        //        }
        //        Console.WriteLine();
        //    }
        //    finally
        //    {
        //        Monitor.Exit(lockPrintNumbers);
        //        Console.WriteLine(Thread.CurrentThread.Name + " Exit from critical section.");
        //    }
        //}
        #endregion

        #region Example - 2 - using Monitor.Enter(lockObject, ref IsLockTaken)
        //public static void PrintNumbers()
        //{
        //    Console.WriteLine(Thread.CurrentThread.Name + " Trying to enter into the critical section...");
        //    bool IsLockTaken = false;

        //    try
        //    {
        //        Monitor.Enter(lockPrintNumbers, ref IsLockTaken);
        //        if (IsLockTaken)
        //        {
        //            Console.WriteLine(Thread.CurrentThread.Name + " Entered into the critical section.");
        //            for (int i = 0; i < 5; i++)
        //            {
        //                Thread.Sleep(100);
        //                Console.Write(i + ",");
        //            }
        //            Console.WriteLine();
        //        }
        //    }
        //    finally
        //    {
        //        if (IsLockTaken)
        //        {
        //            Monitor.Exit(lockPrintNumbers);
        //            Console.WriteLine(Thread.CurrentThread.Name + " Exit from critical section.");
        //        }
        //    }
        //}
        #endregion

        #region Example - 3 - using Monitor.TryEnter(Object, TimeSpan,Boolean)
        //public static void PrintNumbers()
        //{
        //    TimeSpan timeout = TimeSpan.FromMilliseconds(1000);
        //    bool lockTaken = false;

        //    try
        //    {
        //        Console.WriteLine(Thread.CurrentThread.Name + " Trying to enter into the critical section...");
        //        Monitor.TryEnter(lockPrintNumbers, timeout, ref lockTaken);
        //        if (lockTaken)
        //        {
        //            Console.WriteLine(Thread.CurrentThread.Name + " Entered into the critical section.");
        //            for (int i = 0; i < 5; i++)
        //            {
        //                Thread.Sleep(100);
        //                Console.Write(i + ",");
        //            }
        //            Console.WriteLine();
        //        }
        //        else
        //        {
        //            // The lock was not acquired.
        //            Console.WriteLine(Thread.CurrentThread.Name + " Lock was not acquired");
        //        }
        //    }
        //    finally
        //    {
        //        // To Ensure that the lock is released.
        //        if (lockTaken)
        //        {
        //            Monitor.Exit(lockPrintNumbers);
        //            Console.WriteLine(Thread.CurrentThread.Name + " Exit from critical section.");
        //        }
        //    }
        //}
        #endregion

        #region Example -  4b to Understand Wait() and Pulse() Methods of Monitor Class
        //Printing of Even Numbers Function
        static void PrintEvenNumbers()
        {
            try
            {
                //Implement lock as the Console is shared between two threads
                Monitor.Enter(_lockMonitor);
                for (int i = 0; i <= numberLimit; i = i + 2)
                {
                    //Printing Even Number on Console
                    Console.Write($"{i} ");

                    //Notify Odd thread that I'm done, you do your job
                    //It notifies a thread in the waiting queue of a change in the 
                    //locked object's state.
                    Monitor.Pulse(_lockMonitor);

                    //I will wait here till Odd thread notify me 
                    //Monitor.Wait(monitor);
                    //Without this logic application will wait forever

                    bool isLast = false;
                    if (i == numberLimit)
                    {
                           isLast = true;
                    }
                    if(!isLast)
                    {
                        //I will wait here till Odd thread notify me
                        //Releases the lock on an object and blocks the current thread 
                        //until it reacquires the lock.
                        Monitor.Wait(_lockMonitor);
                    }
                }

            }
            finally
            {
                //Release the lock
                Monitor.Exit(_lockMonitor);
            }
        }

        //Printing of Odd Numbers Function
        static void PrintOddNumbers()
        {
            try
            {
                //Implement lock as the Console is shared between two threads
                Monitor.Enter(_lockMonitor);
                for (int i = 1; i <= numberLimit; i = i + 2)
                {
                    //Printing Odd Number on the Console
                    Console.Write($"{i} ");

                    //Notify Even thread that I'm done, you do your job
                    Monitor.Pulse(_lockMonitor);

                    //I will wait here till Even thread notify me 
                    //Monitor.Wait(monitor);
                    //Without this logic application will wait forever

                    bool isLast = false;
                    if (i == numberLimit-1)
                    {
                        isLast = true;
                    }
                    if (!isLast)
                    {
                        //I will wait here till Even thread notify me
                        //Releases the lock on an object and blocks the current thread 
                        //until it reacquires the lock.
                        Monitor.Wait(_lockMonitor);
                    }
                }

            }
            finally
            {
                //Release the lock
                Monitor.Exit(_lockMonitor);
            }
        }
        #endregion
    }

}