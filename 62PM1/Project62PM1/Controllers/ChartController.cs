using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Project62PM1.Models;
namespace Project62PM1.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }
        DatabaseDataContext db = new DatabaseDataContext();
        [HttpPost]
        public string  GetSeriChart()
        {
            // lấy ra dữ liệu : 
            var qr_chart = db.tbl_charts;

            if (qr_chart.Any())
            {
                return JsonConvert.SerializeObject(qr_chart.ToList());
            }

            return "-1";
       
        }
        [HttpPost]
        public string ThemMoiDanhMuc()
        {
            // lấy dữ liệu
            try
            {
                string ngay = Request["txt_ngay"];
                int giatri = int.Parse(Request["txt_giatri"].ToString());
                tbl_chart item = new tbl_chart();
                item.GiaTri = giatri;
                item.Name = ngay;
                db.tbl_charts.InsertOnSubmit(item);
                db.SubmitChanges();
                return "Thêm Thành Công";

            }
            catch (Exception ex)
            {
                return "Lỗi : " + ex.Message;
               
            }
                
           
    
           

         
        }
    }
}