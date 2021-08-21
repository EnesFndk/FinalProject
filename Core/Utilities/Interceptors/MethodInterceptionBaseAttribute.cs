using Castle.DynamicProxy;
using System;

namespace Core.Utilities.Interceptors
{
    //Burda Engin hocanın NetCoreBackEnd'deki kodunu yapıştırdıktan sonra , Nuget Package ile Autofac,Autofac Extension Dependency Injection ve Autofac Extras Dynamic Proxy'i Core katmanı için yüklüyoruz.
    //Zaten bunlar yüklü olduğu için install sekmesi üzerinden core için seçip core'a yüklüyoruz. Sonra çözüyoruz Castle'a göre.
    //Bu base yapı her yerde var.
    public partial class Class1
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
        public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
        {
            //Hangi Attribute önce çalışsın diye Priority veriyoruz.
            public int Priority { get; set; }

            public virtual void Intercept(IInvocation invocation)
            {

            }
        }
    }
}
