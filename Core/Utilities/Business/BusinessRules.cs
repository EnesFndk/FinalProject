using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    //Buraya iş kurallarını göndericez. ProductManager'de Add içerisine yazdığım kuralları düzgün hale getirmek için burada düzenliyorum.
    //Business içine değil Core içine yazdım. Çünkü base'de  yer alması lazım 
    //Ek bilgi ------------JAVADA STATIC BİR PUBLIC OLURSA CLASS'DA STATIC OLMALI --------------
    public class BusinessRules
    {
        //params yazdığımız zaman Run içerisine istediğimiz kadar IResult parametresi verebiliyoruz. ProductManager içerisine yazdığımız. "Run()" içerisine istediğimiz kadar yazabiliyoruz.
        //logics = iş kuralı demek. 
        //logics dediklerimiz "(CheckIfProductCountOfCategoryCorrect(product.CategoryId)" bu ve (CheckIfProductNameExists(product.ProductName) bu gibi iş kuralları
        public static IResult Run(params IResult[] logics)
        {
            //bütün iş kurallarını gez = foreach (var logic in logics)
            foreach (var logic in logics)
            {
                //logic'in success durumu başarız ise = (!logic.Success)
                //eğer başarılı ise demek isteseydik = (logic.Success)
                if (!logic.Success)
                {
                    //iş kuralından başarısız olanı business'a haberder ediyoruz. Şu logic hatalı diyip 
                    //logic dediğimiz (CheckIfProductCountOfCategoryCorrect(product.CategoryId) yada (CheckIfProductNameExists(product.ProductName)
                    //return logic = kurala uymayan.
                    return logic;
                }
            }
            //başarılıysa hiç bir şey döndürmesine gerek yok o sebeple null
            return null;
        }
















        //Yukarıdaki kodu böyle de yapabiliriz. İf içerisine başarısız logic'leri yazabiliriz.
        //public static List<IResult> Run(params IResult[] logics)
        //{
        //    List<IResult> errorResults = new List<IResult>();
        //    foreach (var logic in logics)
        //    {
        //        if (!logic.Success)
        //        {
        //            errorResults.Add(logic);
        //        }
        //    }
            
        //    return errorResults;
        //}
    }
}
