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
    static bool isQueueBusy = false;
    static CGaussian gaussTime = new CGaussian();
    static double countQueuePaticipiant = 0;
    static int time, timeWaiting = 0;
    System.Threading.TimerCallback requestTimerDelegate = new System.Threading.TimerCallback(timerTick);



    public double getAcceptedRequests
    {
        get
        {
            return acceptedRequests;
        }
    }

    public double getDeclinesRequests 
    {
        get
        {
            return declinesRequests;
        }
    }

    public double getInputRequests
    { 
        get
        {
            return inputRequests;
        }
    }

    public int getTimeWaiting 
    { 
        get
        {
        return timeWaiting; 
        }
    }

    public double getCountQueuePaticipiant
    {
        get
        {
            return countQueuePaticipiant;
        }
    }

    static private void timerTick(object obj)
    {
        isRequestEntered = true;
        inputRequests += 1;
        //Console.WriteLine(inputRequests + " input requests");

        if (!queue.isRequestProcessed())
        {
            startProcess();
        }
        else
        {
            if (queue.isTypeQueue && !queue.isQueueBusy)
            {
                time = Environment.TickCount;
                queue.isQueueBusy = true;
                countQueuePaticipiant += 1;
            }
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

        if (queue.isQueueBusy)
        {
            timeWaiting += Environment.TickCount - time;
            queue.isQueueBusy = false;
            queue.EnqueueTask("Input requests: " + inputRequests.ToString() + "\n");
        }
    }

    public CTimerProcess(int timeProcess, bool isGaussian, bool isQueue)
    {
        using (queue = new CProducerConsumerQueue(isGaussian, isQueue))
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
}
