using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class CTimerProcess
{
    static Timer timer;
    static int inputRequests = 0;
    static Random rand = new Random();
    static bool isRequestEntered = false;
    static CProducerConsumerQueue queue;
    double acceptedRequests, declinesRequests;

    static private void timerTick(object obj)
    {
        isRequestEntered = true;
        inputRequests += 1;
        //Console.WriteLine(inputRequests + " input requests");
        if (!queue.isRequestProcessed())
        {
            startProcess();
        }
        timer.Change(rand.Next(12, 34), 0);
        isRequestEntered = false;
    }

    System.Threading.TimerCallback requestTimerDelegate = new System.Threading.TimerCallback(timerTick);

    static private void startProcess()
    {
        queue.EnqueueTask("Input requests: " + inputRequests.ToString() + "\n");
    }

    public CTimerProcess(int timeProcess)
    {
        using (queue = new CProducerConsumerQueue())
        {
            DateTime maxTime = DateTime.Now.AddMilliseconds(timeProcess);
            timer = new Timer(timerTick, null, rand.Next(12, 34), 0);

            while (maxTime > DateTime.Now)
            {
                    
            }

            acceptedRequests = queue.getAcceptedRequests();
            declinesRequests = inputRequests - acceptedRequests;
                
        }
    }

    public double getAcceptedRequests() {return acceptedRequests; }
    public double getDeclinesRequests() { return declinesRequests; }
    public double getInputRequests() { return inputRequests; }

}
