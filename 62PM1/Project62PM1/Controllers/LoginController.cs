using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project62PM1.Models;
namespace Project62PM1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        DatabaseDataContext db = new DatabaseDataContext();
        public ActionResult Index()
        {
            // Xử Lý Đăng Nhập
            return View();
        }
        [HttpPost]
        public bool LoginSystem()
        {
            string tk = Request["txt_tk"];
            string mk = Request["txt_mk"];

            var q = from o in db.tbl_Users
                    where tk == o.tk && mk == o.mk
                    select o;


            if (q.Any())
            {
                return true;
            }
            return false;
        }

    }
}