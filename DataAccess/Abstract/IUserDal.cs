using Core.DataAccess;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        //neden bir tane operasyon koyduk çünkü join atıcaz.Kullanıcıların operationClaim'lerini çekicez.
        List<OperationClaim> GetClaims(User user);
    }
}
