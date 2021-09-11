using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    //ExcetionMiddleware = olurda bir hata olursa nasıl davranmak gerekiyor? burda o kodlar var.
    //Middleware'ler aslında webApi'de startup'da configure altındaki app.use ile başlayanlardır. Biz burda Kendimiz bir middleware yazıyoruz.
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //**********Burdaki try catch ile bütün sistem try catch'lendi.*******
        //InvokeAsync= her zaman çalışan method olduğu için bütün methodlar try catch'e alıyoruz.
        public async Task InvokeAsync(HttpContext httpContext)
        {
            //hata olmazsa devam et 
            try
            {
                await _next(httpContext);
            }
            //hata olursa handle ediyor handle ise aşağıda.
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            //tarayıcıya ben sana bir tane json yolladım diyor. Gönderdiğimiz ContentType ile alakalı tamamen.
            httpContext.Response.ContentType = "application/json";
            //StatusCode = gönderdiğimiz hata kodu.
            //bir hata olursa InternalServerError
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            //Mesaj olarak böyle yazdık.
            string message = "Internal Server Error";
            //eğer aldığım hata ValidationException ise o zaman message'yi e.Message ile değiştir . 
            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
            }

            //olurda bir hata olursa 
            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
