using System;
using System.Collections.Generic;
using FoodDelivery.WebApp.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodDelivery.WebApp.DAC;

namespace FoodDelivery.WebApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult ManageAdmin()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
                if(user.RoleId == 2)
                {
                    User a = new User();
                    List<User> ulist = new AdminDAC().SelectAdminbyRoleId(2);
                    ViewBag.adminlist = ulist;
                    a.SecurityQuestion = "Name your favorite athlete ?";
                    return View(a);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }            
        }

        [HttpPost]
        public ActionResult InsertAdmin(User a)
        {
            if(ModelState.IsValid)
            {
                a.RoleId = 2;
                new AdminDAC().Insert(a);
            }
            else
            {
                return View("ManageAdmin", a);
            }
            return RedirectToAction("ManageAdmin");
        }

        public ActionResult EditAdmin(int id)
        {
            User a = new AdminDAC().SelectById(id,2);
            return View(a);
        }

        [HttpPost]
        public ActionResult UpdateAdmin(User a)
        {
            if (ModelState.IsValid)
            {
                new AdminDAC().Update(a);
            }
            else
            {
                return View("ManageAdmin", a);
            }
            
            return RedirectToAction("ManageAdmin");
        }

        public ActionResult ViewAdmin(int id)
        {
            User u = new CustomerDAC().SelectById(id);
            return View(u);
        }

        public ActionResult DeleteAdmin(int id)
        {
            new AdminDAC().Delete(id);
            return RedirectToAction("ManageAdmin");
        }
    }
}