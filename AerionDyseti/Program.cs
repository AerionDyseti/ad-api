﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AerionDyseti
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseUrls("http://*:3000").Build();
        }
    }
}