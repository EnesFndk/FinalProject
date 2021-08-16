using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Absract
{
    public interface IProductsService
    {
        //bu fiyat aralığındaki fiyatları getir = getbyunitprice decimal olanlar.
        //IDataResult'daki <T> 'nin karşılığı = List<Product>
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<List<ProductDetailDto>> GetProductDetails();
        IDataResult<Product> GetById(int productId);
        IResult Add(Product product);
        
    }
}
