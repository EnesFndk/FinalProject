using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public interface IDataResult<T>:IResult
        //işlem sonucu, mesaj ve data içersin istiyoruz. IResult'da işlem sonucu ve mesaj olduğu için IResult'u ekliyoruz. hangi tiple çalışıcağımızı yazmak için <T> generic yapıyoruz.
    {
        T Data { get; }
    }
}
