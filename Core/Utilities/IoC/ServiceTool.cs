using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        //Kısacası bu kod bizim webapi controller'da oluşturduğumuz "builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();" bu yapıları (injection) oluşturabilmemize yarıyor.
        //.Net Core'un ServiceCollection'larını al ve build et.
        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
