using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodDelivery.WebApp.Models;
using FoodDelivery.WebApp.DAC;

namespace FoodDelivery.WebApp.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
            }
            return View();
        }

        [HttpPost]
        public ActionResult insertMessage(Message model)
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                model.UserId = user.Id;
            }
            new MessageDAC().InsertMessage(model);
            return RedirectToAction("Contact");
        }
    }
}