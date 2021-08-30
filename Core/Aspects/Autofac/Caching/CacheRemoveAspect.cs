using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Caching
{
    //CacheRemoveAspect ne zaman çalışır?= Data bozulursa. Ne zaman bozulur ?= yeni data eklenirse, data güncellenirse, data silinirse.
    //Veriyi Manipüle eden methodlarına CacheRemoveAspect uygularsın.
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheManager _cacheManager;

        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
            //GetService<IMemoryCache>(); yazdıktan sonra kızarsa using Microsoft.Extensions.DependencyInjection; yaz using kısmına.
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }
        //Neden OnSuccess?= belki de add işlemi hata verecek veritabanına yeni ürün ekleyemeyecek.Yeni ürün ekleyememişken ben neden cache'imi sileyim. 
        //Özet olarak Method başarılı olursa git ekle demek bu.
        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}
