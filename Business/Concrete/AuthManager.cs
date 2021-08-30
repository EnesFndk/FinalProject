using Business.Absract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        //kullanıcıyı kontrol etmemiz gerekiyor database'de var mı diye o sebeple UserService kullanıyoruz. Daha sonra IUserService'i Initialize ediyoruz.
        private IUserService _userService;
        //kullanıcı login olduğunda ona token vermek için tokenhelper'e de ihtiyaç var. onuda Initialize ediyoruz
        private ITokenHelper _tokenHelper;

        //Initialize dediğimiz bu işlem. Elle de yapabiliriz.
        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }
        //Kayıt olmak için gerekli operasyonları yazıyor.
        //Veritabanına atabilmemiz için passwordHash ve passwordSalt'ı out ettirecek şekilde  password göndererek, CreatePasswordHash oluşturduk ve User nesnesi oluşturup bilgileri ekliyoruz.
        //Bilgiler zaten Entities'deki User'da mevcut olanlar.
        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            //Kayıt olduktan sonra bize şifre gönderiyor. gönderilen şifrenin bize passwordHash ve passwordSalt olarak bize dönmesini bekliyoruz. 
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                //register'e göre yazıyoruz.
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                //kullanıcı aktif mi = status. Mesela ek olarak  kullanıcı aktif değil ise Email'den doğrulama istersiniz doğrulama yapılınca status true yaparsınız. bu tabi şirket işleri.
                Status = true
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            //Mevcut kullanıcının kontrol edilmesine yarayan bir kod aşağıda.
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            //Veritabanından bu e-mail'e sahip kullanıcı bilgisi gelmedi ise UserNotFound mesajı ver.
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }
            //Bu kullanıcının gönderdiği şifreyi daha önce onun için oluşturduğumuz Salt ve Hash ile Database'deki hash ile kullanıcının datasının Hash'i aynı mı onu kontrol edicez.
            //Kullacının gönderdiği şifreyi (Hash ve salt) kontrol eden bir kod yazıcaz.
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }
            //üstteki 2 bilgide doğru ise Başarılı şekilde login olur.
            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }
        //kullanıcı sistemde var mı yok mu onun kontrolü yapılıyor.
        public IResult UserExists(string email)
        {
            //!=null = null'dan farklı ise 
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }
        //Kullanıcının kayıt olduktan sonra token vericez ve işlemler artık token ile yapılacağı anlamına geliyor.
        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            //kullanının claimlerini (rollerini) vericek.
            var claims = _userService.GetClaims(user);
            //Claims'i burda da kullanıyoruz.
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }
    }
}
