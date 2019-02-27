using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Controllers
{
    public class UserMessageController : Controller
    {
        // GET: UserMessage
        public ActionResult ManageUserMessage()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
                if(user.RoleId == 2)
                {
                    List<Message> mlist = new MessageDAC().SelectAllMessages();
                    ViewBag.messages = mlist;
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

        public ActionResult ViewMessage(int id)
        {
            Message m = new MessageDAC().SelectById(id);
            return View(m);
        }

        public ActionResult DeleteMessage(int id)
        {
            new MessageDAC().DeleteMessage(id);
            return RedirectToAction("ManageUserMessage");
        }
    }
}