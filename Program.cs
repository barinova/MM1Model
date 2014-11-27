using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MM1Model
{

    class StateObjClass
    {
        // Used to hold parameters for calls to TimerTask.
        public int SomeValue;
        public System.Threading.Timer TimerReference;
        public bool TimerCanceled;
    }

    class Program
    {
        
        static System.Threading.TimerCallback requestTimerDelegate = new System.Threading.TimerCallback(request);
        static System.Threading.TimerCallback serviceTimerDelegate = new System.Threading.TimerCallback(service);
        static StateObjClass requestStateObj = new StateObjClass();
        static StateObjClass serviceStateObj = new StateObjClass();
        static bool serviceDelayed = false;

        static double acceptedRequests = 0;
        static double inputRequests = 0;

        static double timeProcess = 48; // sec

        static void request(object obj)
        {
            inputRequests += 1;
            if (!serviceDelayed)
            {
                serviceDelayed = true;
                acceptedRequests += 1;
                System.Threading.Timer serviceTimerItem = new System.Threading.Timer(serviceTimerDelegate, requestStateObj, randomTime(12, 20), 0);
                serviceStateObj.TimerReference = serviceTimerItem;
            }

            StateObjClass State = (StateObjClass)requestStateObj;
            // Use the interlocked class to increment the counter variable
            System.Threading.Interlocked.Increment(ref State.SomeValue);
            System.Diagnostics.Debug.WriteLine("Request:: Launched new thread  " + DateTime.Now.ToString());

            if (State.TimerCanceled)
            {
                State.TimerReference.Dispose();
                System.Diagnostics.Debug.WriteLine("Request:: Done  " + DateTime.Now.ToString());
            }
        }

        static void service(object obj)
        {
            StateObjClass State = (StateObjClass)serviceStateObj;
            // Use the interlocked class to increment the counter variable
            System.Threading.Interlocked.Increment(ref State.SomeValue);
            System.Diagnostics.Debug.WriteLine("Service:: Launched new thread  " + DateTime.Now.ToString());

            State.TimerReference.Dispose();
            System.Diagnostics.Debug.WriteLine("Service:: Done  " + DateTime.Now.ToString());

            serviceDelayed = false;
        }

        static int randomTime(int minTime, int maxTime)
        {
            Random time = new Random();
            return time.Next(minTime, maxTime)*100;
        }

        static void Main(string[] args)
        {
            requestStateObj.TimerCanceled = false;
            requestStateObj.TimerCanceled = false;
            requestStateObj.SomeValue = DateTime.Now.Minute;
            
            System.Threading.Timer requestTimerItem = new System.Threading.Timer(requestTimerDelegate, requestStateObj, 0, randomTime(12, 34));
            
            requestStateObj.TimerReference = requestTimerItem;

            while ((requestStateObj.SomeValue - DateTime.Now.Minute)/10 < 0.8) //or serviceStateObject
            {
                System.Threading.Thread.Sleep(1000);  
            }

            requestStateObj.TimerCanceled = true;
            serviceStateObj.TimerCanceled = true;

            double declinesRequests = inputRequests - acceptedRequests;

            double Lambda = (inputRequests + declinesRequests) / timeProcess; //48000 ms ~ 0.8 min

            if (acceptedRequests > 0)
            {
                double midServicingTime = timeProcess/acceptedRequests;
                Console.WriteLine("Requests:");
                Console.WriteLine(inputRequests.ToString());
                Console.WriteLine("Proccessed:");
                Console.WriteLine(acceptedRequests.ToString());
                Console.WriteLine("Declined:");
                Console.WriteLine(declinesRequests.ToString());

                double Mu = 1 / midServicingTime;
                double q = Lambda / (Mu + Lambda);	//пропускная способность относит
                double A = Lambda * q;	//пропускная способность абс
                double P_otk = declinesRequests / (declinesRequests + acceptedRequests);

                Console.WriteLine("q:");
                Console.WriteLine(q);
                Console.WriteLine("A:");
                Console.WriteLine(A);
                Console.WriteLine("Potk:");
                Console.WriteLine(P_otk);
                Console.WriteLine("L'ambda:");
                Console.WriteLine(Lambda);
                Console.WriteLine("M'u:");
                Console.WriteLine(Mu);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
