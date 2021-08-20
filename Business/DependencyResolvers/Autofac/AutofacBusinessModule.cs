using Autofac;
using Business.Absract;
using Business.Concrete;
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
        protected override void Load(ContainerBuilder builder)
        {
            //IProductsService istenilirse ProductManager new'le. Startup'da yazdığımız kodun aynısı.
            builder.RegisterType<ProductManager>().As<IProductsService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();
        }
    }
}
