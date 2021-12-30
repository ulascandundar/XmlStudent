using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DataAccess.Concrete.Xml
{
    public class XmlStudentDal:IAuthDal
    {
        public void Add(Student student)
        {
            List<string> exa = student.Exams;
            string examps = "";
            for (int i = 0; i < exa.Count; i++)
            {
                if (i < exa.Count - 1)
                {
                    examps += exa[i] + ",";
                }
                else
                    examps += exa[i];

            }
            string date1 = student.Date.ToString();

            XDocument xDocument = XDocument.Load(@"XML\Students.xml");
            xDocument.Element("Students").Add(
                new XElement("Student",
                new XElement("Id", student.Id),
                new XElement("Name", student.Name),
                new XElement("Surname", student.Surname),
                new XElement("Exams", examps),
                new XElement("Date",date1),
                new XElement("Enlem", ""),
                new XElement("Boylam", "")
                ));
            xDocument.Save(@"XML\Students.xml");

            FileStream fs = File.Create(@"XML\" + student.Id + ".xml");
            fs.Close();

            XmlTextWriter xmlText = new XmlTextWriter(@"XML\"+student.Id+".xml", System.Text.UTF8Encoding.UTF8);
            xmlText.WriteComment("");
            xmlText.WriteStartElement("Students");
            xmlText.WriteStartElement("Student");
            xmlText.WriteElementString("Id", student.Id);
            xmlText.WriteElementString("Name", student.Name);
            xmlText.WriteElementString("Surname", student.Surname);
            xmlText.WriteElementString("Exams", examps);
            xmlText.WriteElementString("Date", DateTime.Now.ToString("MM/dd/yyyy"));
            xmlText.WriteElementString("Hour", DateTime.Now.ToString());
            xmlText.WriteElementString("Enlem", "");
            xmlText.WriteElementString("Boylam", "");
            xmlText.WriteEndElement();
            xmlText.Close();
        }
        //
        public List<Student> GetAll()
        {
            //XmlDocument x = new XmlDocument();
            //DataSet dataSet = new DataSet();
            //XmlReader xmlReader;
            //xmlReader = XmlReader.Create(@"C:\XML\Students.xml", new XmlReaderSettings());
            //dataSet.ReadXml(xmlReader);
            //var result = dataSet.Tables;
            //return result;
            List<Student> data = new List<Student>();
            XDocument xDocument = XDocument.Load(@"XML\Students.xml");
            List<XElement> data1 = xDocument.Descendants("Student").ToList();
            foreach (XElement item in data1)
            {
                Student student = new Student();
                student.Id = item.Element("Id").Value;
                student.Name = item.Element("Name").Value;
                student.Surname = item.Element("Surname").Value;
                student.Exams = item.Element("Exams").Value.Split(",").ToList();
                data.Add(student);
            }
            return data;

        }

        public void Delete(string id)
        {
            XDocument xDocument = XDocument.Load(@"XML\Students.xml");
            xDocument.Root.Elements().Where(s => s.Element("Id").Value == id).Remove();
            xDocument.Save(@"XML\Students.xml");
            if (System.IO.File.Exists(@"XML\" + id + ".xml"))
            {
                System.IO.File.Delete(@"XML\" + id + ".xml");
            }
        }

        public void Update(Student student)
        {
            XDocument xDocument = XDocument.Load(@"XML\Students.xml");
            XElement node = xDocument.Root.Elements().Where(s => s.Element("Id").Value == student.Tc).FirstOrDefault();
            
            if (node != null)
            {
                node.SetElementValue("Id", student.Id);
                node.SetElementValue("Name", student.Name);
                node.SetElementValue("Surname", student.Surname);
                List<string> exa = student.Exams;
                string examps = "";
                for (int i = 0; i < exa.Count; i++)
                {
                    if (i < exa.Count - 1)
                    {
                        examps += exa[i] + ",";
                    }
                    else
                        examps += exa[i];

                }
                string date1 = student.Date.ToString();
                node.SetElementValue("Exams", examps);
                node.SetElementValue("Date", date1);
                xDocument.Save(@"XML\Students.xml");

                XmlTextWriter xmlText = new XmlTextWriter(@"XML\" + student.Id + ".xml", System.Text.UTF8Encoding.UTF8);
                xmlText.WriteComment("");
                xmlText.WriteStartElement("Students");
                xmlText.WriteStartElement("Student");
                xmlText.WriteElementString("Id", student.Id);
                xmlText.WriteElementString("Name", student.Name);
                xmlText.WriteElementString("Surname", student.Surname);
                xmlText.WriteElementString("Exams", examps);
                xmlText.WriteElementString("Date", date1);
                xmlText.WriteElementString("Hour", DateTime.Now.ToString());
                xmlText.WriteEndElement();
                xmlText.Close();
                if (student.Id!=student.Tc)
                {
                    if (System.IO.File.Exists(@"XML\" + student.Tc + ".xml"))
                                    {
                                        System.IO.File.Delete(@"XML\" + student.Tc + ".xml");
                                    }
                }
                
            }
        }
        public void AddAdress(string id,string enlem, string boylam)
        {
            XDocument xDocument = XDocument.Load(@"XML\Students.xml");
            XElement node = xDocument.Root.Elements().Where(s => s.Element("Id").Value == id).FirstOrDefault();
            if (node != null)
            {
                node.SetElementValue("Enlem", enlem);
                node.SetElementValue("Boylam", boylam);
                xDocument.Save(@"XML\Students.xml");
            }
            XDocument xDocument1 = XDocument.Load(@"XML\"+id+".xml");
            XElement node1 = xDocument1.Root.Elements().Where(s => s.Element("Id").Value == id).FirstOrDefault();

            if (node1 != null)
            {
                node1.SetElementValue("Enlem", enlem);
                node1.SetElementValue("Boylam", boylam);
                xDocument1.Save(@"XML\"+id+".xml");
            }
        }
    }
}
