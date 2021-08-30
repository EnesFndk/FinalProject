using Business.Absract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryprtion;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIx
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInject --> IoC Container alt yapýsý kuruyor ve bunlardan birini kullanýcaz. "services.AddSingleton<IProductDal, EfProductDal>();" bunun gibileri yazmamak için. gene clean cod yapýyoruz.
            //yukarýdaki iþlemler AOP tekniðiyle yapýcaz.
            //Eðer IProductsService isterse ona newlenmiþ ProductManager ver demek. IoC yöntemi ile.
            //**************Hangi interface'in karþýlýðý nedir? API kýsmýnda deðil (Business) Backend de  Autofac ile yapýcaz. Performans açýsýndan çok önemli.*********
            //services.AddSingleton<IProductsService,ProductManager>();
            //aþaðýdakinde de IProductDal isterse ona newlenmiþ EfProductDal veriyor. IoC yöntemi ile.
            //services.AddSingleton<IProductDal, EfProductDal>();

            //HttpContextAccessor = her yapýlan istekle ilgili oluþan context. Bizim client bir istekte bulunduðu zaman baþtan sona kadar HttpContextAccessor takip ediyor.
            //Burda instance oluþturuyoruz fakat devreye girmesi için ServicesTool yazdýk aþaðýya.
            //********Burdaki kodu Diðer projelerde de kullanabileceðimiz standarta getirmek için Core/DependencyResolvers/CoreModule tarafýna yazýyoruz. Servisler artýk orada toplanýcak. O sebeple yorum satýrý yaptým.
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            //Microsoft.AspNetCore.Authentication.JwtBearer 3.1.12 sürümünü yüklüyoruz çünkü çalýþmýyor diðerleri
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Issuer bilgisini Validate Edeyim mi ? bizde true deyip et diyoruz.
                        //Biz token veriyoruz ýssuer olarak Enes@enes.com olarak veriyoruz o bilgi bize geri geliyor.
                        //Bu hepsinde geçerli.
                        ValidateIssuer = true,
                        //Audience'yide kontrol et.
                        ValidateAudience = true,
                        //Token'ýn yaþam döngüsünü kontrol edeyim mi ? Token olsa yeter bana diyoruz.
                        ValidateLifetime = true,
                        //biz bunu tokenoptions'daki issuer'dan alýyoruz. Ayný þekilde Audience içinde geçerli.
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        //anahtarýda kontrol edeyim mi ? true diyoruz.
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });
            //yukarýda yazdýðýmýz kodlardan autofac haberdar deðil o sebeple ServiceTool yazýyoruz.
            //aþaðýdaki geçici çözüm olduðundan yorum satýrý yapýyorum. onun altýndaki asýl nokta.
            //ServiceTool.Create(services);


            //****Buraya sadece CoreModule yazarsak. HttpContextAccessor'ü injection ettik ya Core altýnda CoreModule'den Yarýn öbür gün farklý birþeyi injection edersek diye birden fazla Module yazmak için AddDependencyResolvers yazýyorum.
            //****AddDependencyResolvers içine gelmediði için Extension oluþturmak adýna ServiceCollectionExtensions ekledik
            //****ICoreModule'ü array halinde (params yada koleksiyon olarak da yapabiliriz) new'ledik ve artýk "{}" içine istediðimiz kadar Module yazabiliriz.
            //****Bu hareket yarýn öbürgün CoreModule gibi farklý modullerde oluþturursak injectionlar için onlarýda istediðimiz kadar oluþturup burda new CoreModule(), "," ekleyip devam ettirebiliriz.
            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule()
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //Bu asp yaþam döngüsünde hangi sýrayla devreye gireceðini belirtiyoruz.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //sýrasý ile ilk Authentication sonra Authorization yapmak lazým çünkü sýrayla çalýþtýðý için.
            //bide UseAuthentication ekliyoruz.
            //Authentication = eve girmek diyebiliriz.
            app.UseAuthentication();
            //Authorization = evin içerisine bir þey yapmaktýr.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
