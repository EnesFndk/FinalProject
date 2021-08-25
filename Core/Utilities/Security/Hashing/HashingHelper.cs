using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Hashing
{

    //HashingHelper bizim için bir araç. Bir bağlayıcılığı yok. O yüzden çıplak kalabilir.
    //HashingHelper hash oluşturmaya ve onu doğrulamaya yarıyor.
    public class HashingHelper
    {
        //String password dedikten sonra out'larla oraya gönderilen değerleri (boş bile olsa) doldurup geri döndürmeye yarıyor.
        //Burdaki out dışarı verilecek değer gibi düşünebiliriz. Birden fazla veriyi döndürmek için de kullanılır. 
        //Biz password vericez ve 2 out'uda dışarıya vericek.
        //hmac istediğimiz şekilde verebiliriz. result yapsakda olur fakat farklı olması daha iyi.
        //HMACSHA512() yeni bir class ürettiği için "()" alıyoruz.
        //passwordHash = hmac.ComputeHash((password);  yazdığımız zaman bizden byte cinsinden istediği için alttaki gibi yazıyoruz.
        //passwordSalt = hmac.Key; = burdaki Key oluşturduğumuz algoritmada'ki "System.Security.Cryptography.HMACSHA512())" Key değeridir. O an oluşturduğu. O sebeple her kullanıcı için farklı key oluşturuyor. 
        //Verdiğimiz password'un Hash'ini oluşturuyor.
        public static void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        //Burda out vermiyoruz. Çünkü değerleri biz vericez.
        //Veritabanındaki Has ve Salt ile Kullanıcının gönderdiği password'ün karşılaştırıcaz. Eğer birbirine eşitse true değil ise false dönücek.Bu bizim veritabanımızdaki Hash'imiz olucak.
        //HMACSHA512 = bize kullandığımız key anahtarını soruyor o sebeple HMACSHA512(passwordSalt)) yazıyoruz.
        //Bu aşağıdaki "string password" sisteme kayıt oldu. Farklı zamanda tekrar girmeye çalışırken girdiği parola.
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                //hesaplanan Hash'in bütün değerlerini tek tek dolaş demek for içinde yazan.
                for (int i = 0; i < computedHash.Length; i++)
                {
                    //eğer benim computedHash'imin i 'ninci değeri eşit değil ise veritabanından gönderilen Hash'in i'ninci değeri değil ise false. Yani 2 değer birbiriyle eşleşiyor.
                    if (computedHash[i] !=passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
