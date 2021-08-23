using Castle.DynamicProxy;
using System;
using static Core.Utilities.Interceptors.Class1;

namespace Core.Utilities.Interceptors
{
    //Engin hocanın MethodInterception dosyası içerisindeki kodları'da yapıştırıyoruz.
    //(IInvocation invation) = (add olarak sayabiliriz) bu aslında bizim çalıştırmak istediğimiz method oluyor.
    //Genelde OnBefore ve OnException genelde kullanılır.
    //OnBefore = Method'un başında çalışır. 
    //OnException = hata aldığında çalışsın dersek bunu yazıyoruz.
    //OnSuccess = Method başarılı olduğunda bu çalışsın.
    //OnAfter = method sonunda çalışsın.
    //Burda zaten genel bir try , catch yazdık. Bidaha bidaha yazıp kodları çorba etmeye gerek yok .
    //Biz sadece OnBefore içine yazıcaz. Method'un başında çalışıcak ve biticek.
    //Try ve catch nedir? Hata yakalamak için kullanılır. Mantık = önce bir dene sonra hatayı yakala. İster çalışsın ister hata versin finally çalışıyor.
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        //virtual = bizim ezmemizi bekleyen methodlar.
        //invocation = business method = add, delete, update, getall falan hepsi .
        //****Biz burda sadece OnBefore'un içini Core.Aspects.Autofac.Validation.ValidationAspect bu klasörde doldurduğumuz için sadece OnBefore çalışır. Diğerleri boş çünkü. Method'un başında çalışıyor.
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }
        protected virtual void OnSuccess(IInvocation invocation) { }
        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                isSuccess = false;
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (isSuccess)
                {
                    OnSuccess(invocation);
                }
            }
            OnAfter(invocation);
        }
    }
}
