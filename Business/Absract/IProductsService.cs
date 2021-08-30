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
        IResult Update(Product product);
        //Transactional Yönetimi ?= Uygulamalarda tutarlılığı korumak için yaptığımız yöntem
        //(Örn= benim hesapta 100 lira para var arkadaşıma 10 lira göndericem. Benim hesabımın 10 lira düşücek şekilde update edilmesi arkadaşımın hesabının 10 lira artacak şekilde update edilmesi )
        //yani 2 tane veritabanı işi var. Fakat benim hesaptan giderken güncelledi arkadaşımın hesabına yazarken sistem hata verdi. Paramı iade etmesi gerekiyor yani işlemi geri alması gerekiyor
        //
        IResult AddTransactionalTest(Product product);
    }
}
