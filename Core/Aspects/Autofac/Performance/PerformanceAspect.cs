using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval;
        //Stopwatch?= timer olarak sayılıyor. Sayaç
        //Bunu coreModule'de zaten injection yapıyorum.
        private Stopwatch _stopwatch;

        //PerformanceAspect çağırınca interval veriyorum.Peki interval ne?=  Method'un çalışması için geçen süre diyebiliriz.
        //Mesela ProductManager'da getbyid'ye [PerformanceAspect(5)] verdik bu , getbyid method'u 5 sn geçerse beni uyar demek. 
        //[PerformanceAspect(5)] yazmaktansa AspectInterceptorSelector içine Aynı FileLogger gibi Performance'yide eklersek heryerde çalışır ve method'ların başına ekledik mi diye kontrol etmeye çalışmayız. 
        public PerformanceAspect(int interval)
        {
            _interval = interval;
            //Stopwatch'ın implementasyonunu yaptık coremodule'de.
            //GetService<Stopwatch>(); yazdıktan sonra kızarsa using Microsoft.Extensions.DependencyInjection; yaz using kısmına.
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }

        //Metonun önünde sayacı başlatıyorum.
        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }
        //Method'un sonunda 
        protected override void OnAfter(IInvocation invocation)
        {
            //O ana kadar ki geçen süreyi hesaplıyorum.
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            }
            _stopwatch.Reset();
        }
    }
}
