using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Controllers
{
    public class RestaurantManagerController : Controller
    {
        // GET: RestaurantManager
        public ActionResult RestaurantManager()
        {
            if (Session["CURRENT_MANAGER"] != null)
            {
                Restaurant res = Session["CURRENT_MANAGER"] as Restaurant;
                ViewBag.resName = res.Name;
                List<Order> orderList = new OrderDAC().SelectByResturantId(res.Id);
                ViewBag.OrderList = orderList;
            }
            else
            {
                HttpCookie myCookie = Request.Cookies["info"];
                if (myCookie != null)
                {
                    Restaurant res = new RestaurantDAC().SelectByPhonePassword(myCookie.Values["PHONE_NUMBER"], myCookie.Values["PASSWORD"]);

                    if (res != null)
                    {
                        Session.Add("CURRENT_MANAGER", res);
                        res = Session["CURRENT_MANAGER"] as Restaurant;
                        ViewBag.userName = res.Name;
                        return RedirectToAction("RestaurantManager");
                    }
                }
            } 
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            string message = "";
            Restaurant res = new Restaurant();

            res = new RestaurantDAC().SelectByPhonePassword(model.Phone, model.Password);
            if (res != null)
            {
                Session.Add("CURRENT_MANAGER", res);
                if (model.Rememberme)
                {
                    HttpCookie cookie = new HttpCookie("info");
                    cookie.Expires = DateTime.Today.AddDays(7);
                    cookie.Values.Add("PHONE_NUMBER", res.Phone);
                    cookie.Values.Add("PASSWORD", res.Password);
                    Response.SetCookie(cookie);
                }

                return Json(new { link = Url.Action("RestaurantManager", "RestaurantManager") });
            }
            else
            {
                message = "Phone Or Password is Incorrect";
                return Json(new { message });
            }

        }

        public ActionResult OrderDetails(int oid)
        {

            List<CustomerOrderViewModel> covm = new List<CustomerOrderViewModel>();
            List<OrderDetails> odlist = new OrderDetailsDAC().SelectAllOrderDetails(oid);
            foreach (OrderDetails od in odlist)
            {
                CustomerOrderViewModel odvm = new CustomerOrderViewModel();
                odvm.orderdetails = od;
                odvm.fooditem = new FoodItemDAC().SelectById(od.FoodItemId);
                covm.Add(odvm);
            }
            covm.TrimExcess();
            ViewBag.orderId = oid;
            ViewBag.OrderDetailsList = covm;

            return View();
        }

        [HttpPost]
        public ActionResult SetCookTime(Order model)
        {
            new RestaurantDAC().SetCookTime(model.Id, model.CookTime);
            return RedirectToAction("RestaurantManager");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            HttpCookie myCookie = Request.Cookies["info"];
            if (myCookie != null)
            {
                myCookie.Expires = DateTime.Now;
                Response.SetCookie(myCookie);
            }
            return RedirectToAction("RestaurantManager");
        }
    }

}