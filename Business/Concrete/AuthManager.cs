using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private ITokenHelper _tokenHelper;
        private IAuthsDal _authsDal;
        private IStudentService _studentService;
        public AuthManager(ITokenHelper tokenHelper, IAuthsDal authsDal, IStudentService studentService)
        {
            _tokenHelper = tokenHelper;
            _authsDal = authsDal;
            _studentService = studentService;
        }

        public IDataResult<SystemUser> Login(UserForLoginDto userForLoginDto)
        {
             var check=_authsDal.Login(userForLoginDto);
            if (check==null)
            {
                return new ErrorDataResult<SystemUser>("Yanlış deneme");
            }

            
            return new SuccessDataResult<SystemUser>(check);

        }

        public IDataResult<SystemUser> Register(SystemUser userForRegisterDto)
        {
            throw new NotImplementedException();
        }
        public IDataResult<AccessToken> CreateAccessToken(SystemUser user)
        {
            
            var accessToken = _tokenHelper.CreateToken(user, null);
            return new SuccessDataResult<AccessToken>(accessToken);
        }

        public IResult PasswordRefresh(string email, string fav)
        {
            var datas= _authsDal.GetAll();
            var data = datas.FirstOrDefault(u => u.Mail == email && u.Fav == fav);
            if (data==null)
            {
                return new ErrorResult("Bilgileriniz yanlış");
            }
            Random rd = new Random();
            string world = "ASDL7.JQXKJ123ULASNAC4589VW";
            string newPassword = "";
            for (int i = 0; i < 6; i++)
            {
                newPassword += world[rd.Next(world.Length)];
            }
            bool isTrue=_authsDal.PasswordRefresh(email, newPassword);
            if (isTrue)
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("mvccodeeps@gmail.com");
                message.To.Add(email);
                message.Subject = "Şifre Değişimi";
                message.Body = "Yeni şifreniz:" + newPassword;
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("mvccodeeps@gmail.com", "MvcCodeeps8066");
                client.Port = 587;

                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Send(message);
                return new SuccessResult("Şifre değiştirildi");
            }
            return new SuccessResult("Şifre değiştirildi");
            
        }

        public IDataResult<List<SystemUser>> GetAll()
        {
            var result = _authsDal.GetAll();
            return new SuccessDataResult<List<SystemUser>>(result);
        }

        public IResult Add(SystemUser systemUser)
        {
            var users= _authsDal.GetAll();
            var user = users.FirstOrDefault(u => u.Id == systemUser.Id);
            if (user != null)
            {
                return new ErrorResult("Belirttiğiniz TC daha önce kullanılmıştır");
            }
            _authsDal.Add(systemUser);
            return new SuccessResult();
        }

        public IResult Delete(string id)
        {
            _authsDal.Delete(id);
            return new SuccessResult("Silindi");
        }

        public IResult Update(SystemUser systemUser)
        {
            _authsDal.Update(systemUser);
            return new SuccessResult("Güncellendi");
        }
    }
}
