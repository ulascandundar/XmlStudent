using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IAuthsDal
    {
        public void Add(SystemUser user);
        public SystemUser Login(UserForLoginDto userForLoginDto);
        public List<SystemUser> GetAll();
        public bool PasswordRefresh(string email, string password);
        public void Delete(string id);
        public void Update(SystemUser systemUser);
    }
}
