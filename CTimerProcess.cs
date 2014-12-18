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
    static bool isGaussian = false;
    static CGaussian gaussTime = new CGaussian();
    System.Threading.TimerCallback requestTimerDelegate = new System.Threading.TimerCallback(timerTick);

    static private void timerTick(object obj)
    {
        isRequestEntered = true;
        inputRequests += 1;
        //Console.WriteLine(inputRequests + " input requests");

        if (!queue.isRequestProcessed())
        {
            startProcess();
        }

        timer.Change(getTime(), 0);
        isRequestEntered = false;
    }

    static private int getTime()
    {
        int time;

        if (!isGaussian)
        {
            time = rand.Next(12, 34);
        }
        else
        {
            time = gaussTime.Next(12, 34);
        }

        return time;
    }


    static private void startProcess()
    {
        queue.EnqueueTask("Input requests: " + inputRequests.ToString() + "\n");
    }

    public CTimerProcess(int timeProcess, bool isGaussian)
    {
        using (queue = new CProducerConsumerQueue(isGaussian))
        {
            DateTime maxTime = DateTime.Now.AddMilliseconds(timeProcess);


            timer = new Timer(timerTick, null, getTime(), 0);

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
