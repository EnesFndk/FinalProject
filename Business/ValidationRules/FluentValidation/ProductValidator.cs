using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    //********neden çift klasörlüyoruz. ValidationRules altına FluentValidation yada DependencyResolvers altında Autofac olarak neden klasörledik. Çünkü Gelecekte farklı bir yapıyla çalışınca başka klasör kurup direk değişiklikleri yapabiliriz.
    public class ProductValidator : AbstractValidator<Product>
    {
        //Hangi nesne için validator yazacaksak bu ctor içine yazıyoruz. Dto'lar içinde olabilir.
        //RuleFor'u ProductName için böyle tek satır yapabiliriz = RuleFor(p => p.ProductName).NotEmpty().MinimumLength(2); gibi fakat bu SOLID 'in prensiplerine ters o yüzden tek tek yazmak daha iyi
        //Gelecekte içine When vs koyarsak bizim için zor olur. O sebeple tek tek yazmak daha mantıklı.
        public ProductValidator()
        {
            //ProductName'in min. 2 karakterli olmalıdır.
            RuleFor(p => p.ProductName).MinimumLength(2);
            //farklı farklı işlemler var burdan kuralları yazabiliyoruz.
            RuleFor(p => p.ProductName).NotEmpty();
            RuleFor(p => p.UnitPrice).NotEmpty();
            //UnitPrice 0 dan büyük olmalı.
            RuleFor(p => p.UnitPrice).GreaterThan(0);
            //içecek kategorilerinin min fiyatı 10 lira olmalıdır. CategoryId=1 içecek gibi mesela.
            //********Normalde hata mesajını dillere göre çeviri yaparak veriyor fakat sen mesajını kendin yazmak istiyorsan .WithMessage(" "); olarak verebiliriz.
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(15).When(p => p.CategoryId == 1);
            //***Ürünlerimin ismi A ile başlamalı gibi kural koymak istiyoruz. Bunu tamamen biz uyduruyoruz. Kendi yazacağımız method. Bize kızıcak o yüzden Generate method yapıp çözüyoruz.
            RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile Başlamalı");
        }


        //arg = productName
        //ProductName A ile başlamalı.
        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}
