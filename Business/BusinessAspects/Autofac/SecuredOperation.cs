using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;
using Business.Constants;

namespace Business.BusinessAspects.Autofac
{
    //Autoration Aspect'ler (SecuredOperation) genellikle business'e yazılır çünkü her projenin yetkilendirme mekanizması değişebilir.
    //Bunu engin hocanın github'undan aldık.
    //SecuredOperation jwt için gerekli bir yapı.
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        //jwt için her istekte httpcontext oluşturuyor.
        private IHttpContextAccessor _httpContextAccessor;

        //bana rolleri ver diyorum. ProductManager'da [SecuredOperation("product.add, admin)")] böyle bir yapı yazdık ya ("product.add, admin") bunlar işte roller.
        public SecuredOperation(string roles)
        {
            //bir metni bizim belirttiğimiz karaktere göre ayırıp array yapıyor.Yani ("product.add, admin) bunlar 2 elemanlı bir array haline geliyor.
            _roles = roles.Split(',');
            //Configuration'u enjecte edebildik fakat Aspect'i enjecte edemiyoruz o sebeple .Net'in kendi Service'ini Autofac ile oluşturduğumuz serviceprovider'e ulaş ve getservice'le getir yani.
            //Servicetool kullanarak windows form içinde çalıştırabiliyoruz.
            //ServiceTool kullanarak injection (builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();) altyapımızı okuyabilmemize yarayan bir araç olucak.
            //yani şöyle "productService = ServiceTool.ServiceProvider.GetService<IProductService>();" şeklinde autofac'deki injection değerlerini alabilecek. bu işe yarıyor.
            //Burda GetService bize kızıyor o yüzden  Microsoft.Extensions.DependencyInjection 'ı  yükleyip çözdürmesi gelmezse elle yazalım using kısmına.
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }
        //Method'un önünde çalıştır demek . Method'da add metodu olabilir.
        protected override void OnBefore(IInvocation invocation)
        {
            //O anki kullanıcının Claimroles (Kurallarını) bul diyor.
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            //kullanıcının rollerini gez
            foreach (var role in _roles)
            {
                //claim'lerin içlerinde ilgili role var ise methodu çalıştırmaya devam et = return
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            //Eğer yok ise yetkin yok hatası ver.
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
