using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    //DataResult<T> hem class hem interface içeriyor. Bu sebepten dolayı DataResult ctor'unu yapıyoruz.
    //Result'daki success ve message yazmamak için base(success,message) yazdık.
    //eğer message yazdırmak istemiyorsam (T data, bool success):base(success) böyle yazdırıyorum.
    //Data = data; = Data'yı set ediyoruz bu şekilde.

    {
        public DataResult(T data, bool success, string message):base(success,message)
        {
            Data = data;
        }

        public DataResult(T data, bool success):base(success)
        {
            Data = data;
        }
        public T Data { get; }
    }
}
