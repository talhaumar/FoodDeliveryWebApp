using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Controllers
{
    public class RiderLocationController : Controller
    {
        // GET: RiderLocation
        public ActionResult Index()
        {
            if (Session["CURRENT_RIDER"] != null)
            {
                Rider rider = Session["CURRENT_RIDER"] as Rider;
                ViewBag.riderName = rider.Name;
                List<Order> orderList = new OrderDAC().SelectByRiderId(rider.Id);
                ViewBag.OrderList = orderList;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            string message = "";
            Rider rider = new Rider();

            rider = new RiderDAC().SelectByPhonePassword(model.Phone, model.Password);
            if (rider != null)
            {
                Session.Add("CURRENT_RIDER", rider);
                if (model.Rememberme)
                {
                    HttpCookie cookie = new HttpCookie("info");
                    cookie.Expires = DateTime.Today.AddDays(7);
                    cookie.Values.Add("PHONE_NUMBER", rider.Phone);
                    cookie.Values.Add("PASSWORD", rider.Password);
                    Response.SetCookie(cookie);
                }

                return Json(new { link = Url.Action("Index", "RiderLocation") });
            }
            else
            {
                message = "Phone Or Password is Incorrect";
                return Json(new { message });
            }

        }


        public ActionResult UpdateRiderLocation(Rider model)
        {
            if (Session["CURRENT_RIDER"] != null)
            {
                Rider rider = Session["CURRENT_RIDER"] as Rider;
                Rider r = new Rider();
                r.Id = rider.Id;
                r.Latitude = model.Latitude;
                r.Longitude = model.Longitude;
                new RiderDAC().UpdateriderLatLon(r);
                return Json(new { flag = true });
            }
            else
            {
                return Json(new { flag = false });
            }
        }

        public ActionResult OrderDelivered(int oid)
        {
            Order order = new Order();
            order.Id = oid;
            order.OrderStatusId = 3;
            new OrderDAC().UpdateOrderStatus(order);
            return RedirectToAction("Index");
        }

        //public ActionResult RiderMap(int riderid)
        //{
        //    ViewBag.RiderID = riderid;
        //    return View();
        //}

        public JsonResult GetCords(int RiderID)
        {
            Rider r = new RiderDAC().SelectById(RiderID);
            string lat = r.Latitude;
            string lon = r.Longitude;
            return Json(new { lat, lon }, JsonRequestBehavior.AllowGet);
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
            return RedirectToAction("Index", "RiderLocation");
        }
    }
}