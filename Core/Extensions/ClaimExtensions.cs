using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    public static class ClaimExtensions
    {
        //Extension methodu yazabilmek için hem class'ın hemde method'un static olması gerekiyor.
        //this ICollection<Claim> claims = bu method (AddEmail) Claim içine eklenecek demek. 
        //Claim extension methodu oluşturuyoruz.böyle oluşturuluyor.
        //"string email" = bu da parametre
        //bu hepsi için geçerli. Claim .Net core 'un base'inde var zaten. 
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        //string[] roles = Bana gönderilen Rolleri
        //  roles.ToList() = listeye çevir
        //ForEach(role => = her rolü tek tek dolaş
        //claims.Add(new Claim(ClaimTypes.Role, role = her bir role git claim ekle
        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}