using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    //Tüm projelerde kullanabileceğimiz injection'lar kodlar burda yer alacak.
    public interface ICoreModule
    {
        //Bağımlılıkları yazıcaz.Servisleri Load yüklüyor olacak. 
        //Startup'daki IServiceCollection'u verdik. Ordaki işlemleri burası yapacak.
        void Load(IServiceCollection serviceCollection);
    }
}
