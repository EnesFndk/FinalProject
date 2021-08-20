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

        //Burada WebAPI>startup yerine Business'deki AutofacBusinessModule kullanmam�z i�in AutofacServiceProviderFactory kulland�k.
        //Bunu extensions.DependencyInjection olarak install yapt�k. daha sonras�nda kodlar a�a��daki gibi.
        //****�NEML�*****Mesela yar�n Autofac'den vazge�ip ba�ka birini kullan�rsan yapaca��n �eyler Business.DependencyResolvers i�ine dosya kurup, i�indekileri yazmak , bu k�s�mda da AutofacServiceProviderFactory ve AutofacBusinessModule se�ti�in �eye g�re de�i�tirmek.
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
