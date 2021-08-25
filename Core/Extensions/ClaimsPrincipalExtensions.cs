using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    //Bir kişinin claim'lerini ararken .Net bizi uğraştırdığı için bunları yazmamız lazım.
    //json ve token'deki claim'leri okumak için bu kodları yazmak gerekiyor.
    public static class ClaimsPrincipalExtensions
    {
        //ClaimsPrincipal = Bir kişinin claimlerine erişmek için 
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            //claimsPrincipal? = burdaki sadece ? null olabileceğini gösteriyor.
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result;
        }
        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
    }
}
