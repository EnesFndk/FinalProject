using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class ErrorResult:Result
    {
        //base'e yani Result'a bişey göndermek istiyosan :base yazıp gönderebilirsin.
        public ErrorResult(string message) : base(false, message)
        {

        }

        public ErrorResult() : base(false)
        {

        }
    }
}
