using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        //Startup'daki ServiceTool geçici olduğu için onu sildik artık.
        //IServiceCollection apimizin servis bağımlılıklarını eklediğimiz , araya girmesini istediğimiz servisleri eklediğimiz koleksiyonların kendisidir.
        //IServiceCollection genişletmek istediğim için this ile başlıyorum
        //ICoreModule array olsun.
        //Kısaca bu bizim core katmanıda dahil olmak üzere ekleyeceğimiz injection'ları toplayabileceğimiz yapıya dönüştü.
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection serviceCollection, ICoreModule[] modules)
        {
            //Bize eklenen modüllerdeki her bir module için.
            foreach (var module in modules)
            {
                //Birden fazla module ekleyebileceğimizi gösteriyor.
                module.Load(serviceCollection);
            }
            //Servisleri create et.
            return ServiceTool.Create(serviceCollection);
        }
    }
}
