using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
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
            //MemoryacheManager'daki "IMemoryCache _memoryCache;" karşılığı var anlamına geliyor alttaki kod'da.
            //Hazır bir injection aslında bu C# ile gelen. Hazır bir ICacheManager instance'si oluşturuyor.
            serviceCollection.AddMemoryCache();
            //yarın öbürgün redis'e geçmek istersen MemoryCacheManager yerine RedisCacheManager yazman yeterli ve serviceCollection.AddMemoryCache(); silmen yeterli olacaktır. Şuan Microsoft'un base alternatifini kullanıyoruz.
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
        }
    }
}
