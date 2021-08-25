using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Encryprtion
{
    public class SecurityKeyHelper
    {
        //Bu webapi'deki appsettings.json 'da yazdığımız SecurityKey.
        // oluşturduğumuz SecurityKey string olduğu için burda byte haline getiriyor ve simetrik (anahatar) hale getiriyor. bu yapılar jwt ihtiyaç duyduğu yapılar.
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
