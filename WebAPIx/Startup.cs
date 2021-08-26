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
            //Autofac, Ninject, CastleWindsor, StructureMap, LightInject, DryInject --> IoC Container alt yap�s� kuruyor ve bunlardan birini kullan�caz. "services.AddSingleton<IProductDal, EfProductDal>();" bunun gibileri yazmamak i�in. gene clean cod yap�yoruz.
            //yukar�daki i�lemler AOP tekni�iyle yap�caz.
            //E�er IProductsService isterse ona newlenmi� ProductManager ver demek. IoC y�ntemi ile.
            //**************Hangi interface'in kar��l��� nedir? API k�sm�nda de�il (Business) Backend de  Autofac ile yap�caz. Performans a��s�ndan �ok �nemli.*********
            //services.AddSingleton<IProductsService,ProductManager>();
            //a�a��dakinde de IProductDal isterse ona newlenmi� EfProductDal veriyor. IoC y�ntemi ile.
            //services.AddSingleton<IProductDal, EfProductDal>();

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            //Microsoft.AspNetCore.Authentication.JwtBearer 3.1.12 s�r�m�n� y�kl�yoruz ��nk� �al��m�yor di�erleri
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
        //Bu asp ya�am d�ng�s�nde hangi s�rayla devreye girece�ini belirtiyoruz.
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
