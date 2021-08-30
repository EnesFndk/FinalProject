using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        //**CreateToken = Token üretecek mekanizma 
        //**Kim için oluşturuyoruz = User user
        //**Bu token'in içerisine hangi yetkileri koyalım = List<OperationClaim> opertionClaims
        //***Biz sistemimiz api sistemi ve Kullanıcı client'den giriş yapıyor ve api'ye gönderiyor. Burda CreateToken çalışıcak. Eğer doğru ise ilgili kullanıcı (User) OperationClaim'lerini bulucak Orda JWT üretecek ve client'e verecek.
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
    }
}
