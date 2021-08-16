using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{

    public class EfProductDal : EfEntityRepositoryBase<Product, NorthwindContext>, IProductDal
    {
        public List<ProductDetailDto> GetProductDetails()
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                //aşağıdaki yapı , ürünlerle kategorileri join et (birleştir) demek.
                //on p.CategoryId equals c.CategoryId  = p'deki categoryid , c'deki categoryid eşitse onları join et demek.
                //select new ProductDetailDto {ProductId = p.ProductId, ProductName = p.ProductName, CategoryName = c.CategoryName, UnitsInStock = p.UnitsInStock }; yapı ise sonucu kolonlara uydurarak ver yani, productid'yi p'deki productid den al  productname'i p'deki Productname'den al vs vs gibi.
                //neden result.ToList yaptık çünkü var result Queryable istediği için. var result üstüne gelince gösteriyor. Böyle olunca ToList ekliyoruz.
                var result = from p in context.Products
                             join c in context.Categories
                             on p.CategoryId equals c.CategoryId
                             select new ProductDetailDto 
                             {
                                 ProductId = p.ProductId, ProductName = p.ProductName, 
                                 CategoryName = c.CategoryName, UnitsInStock = p.UnitsInStock 
                             };
                return result.ToList();
            }
        }
    }
}
