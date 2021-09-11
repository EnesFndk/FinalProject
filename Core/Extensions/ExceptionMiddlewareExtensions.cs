using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        //Kendi Middleware'imizi yazmamız için gereken işlem.
        //Burda özetle startup'daki configurate içerisine hata yakalamayı(middleware) da ekle diyoruz. Aşağıdaki app.UseMiddleware zaten configurate'ler ile aynı.
        //Bu koddan sonra startup'a  app.ConfigureCustomExceptionMiddleware(); ekliyoruz.
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
