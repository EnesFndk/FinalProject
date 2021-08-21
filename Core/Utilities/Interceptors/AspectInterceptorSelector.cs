using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static Core.Utilities.Interceptors.Class1;

namespace Core.Utilities.Interceptors
{
    //CLASS1.CS OLARAK DOSYAMIZIN İÇİNDEYDİ METHODINTERCEPTION VE DİĞER 2 Sİ FAKAT BİZ MOVE TO TYPE OLARAK ONLARI SEÇİP İSİMLERLE DOSYA HALİNE GETİRDİK.
    //var classAttributes = type.GetCustomAttributes = Git class'ın attribute'larını oku
    //var methodAttributes = type.GetMethod == git method'un attribute'larını oku diyor. 
    //sonrasında ONLARI ÇALIŞMA SIRASINA KOY  diyor.
    //classAttributes.OrderBy(x => x.Priority) = Öncelik değerine göre sırala (Priority) 
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>
                (true).ToList();
            var methodAttributes = type.GetMethod(method.Name)
                .GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            classAttributes.AddRange(methodAttributes);
            //Otomatik olarak sistemdeki logları dahil et. diyor aşağıdaki kod'da. Şuan loglama altyapımız hazır olmadığı için yorum satırı haline getirdim.
            //classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger)));

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
