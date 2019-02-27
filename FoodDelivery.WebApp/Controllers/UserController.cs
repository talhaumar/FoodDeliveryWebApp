using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult ManageUser()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
                if(user.RoleId == 2)
                {
                    List<User> tempList = new List<User>();
                    CustomerDAC cd = new CustomerDAC();
                    tempList = cd.SelectAllCustomers(1);
                    ViewBag.userList = tempList;
                    return View();
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

        public ActionResult ViewUser(int id)
        {
            User u = new CustomerDAC().SelectById(id);
            return View(u);
        }

        public ActionResult DeleteUser(int id)
        {
            new CustomerDAC().DeleteCustomer(id);
            return RedirectToAction("ManageUser");
        }
    }
}