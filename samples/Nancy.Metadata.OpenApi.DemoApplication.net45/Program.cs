using Nancy.Hosting.Self;
using System;

namespace Nancy.Metadata.OpenApi.DemoApplication.net45
{
    public class Program
    {
        private static void Main()
        {
            string url = "http://localhost:5000";

            //var hostConfigs = new HostConfiguration()
            //{
            //    UrlReservations = new UrlReservations() { CreateAutomatically = true }
            //};

            NancyHost host = new NancyHost(new Uri(url));
            host.Start();

            Console.WriteLine("Nancy host is listening at {0}", url);
            Console.WriteLine("Press <Enter> to exit");

            Console.ReadLine();
        }
    }
}