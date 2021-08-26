using Business.Absract;
using Business.Concrete;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryprtion;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            //Microsoft.AspNetCore.Authentication.JwtBearer 3.1.12 sürümünü yüklüyoruz çünkü çalýþmýyor diðerleri
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
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
            //bide UseAuthentication ekliyoruz.
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
