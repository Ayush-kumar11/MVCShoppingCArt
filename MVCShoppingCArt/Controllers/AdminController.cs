using MVCShoppingCArt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace MVCShoppingCArt.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        ApplicationDbContext db=new ApplicationDbContext();
        public ActionResult Index()
        {

            return View(db.Products.ToList());
        }
        public ActionResult Delete(Guid id)
        {
            var data = db.Products.Find(id);
            return View(data);
        }

        [HttpPost]
        public ActionResult Delete(Guid id,string x)
        {
            Product data = db.Products.Find(id);
            db.Products.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
           return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file,Product product)
        {
            string filename=Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            string extension=Path.GetExtension(file.FileName);


            string path=Path.Combine(Server.MapPath("~/Content/Images/"), _filename);
            product.PImage = "~/Content/Images/" + _filename;

            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {
                if (file.ContentLength < 10000)
                {
                    db.Products.Add(product);

                    if (db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        ViewBag.msg = "Student Added";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.msg = "File Size should be Less than 1 Mb";
                }
            }
            else
            {
                ViewBag.msg = "Invalid File Type";
            }
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var data=db.Products.Find(id);
            Session["ImgPath"] = data.PImage;
            return View(data);
        }

        public ActionResult Details(Guid id)
        {
            var data = db.Products.Find(id);

            return View(data);
        }




        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file,Product product)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);


                    string path = Path.Combine(Server.MapPath("~/Content/Images/"), _filename);
                    product.PImage = "~/Content/Images/" + _filename;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (file.ContentLength < 10000)
                        {
                            db.Entry(product).State=EntityState.Modified;
                            string OldImgPath = Request.MapPath(Session["ImgPath"].ToString());
                            if (db.SaveChanges() > 0)
                            {
                                file.SaveAs(path);
                                if (System.IO.File.Exists(OldImgPath))
                                {
                                    System.IO.File.Delete(OldImgPath);
                                }
                                ViewBag.msg = "Student Added";
                                TempData["msg"] = "Data Updated";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ViewBag.msg = "File Size should be Less than 1 Mb";
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Invalid File Type";
                    }
                }
            }
            else
            {
                product.PImage = Session["ImgPath"].ToString();
                db.Entry(product).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    TempData["msg"] = "Data Updated";
                    return RedirectToAction("Index");
                }
            }

            return View();
        }


    }
}