using Business.Absract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //[ApiController] bu bir Attribute'dür . Attribute nedir ? Attribute ise bir class ile ilgili bilgi verme yöntemidir.
        //API'lerde çoğul isim veriyoruz.Product değil Products, Category değil Categories.
        //IoC container - Inversion of Control = yani bu bir havuzda newlenmiş olan referanslar var onları istediğimizde alabiliyoruz onun içinden. WEBAPI'da Startup içerisine yazdık. "services.AddSingleton<IProductsService,ProductManager>();"
        IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Dependency Chain var burda = yani IProductsService ProductManager'e ihtiyaç duyuyor. ProductManager ise EfProductDal'a ihtiyaç  duyuyor.
            //Kötü kod = IProductsService productsService = new ProductManager(new EfProductDal());
            //yukarıdaki kötü kod yerine startup'da IoC ile Newlenebilir yapılar yaptık ve clean cod olarak yazdık.
            //Postman'deki Status önemli oraya bakarak veri gelişini görebiliyoruz. Baktık 400 bad request yaptı = sistem bakımda mesajı verdi. status 200 OK ise çalışıyor.

            //burda dataloaded ekledik onu görmemiz için 5 sn bekleticez.
            Thread.Sleep(1000);

            var result = _productsService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //Get içine int id koyduk çünkü üstteki ile aynı olduğu için çalışmaz ve GetById için.
        //isim veriyoruz ki karışmasın çünkü çalıştırınca patlıyor ("getall") yada ("getbyid")
        //postman'de çalışması için ise https://localhost:44368/api/products/getall sonuna getall yada getbyid yazıyoruz.
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productsService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        //routerLink için backend'de de düzenleme yapmamız gerekiyor.
        //neden category'e yazmıyoruz da product'a yazıyoruz çünkü {path:"products/category/:categoryId" , component:ProductComponent}, burda parametre product.
        [HttpGet("getbycategory")]
        public IActionResult GetByCategory(int categoryId)
        {
            //GetAllByCategoryId = bunu oluşmadıysak zaten kızacağı için ampulden create method deyip IProductsManager'a gidip onu IDataResult şeklinde düzelttikten sonra ProductManager'da implemente ediyoruz.
            var result = _productsService.GetAllByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //Bu httpPost postman'de get yerine post'u seçiyoruz. Bu işlem Dataya veri göndericez.
        //Postman'de Post seçtikten sonra Body'yi , raw'ı ve Json'ı seçip veriyi id'siz şekilde ekliyoruz.
        //sağlama yapmak için Get'e gelip Send yaparsak en aşağıda girdiğimiz veri gözükecektir.
        //silme (delete) ve güncelleme (update) içinde httpPost kullanabiliriz.
        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productsService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
