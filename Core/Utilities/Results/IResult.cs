using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    //Temel voidler için başlangıç
    //İşlem sonucu ve bilgilendirmenin interface'ini yazıyoruz. Bool = true false için yazdık.
    //get sadece okuma set yazma oluyor.
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}
