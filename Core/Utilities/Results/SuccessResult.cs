using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class SuccessResult:Result
    {
        //base'e yani Result'a bişey göndermek istiyosan :base yazıp gönderebilirsin.
        public SuccessResult(string message) : base(true,message)
        {

        }

        public SuccessResult() : base(true)
        {

        }
    }
}
