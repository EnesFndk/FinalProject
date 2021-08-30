using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Core.Aspects.Autofac.Caching
{
    //*******Her method'un üstüne de cache Koyulmaz. Performansı arttırılır ama her methodda da kullanılmaz. (örn= ürünler filtreleriniyor ya onlar mesela cache'den gelir ama bir method 40 yılda 1 kullanılıyodur ama büyük data getiriyodur onu cache'e koymamak gerekiyor. )
    //CacheAspect yazmasaydık kodları product manager'da bütün methodlarda tek tek yazmak zorunda kalacaktık.
    //Bu kodlar benim için bu method çalışmaya başlamadan kontrol etsin cache de varsa cache'den getirsin yoksa veritabanından getirsin cache'e eklesin.
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        //Default değer vermişiz 60 dk. Biz süre vermezsek bu veri 60 dk boyunca cache'de durucak sonra cache'den uçucak. Oto sistem direk atıcak.
        public CacheAspect(int duration = 60)
        {
            //duration'u burda set etmişiz.
            _duration = duration;
            //hangi cachemanager'ı kullanacaksak onu belirtiyoruz. Injection yapıyoruz yani.
            //GetService<IMemoryCache>(); yazdıktan sonra kızarsa using Microsoft.Extensions.DependencyInjection; yaz using kısmına.
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        //Intercept'i override ediyoruz MethodInterception da vardı zaten. Ona entegre ediyoruz.
        //Getall'ı çalıştırmadan bu kodları çalıştırıyoruz.
        public override void Intercept(IInvocation invocation)
        {
            //Invocation = method demek.


            //******ASLINDA AŞAĞIDA ÇALIŞTIRDIĞIMIZ NAMESPACE, İSMİ , METHOD İSMİ, PARAMETRELERİNE GÖRE KEY OLUŞTURUYOR.********
            //******EĞER BU KEY DAHA ÖNCE VARSA DİREK CACHE'DEN AL YOKSA VERİTABANINDAN AL AMA CACHE EKLE DEMEK AŞAĞIDAKİ KODUN ÖZETİ**********

            //Bu methodumun ismini bulmaya çalışıyorum. 
            //string.Format = mesela nortwind gibi 
            //Invocation.Method'un (mesela getall), Getall'ın namespace'ini al (ReflectedType demek = Mesela IProductService için namespace'si Business.Abstract.IProductService)
            //kısaca invocation.Method.ReflectedType.FullName bu kod = namespace + class'ın ismini verir.
            //invocation.Method.Name = çalıştırdığımız method ismi (Mesela GetAll)
            //Kısaca aşağıda Nortwind.Business.Abstract.IProductService.GetAll'u yazdırdık.

            //********Hangi Key'i verdiğini öğrenmek için  if (_cacheManager.IsAdd(key)) oraya breakpoint koy çalıştır  if (_cacheManager.IsAdd(key)) üstüne gel zaten gösteriyor.
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            //invocation.Arguments.ToList(); = methodun parametrelerini listeye çevir diyoruz.
            var arguments = invocation.Arguments.ToList();
            //aşağıda key oluşturuyoruz.
            //Method'un parametrelerini tek tek , eğer parametre değeri var ise o paratmetre değerini (yukarıda GetAll vardı ya) GetAll'ın içerisine ekliyoruz.
            //GetAll() parametre değeri 1 verdiğimizi düşün o zaman içerisi 1 olcak. değer vermediysek null. Dedik aşağıdaki kodda.
            //string.Join = bir araya getirmek demek. nasıl getir. ("," ) = aralarına virgül koyarak bir araya getir. arguments.Select = parametrelerin her biri için virgül koy.
            //Mesela 2 parametre var birinin değeri ankara diğerinin 5 key oluştururken (Ankara, 5) olarak oluşturmamıza yarıyor.
            //?? = varsa ( x => x?.ToString() bunu ekle yoksa "<Null>"))}) ekle demek. O 2 soru işaretlerinin anlamı
            
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            //gidip key var mı bak bellekte böyle bir cache key'i var mı diye? Çünkü her seferinde aynı anahtar oluştuğu için daha önce var mı ?
            if (_cacheManager.IsAdd(key))
            {
                //eğer cache'de var ise yani true ise 
                //***********invocation.ReturnValue = methodun returndeğeri cache'deki data olsun demek. (Örn= GetAll'ın üstüne [CacheAspect] ekledik, veritabanından gitmesin de Ordaki return değeri cache'deki data olsun demek. )
                //hiç çalıştırmadan geri dön demek. Manuel olarak return oluşturduk aslında. 
                //_cacheManager.Get(key); = _cacheManager'dan get et çünkü neden ? cache'de var o sebeple.
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }
            //Methodu çalıştır diyoruz burda.
            //Method çalıştı veritabanından datayı getirdi.
            invocation.Proceed();
            //daha önce cache eklenmedi ise key, invocation.ReturnValue, _duration eklenmiş olacak.
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
            //****Bunu postman'de http://localhost:45495/api/products/getall yazarak kontrol ediyoruz if (_cacheManager.IsAdd(key)) oraya breakpoint koyarak hangi key'i verdi görebiliyoruz.
            //****http://localhost:45495/api/products/getall yazarak send dedikten sonra cache'de yok ise veritabanından alıp cache'e veriyor tekrar send dedikten sonra cache hafızasına aldığı için direk çalışıyor
            //****aynı şey http://localhost:45495/api/products/getbyid içinde geçerli çünkü productManager'deki getall ve getbyid'ye [CacheAspect] yazdık.
        }
    }
}