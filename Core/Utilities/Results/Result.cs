using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {

        //Sadece get ettik orda set etmedik. Constructor'da set edebiliyoruz. Message = message; dediği ise Public Message'i message olarak set et demek. 
        //Aslında get; set; olarak da yazabilirdik ama Standart code için yani herkes tarafından anlaşılır olması için böyle bir yapı yaptık. Yazılımcı kafasına göre kodlama yapmasın diye yaptık.
        //this dediğimizde class'ın kendisi demek oluyor.
        //this(success) = constructor'ına success'i yolla demek oluyor. Şuan ikiside sorunsuz çalışıyor. Kod kendini tekrar etmemiş oluyor.
        public Result(bool success, string message):this(success)
        {
            Message = message;
        }

        //Sadece true false döndürmek için bunu yazdık. Mesela Ürün eklendi mesajını vermek istemiyorsak böyle yapıcaz.
        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}
