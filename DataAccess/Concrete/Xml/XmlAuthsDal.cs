using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataAccess.Concrete.Xml
{
    public class XmlAuthsDal : IAuthsDal
    {
        public void Add(SystemUser user)
        {
            string super = "False";
            if (user.Super)
            {
                super = "True";
            }
            else
            {
                super = "False";
            }
            XDocument xDocument = XDocument.Load(@"XML\Users.xml");
            xDocument.Element("Users").Add(
                new XElement("User",
                new XElement("Id", user.Id),
                new XElement("Name", user.Name),
                new XElement("Mail", user.Mail),
                new XElement("Fav", user.Fav),
                new XElement("Password", user.Password),
                new XElement("Super", super)

                ));
            xDocument.Save(@"XML\Users.xml");
        }



        public SystemUser Login(UserForLoginDto userForLoginDto)
        {
            List<SystemUser> data = new List<SystemUser>();
            XDocument xDocument = XDocument.Load(@"XML\Users.xml");
            List<XElement> data1 = xDocument.Descendants("User").ToList();
            foreach (XElement item in data1)
            {
                SystemUser user = new SystemUser();
                user.Id = item.Element("Id").Value;
                user.Name = item.Element("Name").Value;
                user.Password = item.Element("Password").Value;
                user.Type= item.Element("Type").Value;
                data.Add(user);
            }
            var result = data.FirstOrDefault(u => u.Name == userForLoginDto.Name && u.Password == userForLoginDto.Password);

            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public List<SystemUser> GetAll()
        {
            List<SystemUser> data = new List<SystemUser>();
            XDocument xDocument = XDocument.Load(@"XML\Users.xml");
            List<XElement> data1 = xDocument.Descendants("User").ToList();
            foreach (XElement item in data1)
            {
                bool super = false;
                if (item.Element("Super").Value == "True")
                {
                    super = true;
                }
                SystemUser user = new SystemUser();
                user.Id = item.Element("Id").Value;
                user.Name = item.Element("Name").Value;
                user.Password = item.Element("Password").Value;
                user.Mail = item.Element("Mail").Value;
                user.Fav = item.Element("Fav").Value;
                user.Type = item.Element("Type").Value;
                user.Super = super;
                data.Add(user);
            }
            return data;

        }

        public bool PasswordRefresh(string email, string password)
        {
            XDocument xDocument = XDocument.Load(@"XML\Users.xml");
            XElement node = xDocument.Root.Elements().Where(s => s.Element("Mail").Value == email).FirstOrDefault();
            if (node != null)
            {
                node.SetElementValue("Password", password);

                xDocument.Save(@"XML\Users.xml");

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Delete(string id)
        {
            XDocument xDocument = XDocument.Load(@"XML\Users.xml");
            xDocument.Root.Elements().Where(s => s.Element("Id").Value == id).Remove();
            xDocument.Save(@"XML\Users.xml");

        }

        public void Update(SystemUser systemUser)
        {
            XDocument xDocument = XDocument.Load(@"XML\Users.xml");
            XElement node = xDocument.Root.Elements().Where(s => s.Element("Id").Value == systemUser.Id).FirstOrDefault();
            string super = "False";
            if (systemUser.Super)
            {
                super = "True";
            }
            else
            {
                super = "False";
            }
            if (node != null)
            {
                node.SetElementValue("Id", systemUser.Id);
                node.SetElementValue("Name", systemUser.Name);
                node.SetElementValue("Password", systemUser.Password);
                node.SetElementValue("Mail", systemUser.Mail);
                node.SetElementValue("Fav", systemUser.Fav);
                node.SetElementValue("Super", super);
                xDocument.Save(@"XML\Users.xml");
            }
        }
    }
}
