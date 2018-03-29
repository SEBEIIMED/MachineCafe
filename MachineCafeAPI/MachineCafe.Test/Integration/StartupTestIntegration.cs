using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace MachineCafe.Test
{
    public class StartupTestIntegration<T> : IDisposable where T : class
    {
        private readonly TestServer server;

        public StartupTestIntegration()
        {
            var host = new WebHostBuilder()
                            .UseStartup<T>()
                            .ConfigureServices(ConfigureServices);

            this.server = new TestServer(host);
            this.Client = this.server.CreateClient();
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            this.Client.Dispose();
            this.server.Dispose();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        { }
    }
}
