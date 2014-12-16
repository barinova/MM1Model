using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class Program
{
    static Timer timer;
    static System.Threading.TimerCallback requestTimerDelegate = new System.Threading.TimerCallback(timerTick);
    static int inputRequests = 0;
    static bool request = false;
    static Random rand = new Random();
    static private void timerTick(object obj)
    {
        request = true;
        inputRequests += 1;
        timer.Change(rand.Next(12, 34), 0);
    }
    
    static void Main()
    {
        int timeProcess = 48000;
        double acceptedRequests, declinesRequests;
        CProducerConsumerQueue queue;
        using (queue = new CProducerConsumerQueue())
        {
            DateTime maxTime = DateTime.Now.AddMilliseconds(timeProcess);
            timer = new Timer(timerTick, null, 1, rand.Next(12, 34));

            while (maxTime > DateTime.Now)
            {
                if (request)
                {
                    request = false;
                    queue.EnqueueTask("Input requests: " + inputRequests.ToString() + "\n");
                }
            }
        }

        acceptedRequests = queue.getAcceptedRequests();
        declinesRequests = inputRequests - acceptedRequests;
        double lambda = (inputRequests + declinesRequests) / timeProcess;
        double midServicingTime = timeProcess / acceptedRequests;
        double Mu = 1 / midServicingTime;
        double q = lambda / (Mu + lambda);	//пропускная способность относит
        double A = lambda * q;	//пропускная способность абс
        double P_otk = declinesRequests / (declinesRequests + acceptedRequests);

        Console.WriteLine("Obrabotanniye zayavki: {0:D}\n", (int)acceptedRequests);
        Console.WriteLine("Neobrabotanniye zayavki: {0:D}\n", (int)declinesRequests);
        Console.WriteLine("Propnaya sposobnost' otnos: {0:F}\n", q);
        Console.WriteLine("Propnaya sposobnost' abs: {0:F}\n", A);
        Console.WriteLine("Veroyatnost' otkaza: {0:F}\n", P_otk);
    }
}