using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{        
    //Data Transformation Object
    //gerçek hayatta hani e-ticaret sitesinde ürün ismi ve kategori ismi yan yana yazar ya biz şuan yapmaya kalksak product.ProductName + "/" + product.CategoryId yazabiliriz anca
    //o sebepten dolayı DTO kullanıyoruz.
    public class ProductDetailDto:IDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public short UnitsInStock { get; set; }
    }
}
