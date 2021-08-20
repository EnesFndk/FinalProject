using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        //******Mesela ProductName üstüne [Required] yazarak isim ürün ismini zorunlu tutabiliriz ve FluentValidation gerek olmayabilir fakat tc kimlik no ile oluşturduğumuzu düşünelim 
        //******Tc kimlik no zorunlu olsun , ilerki tarihlerde yabancı uyruklularda bu sisteme dahil edilirse patlarız o sebeple temiz kod ve güncellenebilir olması için  FluentValidation kullanmak şart.
        //****** VE SOLID 'in S harfine denk gelir.
        public string ProductName { get; set; }
        public short UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
