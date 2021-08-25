using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    //*****JWT ÇALIŞMA SİSTEMİ ********
    //*****Client(firefox, chrome, mobil uygulamalar vs.) Api tarafına bir istekte bulunuyor. Token'ı yoksa api uygulaması cevap olarak http yanıtı (olumsuz) dönüyor. 
    //*****Client benim kullanıcı adım ve şifrem var yada email parola istekte bulunuyor. Api app'de olumlu jwt dönüyor. Client bunu hafızasında tutuyor. Veri kaynağında, cookie 'de gibi
    //*****Client bundan sonraki isteklerinde (mesela ürün ekleme olsun) bide jwt ekliyor , yani diyor ben yetkimle anahtarımla geldim diyor. Api app ise database'ine bakıyor. Api olumlu yada olumsuz cevap dönüyor.

    //AccessToken anlamsız karakterlerden oluşan bir anahtar değeri olduğu için string.
    //Expiration = user'a verilen token'in bitiş süresi.
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
