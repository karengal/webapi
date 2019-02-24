using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using webapi.Models;
using File = System.IO.File;

namespace webapi.Controllers
{
    public class UsersController : Controller
    {
        string path = String.Empty;
        string imagesPath = String.Empty;

        public UsersController()
        {
            //path of file containing data
            path = ConfigurationManager.AppSettings["UsersFilePath"].ToString();         

        }
        // GET: api/Users - show list of users
        [ResponseType(typeof(IEnumerable<User>))]
        public ActionResult Get()
        {
            var users = CreateUsersArray();
            return View("Users",users);
        }

        private List<User> CreateUsersArray()
        {
            if (!System.IO.File.Exists(path))
            {
                return new List<User>();
            }
            List<User> users = new List<User>();
            string[] lines = System.IO.File.ReadAllLines(path);
            if(lines.Length == 1) {
                return new List<User>();
            };
            for(int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                users.Add(new Models.User() { id = 0, first_name = values[0], last_name = values[1] ,image=values[2]});
            }
            return users;
        }

        private void WriteToCSV(User[] users)
        {            
            if (System.IO.File.Exists(path))
            {
                ConvertToRowsAndWrite(users);
            }
            else //header only written once
            {
                System.IO.File.WriteAllLines(path, new string[] { "First Name, Last Name, Image" });
                ConvertToRowsAndWrite(users);
            }
        }

        private void ConvertToRowsAndWrite(User[] users)
        {
            System.IO.File.AppendAllLines(path, users.Select(u => $"{u.first_name},{u.last_name},{u.image}"));
        }

        
        // POST: api/Users
        public ActionResult Create(string firstNameInput, string lastNameInput, HttpPostedFileBase filePic)
        {
            string pic = string.Empty;
            if (filePic != null)
            {
                pic = System.IO.Path.GetFileName(filePic.FileName);
                string picPath = System.IO.Path.Combine(
                                       Server.MapPath("~/images/profile"), pic);
                // file is uploaded
                filePic.SaveAs(picPath);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    filePic.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            WriteToCSV(new User[] { new User { first_name = firstNameInput, last_name = lastNameInput, image = pic } });
            //refresh the view
            return RedirectToAction("Get");
        }

       
    }
}
