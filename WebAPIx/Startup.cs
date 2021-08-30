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
            //Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInject --> IoC Container alt yap�s� kuruyor ve bunlardan birini kullan�caz. "services.AddSingleton<IProductDal, EfProductDal>();" bunun gibileri yazmamak i�in. gene clean cod yap�yoruz.
            //yukar�daki i�lemler AOP tekni�iyle yap�caz.
            //E�er IProductsService isterse ona newlenmi� ProductManager ver demek. IoC y�ntemi ile.
            //**************Hangi interface'in kar��l��� nedir? API k�sm�nda de�il (Business) Backend de  Autofac ile yap�caz. Performans a��s�ndan �ok �nemli.*********
            //services.AddSingleton<IProductsService,ProductManager>();
            //a�a��dakinde de IProductDal isterse ona newlenmi� EfProductDal veriyor. IoC y�ntemi ile.
            //services.AddSingleton<IProductDal, EfProductDal>();

            //HttpContextAccessor = her yap�lan istekle ilgili olu�an context. Bizim client bir istekte bulundu�u zaman ba�tan sona kadar HttpContextAccessor takip ediyor.
            //Burda instance olu�turuyoruz fakat devreye girmesi i�in ServicesTool yazd�k a�a��ya.
            //********Burdaki kodu Di�er projelerde de kullanabilece�imiz standarta getirmek i�in Core/DependencyResolvers/CoreModule taraf�na yaz�yoruz. Servisler art�k orada toplan�cak. O sebeple yorum sat�r� yapt�m.
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            //Microsoft.AspNetCore.Authentication.JwtBearer 3.1.12 s�r�m�n� y�kl�yoruz ��nk� �al��m�yor di�erleri
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Issuer bilgisini Validate Edeyim mi ? bizde true deyip et diyoruz.
                        //Biz token veriyoruz �ssuer olarak Enes@enes.com olarak veriyoruz o bilgi bize geri geliyor.
                        //Bu hepsinde ge�erli.
                        ValidateIssuer = true,
                        //Audience'yide kontrol et.
                        ValidateAudience = true,
                        //Token'�n ya�am d�ng�s�n� kontrol edeyim mi ? Token olsa yeter bana diyoruz.
                        ValidateLifetime = true,
                        //biz bunu tokenoptions'daki issuer'dan al�yoruz. Ayn� �ekilde Audience i�inde ge�erli.
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        //anahtar�da kontrol edeyim mi ? true diyoruz.
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });
            //yukar�da yazd���m�z kodlardan autofac haberdar de�il o sebeple ServiceTool yaz�yoruz.
            //a�a��daki ge�ici ��z�m oldu�undan yorum sat�r� yap�yorum. onun alt�ndaki as�l nokta.
            //ServiceTool.Create(services);


            //****Buraya sadece CoreModule yazarsak. HttpContextAccessor'� injection ettik ya Core alt�nda CoreModule'den Yar�n �b�r g�n farkl� bir�eyi injection edersek diye birden fazla Module yazmak i�in AddDependencyResolvers yaz�yorum.
            //****AddDependencyResolvers i�ine gelmedi�i i�in Extension olu�turmak ad�na ServiceCollectionExtensions ekledik
            //****ICoreModule'� array halinde (params yada koleksiyon olarak da yapabiliriz) new'ledik ve art�k "{}" i�ine istedi�imiz kadar Module yazabiliriz.
            //****Bu hareket yar�n �b�rg�n CoreModule gibi farkl� modullerde olu�turursak injectionlar i�in onlar�da istedi�imiz kadar olu�turup burda new CoreModule(), "," ekleyip devam ettirebiliriz.
            services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule()
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //Bu asp ya�am d�ng�s�nde hangi s�rayla devreye girece�ini belirtiyoruz.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //s�ras� ile ilk Authentication sonra Authorization yapmak laz�m ��nk� s�rayla �al��t��� i�in.
            //bide UseAuthentication ekliyoruz.
            //Authentication = eve girmek diyebiliriz.
            app.UseAuthentication();
            //Authorization = evin i�erisine bir �ey yapmakt�r.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
