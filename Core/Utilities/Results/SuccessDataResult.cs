using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class SuccessDataResult<T>:DataResult<T>
    {
        //(T data, string message):base(data,true,message) = data ve message ver. Ama base işlem sonucu data'dır , işlem sonucu true'dur ve message verir.
        //bunun gibi 4 tane alternatif yazdık. hangisini kullanmak istersek onu yazabiliriz.
        //Sadece 1 kez yazıyoruz  ve bidaha yazmıyoruz. Çok iyi kod haline geliyor.
        public SuccessDataResult(T data, string message):base(data,true,message)
        {

        }
        public SuccessDataResult(T data):base(data,true)
        {

        }
        public SuccessDataResult(string message):base(default,true,message)
        {

        }
        public SuccessDataResult():base(default,true)
        {

        }
    }
}
