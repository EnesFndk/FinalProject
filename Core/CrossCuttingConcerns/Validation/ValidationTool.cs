using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Validation
{
    public class ValidationTool
    {
        //hepsinin base'i object olduğu için object ekliyoruz.
        //IValidator = aslında productvalidator çünkü onun içinde IValidator var.
        //***IValidator validator = doğrulama kurallarının olduğu class.
        //***object entity = doğrulanacak class.

        public static void Validate(IValidator validator,object entity)
        {
            //--------- AŞAĞIDAKİ KODUN TÜRKÇESİ ----------- VALIDATION
            //Validation yapacağımız zaman aşağıdaki kod standarttır.
            //EntityFramework gibi bir context yazıyoruz. Product için doğrulama yapıyoruz. Parametre'den gelen product için çalışacağım diyorsun. Çünkü <Product> generic. 
            //if içindekiler ise = eğer sonuç geçerli değil ise Error göster. 
            //Validate olan interface buluyoruz. ve Validate parametresi olarak yazıyoruz yukarı.
            //ValidationContext<Product>(product); Yerine ValidationContext<object>(entity); yazıyoruz.
            var context = new ValidationContext<object>(entity);
            //IValidator eklediğimiz için artık Aşağıdaki satıra ihtiyaç yok ama görmek için yorum satırı yapıyorum.
            //ProductValidator productValidator = new ProductValidator();
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
