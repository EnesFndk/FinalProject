using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryprtion;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        //Engin hocanın github'ından NetCoreBackEnd'den aldık 
        //IConfiguration = Api'deki appsettings.json'daki değerleri okumamıza yarıyor. Entegre ediyoruz.
        public IConfiguration Configuration { get; }
        //TokenOptions = Configuration ile api'deki appsettings.json'daki değerleri okuduk ya o nesneleri oluşturduğumuz tokenoptions nesnesine atıcak.
        private TokenOptions _tokenOptions;
        //AccessToken ne zaman gerçersizleşicek onu yapıyoruz.
        private DateTime _accessTokenExpiration;

        //Burda IConfiguration enjecte ettik.
        public JwtHelper(IConfiguration configuration)
        {
            //Configuration = appsettings.json 
            Configuration = configuration;
            //benim değerlerim configuration'daki Alanı bul(GetSection ), (GetSection appsetting.json'daki her bir parantez içi örn. loggin, tokenoptions gibi.)
            //ordaki Tokenoptions bölümünü al ve Oluşturduğumuz TokenOptions class'ıyla entegre ediyoruz. 
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        }
        //Kullanıcı için bir Token üretiyoruz burda.
        //User user, List<OperationClaim> operationClaims = bana user ve claim bilgisi ver. ona göre token oluşturayım.
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            //Token ne zaman Expiration olucağını yazıyoruz.
            //Onu nerden alıyoruz "_tokenOptions.AccessTokenExpiration" = appsettings.json'da değeri TokenOptions class'ındaki AccessToken'e entegre ettik ve appsettings.json'da 10 olarak vermiştik ordan alıyor.
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            //Ben bunu oluştururken securityKey'e ihtiyacım var , Bizde yazdığımız SecurityKey'i yazıp onu oluşturabiliyoruz.
            //zaten securitykeyhelper'da yazdığımız symmetric'i burda kullanıyoruz. bir anahtara ihtiyacımız olduğundan yazdık.
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            //Hangi algoritmayı kullanayıp dediğinde Bizim oluşturduğumuz "SigningCredentialsHelper" var onu burayla ilişkilendiriyoruz.
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            //Tokenoptions'ları kullanarak ilgili kullanıcı için (user) ilgili credential'ları kullanarak , buna atanacak yetkileri(claim) içeren bir method yazdık.
            //aşağıda zaten JwtSecurityToken oluşturduk.
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            //Bu kodlarla birlikte elimdeki Token bilgisini yazdıracağım.
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        //Nuget'den system.identityModel.Tokens.jwt ' yi yüklüyoruz ve çözüyoruz. Parametrelere ihtiyacımız var onları ekliyoruz.
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            //Burda JwtSecurityToken Token oluşturuyor. Karşılıklı entegre ediyoruz burda.
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                //notBefore: DateTime.Now = burda önceki zamanda bilgi eklenemez demek
                //Token'ın Expiration bilgisi şuandan önce ise geçerli değil demek.
                notBefore: DateTime.Now,
                //bizden claim türünde bilgiler istediği için "IEnumerable<Claim> SetClaims" oluşturduk .
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }
        //Claim'ler bizim için çok önemli olduğundan onun içinde method yaptık.
        //neden IEnumerable çünkü JwtSecurityToken'da claims'de IEnumerable istiyor.
        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            //Claim , .Net core içinde olduğu için direk çözüyoruz.
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            //Email'i şu şekilde de yazabilirdik ama uzun olduğu için ClaimExtensions oluşturup buraya ekledik.
            //claims.Add(new Claim(JwtRegisteredClaimNames.Email, email)); bu şekilde olacağına aşadağıdaki şekil çok çok iyi.
            claims.AddEmail(user.Email);
            //Başına $ eklersek string'de hani + şeklinde 2 tane string yazabilirdik ya onun yerine $ koyarsak + koymaya gerek duymadan yapabiliriz.
            claims.AddName($"{user.FirstName} {user.LastName}");
            //Roller'de ekleyebiliyoruz. Nasıl ekliyoruz = OperationClaims'deki kullanının veritabanındaki claim'lerinin Namelerini çekip array'e basıp rolleri ekleyebiliyoruz.
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }
    }
}