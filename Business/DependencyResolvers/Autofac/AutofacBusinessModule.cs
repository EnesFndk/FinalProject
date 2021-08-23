using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Absract;
using Business.CCS;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers.Autofac
{   
    //********neden çift klasörlüyoruz. ValidationRules altına FluentValidation yada DependencyResolvers altında Autofac olarak neden klasörledik. Çünkü Gelecekte farklı bir yapıyla çalışınca başka klasör kurup direk değişiklikleri yapabiliriz.
    //Module yazıp çözdürcez Autofac'e göre.
    //biz niye böyle bi class oluşturduk. çünkü webapi'deki startup'da hani "services.AddSingleton<IProductsService,ProductManager>();" yazdık ya onların alt yapısı bunlar 
    //Onları yazmak için böyle class oluşturmak lazım ve 1 kere yazıcaz. Backend.
    //override deyip space'e tıklayıp load'ı seçiyoruz.
    public class AutofacBusinessModule:Module
    {
        //KISACASI AUTOFAC NE YAPIYOR ? = BÜTÜN SINIFLARIMIZ İÇİN ÖNCE AspectInterceptorSelector ÇALIŞTIRIYOR. GİT BAK DİYOR ASPECT'I VAR MI ? ONDAN SONRA İŞLEMLERİ YAPIYOR.
        protected override void Load(ContainerBuilder builder)
        {
            //IProductsService istenilirse ProductManager new'le. Startup'da yazdığımız kodun aynısı.
            builder.RegisterType<ProductManager>().As<IProductsService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();


            //Çalışan uygulamalar içerisinde 
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //implemente edilmiş interface'leri bul
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    //onlar için AspectInterceptorSelector'u çağır diyoruz.
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
