using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class CProducerConsumerQueue : IDisposable
{
    EventWaitHandle wh = new AutoResetEvent(false);
    Thread worker;
    object locker = new object();
    Queue<string> tasks = new Queue<string>();
    Random rand = new Random();
    int acceptedRequests = 0;

    public CProducerConsumerQueue()
    {
        worker = new Thread(Work);
        worker.Start();
    }

    public void EnqueueTask(string task)
    {
        lock (locker)
            tasks.Enqueue(task);

        wh.Set();
    }

    public void Dispose()
    {
        EnqueueTask(null);
        worker.Join();
        wh.Close();
    }

    void Work()
    {
        while (true)
        {
            string task = null;

            lock (locker)
            {
                if (tasks.Count > 0)
                {
                    task = tasks.Dequeue();
                    if (task == null)
                        return;
                }
            }

            if (task != null)
            {
                Thread.Sleep(rand.Next(12, 20));
                
                acceptedRequests += 1;
                Console.WriteLine(task + "\nAccepted requests: " + acceptedRequests + "\n");
            }
            else
                wh.WaitOne();
        }
    }

    public int getAcceptedRequests() { return acceptedRequests; }

}