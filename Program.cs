using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        string inputText;
        int timeProcess = 48000;
        double acceptedRequests, declinesRequests, inputRequests, lambda, midServicingTime, mu, q, A, probabilityDeclines;
        bool isGaussian = false;
        Console.WriteLine("Enter '0' if the distribution of time is linear and '1' if gaussian");
        inputText = Console.ReadLine();

        if (inputText.CompareTo("1") == 0)
            isGaussian = true;


        CTimerProcess proc = new CTimerProcess(timeProcess, isGaussian);

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