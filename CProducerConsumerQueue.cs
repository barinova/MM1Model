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
    CGaussian gaussTime = new CGaussian();
    bool isGaussian = false;
    bool isQueue = false;
    bool isRequestProc = false;
    bool queueBusy = false;

    public bool isQueueBusy
    {
        get { return queueBusy; }
        set
        {
            queueBusy = value;
        }
    }

    public bool isTypeQueue
    {
        get
        {
            return isQueue;
        }
    }
    public CProducerConsumerQueue(bool typeTime, bool typeQueue)
    {
        isGaussian = typeTime;
        isQueue = typeQueue;
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
                isRequestProc = true;
                Thread.Sleep(getTime());
                acceptedRequests += 1;
                isRequestProc = false;
                //Console.WriteLine(acceptedRequests + " accepted");

            }
            else
                wh.WaitOne();
        }
    }

    private int getTime()
    {
        int time;

        if (!isGaussian)
        {
            time = rand.Next(12, 20);
        }
        else
        {
            time = gaussTime.Next(12, 20);
            //Console.WriteLine(time);
        }

        return time;
    }

    public int getAcceptedRequests() { return acceptedRequests; }
    public bool isRequestProcessed() { return isRequestProc; }

}