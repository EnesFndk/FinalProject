using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    //burda uzun uzun list<Category>(Category category) yazmak yerine IEntityReposityory yi base olarak verdik ve product olarak yapılandırdık.
    public interface ICategoryDal:IEntityRepository<Category>
    {
        
    }
}
