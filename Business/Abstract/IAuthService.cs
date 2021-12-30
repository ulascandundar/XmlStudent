using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<SystemUser> Register(SystemUser userForRegisterDto);
        public IDataResult<SystemUser> Login(UserForLoginDto userForLoginDto);

        public IDataResult<AccessToken> CreateAccessToken(SystemUser user);
        public IResult PasswordRefresh(string email, string fav);
        public IDataResult<List<SystemUser>> GetAll();
        public IResult Add(SystemUser systemUser);
        public IResult Delete(string id);
        public IResult Update(SystemUser systemUser);

    }
}
