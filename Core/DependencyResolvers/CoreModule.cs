using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        //ICoreModule entegre ettik ve implemente ettikten sonra webApi'deki Startup'ta yazdığımız singleton'u buraya yazıyoruz. Her projede kullanılacak standarta getirmek için.
        //Service bağımlılıklarını çözümlüyeceğimiz yer. Zaten DependencyResolvers = bağımlılıkları çözdürmek .
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
