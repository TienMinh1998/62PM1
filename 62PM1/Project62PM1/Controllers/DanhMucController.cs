using Newtonsoft.Json;
using Project62PM1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project62PM1.Controllers
{
    public class DanhMucController : Controller
    {
        DatabaseDataContext db = new DatabaseDataContext();
        // GET: DanhMuc
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetListDanhMuc()
        {
            string _crrpage = Request["txt_currpage"];
            string _rds = Request["txt_rdonpage"];
            
            int Intcurrpage = 1;
            int rd_onpage = int.Parse(_rds);
            if (!string.IsNullOrEmpty(_crrpage))
            {
                Intcurrpage = int.Parse(_crrpage);
            }
            var qr_dm = db.tbl_DanhMucs.Where(o => o.isDelete == null || o.isDelete == 0).Skip((Intcurrpage-1)*rd_onpage).Take(rd_onpage);

            if (qr_dm.Any())
            {

                #region Trường hợp cần truyền dữ liệu mở rộng tổng quát (Sử dụng khi cần truyền thêm nhiều dữ liệu mở rộng)
                //đây là trường hợp lấy được danh sách danh mục

                foreach (var i in qr_dm)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    if (i.Id_Cha == null || i.Id_Cha == -1)
                    {
                        //trường hợp danh mục này là danh mục cao nhất
                        dic.Add("TieuDeDanhMucCha", "");
                    }
                    else
                    {
                        var qr_dm_con = qr_dm.Where(o => o.Id == i.Id_Cha);
                        if (qr_dm_con.Any())
                        {
                            //trường hợp tồn tại danh mục cha của danh mục đang xét
                            dic.Add("TieuDeDanhMucCha", qr_dm_con.SingleOrDefault().TieuDe);
                        }
                        else
                        {
                            //trường hợp không tồn tại danh mục cha
                            dic.Add("TieuDeDanhMucCha", "KHÔNG tồn tại danh mục cha của danh mục hiện tại (Mã danh mục = " + i.Id_Cha + ")");
                        }
                    }

                    dic.Add("TruongDuLieuMoRong02", "Giá trị của trường dữ liệu mở rộng thứ 2");

                    i.ExtentData = JsonConvert.SerializeObject(dic);
                }
                #endregion

                return JsonConvert.SerializeObject(qr_dm.ToList());

            }
            else
            {
                //đây là trường hợp danh sách danh mục trả về rỗng
                return "-1";
            }
        }
        [HttpPost]
        public string GetListDanhMucDaXoa()
        {
            var qr_dm = db.tbl_DanhMucs.Where(o => o.isDelete == 1);

            if (qr_dm.Any())
            {
                

                #region Trường hợp cần truyền dữ liệu mở rộng tổng quát (Sử dụng khi cần truyền thêm nhiều dữ liệu mở rộng)
                //đây là trường hợp lấy được danh sách danh mục

                foreach (var i in qr_dm)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    if (i.Id_Cha == null || i.Id_Cha == -1)
                    {
                        //trường hợp danh mục này là danh mục cao nhất
                        dic.Add("TieuDeDanhMucCha", "");
                    }
                    else
                    {
                        var qr_dm_con = qr_dm.Where(o => o.Id == i.Id_Cha);
                        if (qr_dm_con.Any())
                        {
                            //trường hợp tồn tại danh mục cha của danh mục đang xét
                            dic.Add("TieuDeDanhMucCha", qr_dm_con.SingleOrDefault().TieuDe);
                        }
                        else
                        {
                            //trường hợp không tồn tại danh mục cha
                            dic.Add("TieuDeDanhMucCha", "KHÔNG tồn tại danh mục cha của danh mục hiện tại (Mã danh mục = " + i.Id_Cha + ")");
                        }
                    }

                    dic.Add("TruongDuLieuMoRong02", "Giá trị của trường dữ liệu mở rộng thứ 2");

                    i.ExtentData = JsonConvert.SerializeObject(dic);
                }
                #endregion

                return JsonConvert.SerializeObject(qr_dm.ToList());

            }
            else
            {
                //đây là trường hợp danh sách danh mục trả về rỗng
                return "-1";
            }
        }


        public ActionResult Them()
        {
            return View();
        }
        
        public ActionResult ViewDanhMucDaXoa()
        {
            return View();
        }
        public ActionResult ChinhSua()
        {
            return View();
        }

        [HttpPost]
        public string ThemMoiDanhMuc() //Hàm xử lý việc thêm mới danh mục
        {
            //lấy toàn bộ dữ liệu từ form đẩy lên
            string tieude = Request["txt_TieuDe"];
            string url = Request["txt_URL"];
            string idDMCha = Request["cbx_Id_DMCha"];
            string mota = Request["txt_MoTa"];
            string gioithieu = Request["txt_GioiThieu"];
            string ThuTuTrenMainMenu = Request["txt_MainMenuOrder"];
            string ThuTuTrenHeadMenu = Request["txt_MainHeadOrder"];
            string ShowOnMainMenu = Request["chx_isShowMainMenu"]; //== on nếu checked
            string ShowOnHeadMenu = Request["chx_isShowHeadMenu"]; //== on nếu checked

            if (tieude == "" || url == "")
            {
                return "Vui lòng điền vào toàn bộ các trường dữ liệu bắt buộc có đánh dấu * màu đỏ";
            }
            else
            {
                try
                {
                    //code insert data to db
                    //khởi tạo đối tượng danh mục mới
                    var new_dm = new tbl_DanhMuc();
                    new_dm.TieuDe = tieude;
                    new_dm.URL = url;
                    new_dm.Id_Cha = long.Parse(idDMCha);
                    new_dm.MoTa = mota;
                    new_dm.GioiThieu = gioithieu;
                    new_dm.MainMenu_Order = int.Parse(ThuTuTrenMainMenu);
                    new_dm.HeadMenu_Order = int.Parse(ThuTuTrenHeadMenu);
                    if (ShowOnMainMenu == "on")
                    {
                        new_dm.is_ShowMainMenu = 1;
                    }

                    if (ShowOnHeadMenu == "on")
                    {
                        new_dm.is_ShowHeadeMenu = 1;
                    }

                    new_dm.Create_at = DateTime.Now; //cách 1 lấy thời gian local của server
                    new_dm.Create_at = Shareds.SharedFunct.GetServerTime(); //cách 2 lấy thời gian trực tiếp trên server time internet

                    db.tbl_DanhMucs.InsertOnSubmit(new_dm);
                    db.SubmitChanges();

                    return "Thêm mới danh mục có tiêu đề: \"" + tieude + "\" thành công";
                }
                catch (Exception ex)
                {
                    return "Có lỗi xảy ra trong quá trình thêm mới danh mục có tiêu đề: \"" + tieude + "\"";
                }

            }
        }
        
        [HttpPost]
        public string ChinhSuaDanhMuc() //Hàm xử lý việc chỉnh sửa danh mục
        {
            //lấy toàn bộ dữ liệu từ form đẩy lên
            string id_dm = Request["txt_id"];
            string tieude = Request["txt_TieuDe"];
            string url = Request["txt_URL"];
            string idDMCha = Request["cbx_Id_DMCha"];
            string mota = Request["txt_MoTa"];
            string gioithieu = Request["txt_GioiThieu"];
            string ThuTuTrenMainMenu = Request["txt_MainMenuOrder"];
            string ThuTuTrenHeadMenu = Request["txt_MainHeadOrder"];
            string ShowOnMainMenu = Request["chx_isShowMainMenu"]; //== on nếu checked
            string ShowOnHeadMenu = Request["chx_isShowHeadMenu"]; //== on nếu checked

            if (tieude == "" || url == "")
            {
                return "Vui lòng điền vào toàn bộ các trường dữ liệu bắt buộc có đánh dấu * màu đỏ";
            }
            else
            {
                //try
                //{
                //code update data to db
                //lấy về đối tượng danh mục đang được chỉnh sửa
                var qr_dm = db.tbl_DanhMucs.Where(o => o.Id == long.Parse(id_dm));
                if (qr_dm.Any())
                {
                    //trường hợp tồn tại danh mục cần chỉnh sửa
                    var edit_dm = qr_dm.SingleOrDefault();
                    edit_dm.Id = int.Parse(id_dm);
                    edit_dm.TieuDe = tieude;
                    edit_dm.URL = url;
                    edit_dm.Id_Cha = long.Parse(idDMCha);
                    edit_dm.MoTa = mota;
                    edit_dm.GioiThieu = gioithieu;
                    edit_dm.MainMenu_Order = int.Parse(ThuTuTrenMainMenu);
                    edit_dm.HeadMenu_Order = int.Parse(ThuTuTrenHeadMenu);
                    if (ShowOnMainMenu == "on")
                        edit_dm.is_ShowMainMenu = 1;
                    else edit_dm.is_ShowMainMenu = 0;


                    if (ShowOnHeadMenu == "on")
                        edit_dm.is_ShowHeadeMenu = 1;
                    else edit_dm.is_ShowHeadeMenu = 0;

                    edit_dm.LastEdit_at = DateTime.Now; //cách 1 lấy thời gian local của server
                    edit_dm.LastEdit_at = Shareds.SharedFunct.GetServerTime(); //cách 2 lấy thời gian trực tiếp trên server time internet

                    db.SubmitChanges();

                    return "Chỉnh sửa danh mục có tiêu đề: \"" + tieude + "\" thành công";
                }
                else
                {
                    return "KHÔNG tìm thấy danh mục cần chỉnh sửa";
                }

                //}
                //catch (Exception ex)
                //{
                //    return "Có lỗi xảy ra trong quá trình chỉnh sửa danh mục có tiêu đề: \"" + tieude + "\"";
                //}

            }
        }

        [HttpPost]
        public string GetDMInfo()
        {
            var id_dm = Request["id_dm"];
            //request db để lấy ra danh mục có id tương ứng
            try
            {
                var qr = db.tbl_DanhMucs.Where(o => o.Id == long.Parse(id_dm) && (o.isDelete == 1 || o.isDelete == null||o.isDelete==0));
                if (qr.Any())
                {
                    //nếu tồn tại danh mục
                    return JsonConvert.SerializeObject(qr.SingleOrDefault());
                }
                else
                {
                    return "Lỗi - KHÔNG tồn tại danh mục cần sửa";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi - Xảy ra lỗi trong quá trình tìm kiếm danh mục cần sửa";
            }
        }
        [HttpPost]
        public string XoaDanhMuc()
        {
            string id_dm = Request["id"];
            var qr_dm = db.tbl_DanhMucs.Where(o => o.Id == long.Parse(id_dm));
            if (qr_dm.Any())
            {
                var delete_dm = qr_dm.SingleOrDefault();
                delete_dm.isDelete = 1;
                delete_dm.Delete_at = DateTime.Now; //cách 1 lấy thời gian local của server
                delete_dm.Delete_at = Shareds.SharedFunct.GetServerTime(); //cách 2 lấy thời gian trực tiếp trên server time internet
                db.SubmitChanges();
                return "Xóa Thành Công!";
            }
            else
            {
                return "KHÔNG tìm thấy danh mục cần chỉnh sửa";
            }
        }

        [HttpPost]
        public string KhoiPhucDanhMuc()
        {
            string id_dm = Request["id"];
            var qr_dm = db.tbl_DanhMucs.Where(o => o.Id == long.Parse(id_dm));
            if (qr_dm.Any())
            {
                var delete_dm = qr_dm.SingleOrDefault();
                delete_dm.isDelete = 0;
                delete_dm.Delete_at = DateTime.Now; //cách 1 lấy thời gian local của server
                delete_dm.Delete_at = Shareds.SharedFunct.GetServerTime(); //cách 2 lấy thời gian trực tiếp trên server time internet
                db.SubmitChanges();
                return "Khôi PHục Thành Công!";
            }
            else
            {
                return "KHÔNG tìm thấy danh mục cần Khôi Phục";
            }
        }
        public ActionResult KhoaHocIndex()
        {
            return View();
        }
        // Khóa học
       public ActionResult QuanLyKhoaHoc()
        {
            return View();
        }
        [HttpPost]
        public string GetlistKhoaHoc()
        {
            var qr_khoahoc = db.tbl_khoahocs.Where(o => o.isdelete == null || o.isdelete == 0);

            if (qr_khoahoc.Any())
            {
                return JsonConvert.SerializeObject(qr_khoahoc.ToList());
            }
             
            return "-1";
        }
        [HttpPost]
        public string ThemMoiKhoaHoc()
        {
            // lấy dữ liệu từ Client : 
            string tenkhoahoc = Request["txt_tenkhoahoc"];
            string links = Request["txt_duongdan"];
            string mota = Request["txt_Mota"];

            tbl_khoahoc kh = new tbl_khoahoc();
            kh.linkskhoahoc = links;
            kh.ngaytao = DateTime.Now;
            kh.name = tenkhoahoc;
            kh.noidung = mota;
            db.tbl_khoahocs.InsertOnSubmit(kh);
            db.SubmitChanges();


            return "THêm mới thành công";
        }

        // End khóa học

        // Nhật kí
        public ActionResult QuanLyNhatKi()
        {
            return View();
        }
        public string GetlistNhatKi()
        {
            var qr_nhatki = db.tbl_nhatkis.Where(o => o.isdelete == null || o.isdelete == 0);

            if (qr_nhatki.Any())
            {
                return JsonConvert.SerializeObject(qr_nhatki.ToList());
            }

            return "-1";
        }
        [HttpPost]
        public string ThemMoiNhatKi()
        {
            // lấy dữ liệu từ Client : 
            string tenkhoahoc = Request["txt_tenkhoahoc"];
            string links = Request["txt_duongdan"];
            string mota = Request["txt_Mota"];

            tbl_nhatki nk = new tbl_nhatki();
            nk.mota = links;
            nk.ngaytao = DateTime.Now;
            nk.name = tenkhoahoc;
            nk.noidung = mota;
            nk.isdone = 1;

            db.tbl_nhatkis.InsertOnSubmit(nk);
            db.SubmitChanges();


            return "THêm mới thành công";
        }

                //edit nhật kí : 
                public ActionResult Chinhsuanhatki()
        {
            return View();
        }
        //end nhật kí
        [HttpPost]
        public string GetNKInfo()
        {
            var id_dm = Request["id_dm"];
            //request db để lấy ra danh mục có id tương ứng
            try
            {
                var qr = db.tbl_nhatkis.Where(o => o.Id == long.Parse(id_dm) );
                if (qr.Any())
                {
                    //nếu tồn tại danh mục
                    return JsonConvert.SerializeObject(qr.SingleOrDefault());
                }
                else
                {
                    return "Lỗi - KHÔNG tồn tại danh mục cần sửa";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi - Xảy ra lỗi trong quá trình tìm kiếm danh mục cần sửa";
            }
        }
    }
}