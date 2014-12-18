using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        int timeProcess = 48000;
        double acceptedRequests, declinesRequests, inputRequests, lambda, midServicingTime, mu, q, A, probabilityDeclines;

        CTimerProcess proc = new CTimerProcess(timeProcess);

        acceptedRequests = proc.getAcceptedRequests();
        declinesRequests = proc.getDeclinesRequests();
        inputRequests = proc.getInputRequests();

        lambda = (inputRequests + declinesRequests) / timeProcess;
        midServicingTime = timeProcess / acceptedRequests;
        mu = 1 / midServicingTime;
        q = lambda / (mu + lambda);	//пропускная способность относит
        A = lambda * q;	//пропускная способность абс
        probabilityDeclines = declinesRequests / (declinesRequests + acceptedRequests);
        Console.WriteLine("Vhodyashie zayavki: {0:D}\n", (int)inputRequests);
        Console.WriteLine("Obrabotanniye zayavki: {0:D}\n", (int)acceptedRequests);
        Console.WriteLine("Neobrabotanniye zayavki: {0:D}\n", (int)declinesRequests);
        Console.WriteLine("Propnaya sposobnost' otnos: {0:F}\n", q);
        Console.WriteLine("Propnaya sposobnost' abs: {0:F}\n", A);
        Console.WriteLine("Veroyatnost' otkaza: {0:F}\n", probabilityDeclines);
    }
}