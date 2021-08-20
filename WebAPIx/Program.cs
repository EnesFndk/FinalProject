using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIx
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //Burada WebAPI>startup yerine Business'deki AutofacBusinessModule kullanmamýz için AutofacServiceProviderFactory kullandýk.
        //Bunu extensions.DependencyInjection olarak install yaptýk. daha sonrasýnda kodlar aþaðýdaki gibi.
        //****ÖNEMLÝ*****Mesela yarýn Autofac'den vazgeçip baþka birini kullanýrsan yapacaðýn þeyler Business.DependencyResolvers içine dosya kurup, içindekileri yazmak , bu kýsýmda da AutofacServiceProviderFactory ve AutofacBusinessModule seçtiðin þeye göre deðiþtirmek.
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacBusinessModule());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
