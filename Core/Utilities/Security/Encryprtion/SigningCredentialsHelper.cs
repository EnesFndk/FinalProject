using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Encryprtion
{
    public class SigningCredentialsHelper
    {
        //JWT servislerinin webapi'nin kullanabileceği jwt oluşturulabilmesi için bir anahtara ihtiyacımız olduğu için bunu yazıyoruz. O yüzden parametre olarak elimizdeki SecurityKey formatında vericez
        //Credentials = giriş bilgileri (kullanıcı adi, parola'dır.) Yani sisteme girebilmemiz için elimizde olanlardır.
        //Oda bize imzalama nesnesini döndürüyor olacak.
        //Hash'in işlemi yapacağımız için anahtar olarak securityKey kullan , şifreleme olarak da (güvenlik algoritmalarından) HmacSha512Signature kullan diyoruz. 
        //Biz SecurityKeyHelper'da HmacSha512 kullandık ama bunu webapi'de de ihtiyacı olduğu için HmacSha512Signature yazıyoruz.

        //Aslında hangi güvenlik anahtarı kullanacaksın , hangi algoritmayı kullanacaksın diyoruz aşağıdaki kodda.
        //bu HmacSha512 yada HmacSha256 güçlendirilmiş security'ler.
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha512Signature);
        }
    }
}
