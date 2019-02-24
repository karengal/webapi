using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webapi.Models;
using System.IO;

namespace webapi.Controllers
{    
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new List<User>());
        }

        public void ExportToCSV()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"First Name\",\"Last Name\",\"Time Zone\",\"Phone\",\"About\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=exportedUsers.csv");
            Response.ContentType = "text/csv";
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            List<User> users = new List<User>();
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);

                //Execute a loop over the rows.
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        users.Add(new User
                        {
                            id = Convert.ToInt32(row.Split(',')[0]),
                            first_name = row.Split(',')[1],
                            last_name = row.Split(',')[2],
                            timezone = row.Split(',')[3],
                            phone = row.Split(',')[4],
                            about = row.Split(',')[5]
                        });
                    }
                }
            }

            return View(users); 
        }
    }

}
