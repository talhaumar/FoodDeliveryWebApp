using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FoodDelivery.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
            }

            else
            {
                HttpCookie myCookie = Request.Cookies["info"];
                if (myCookie != null)
                {
                    User user = new CustomerDAC().SelectByPhonePassword(myCookie.Values["PHONE_NUMBER"], myCookie.Values["PASSWORD"]);

                    if (user != null)
                    {
                        Session.Add("CURRENT_USER", user);
                        user = Session["CURRENT_USER"] as User;
                        ViewBag.userName = user.Name;
                        if (user.RoleId == 2)
                        {
                            return RedirectToAction("ManageRestaurant", "Restaurant");
                        }
                    }
                }
            } 

            return View();
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
            return RedirectToAction("Index");
        }
        public static double GetDrivingDistanceInMiles(string origin, string destination)
        {
            string url = @"http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" +
              origin + "&destinations=" + destination +
              "&mode=driving&sensor=false&language=en-EN&units=imperial";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);


            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                return Convert.ToDouble(distance[0].ChildNodes[1].InnerText.Replace(" mi", ""));
            }

            return 0;
        }
        public ActionResult RestaurantsSearchBar()
        {
            String area = Request.Form["searchArea"];
            Session["Customer_Area"] = area;

            return RedirectToAction("Restaurants");
        }

        public ActionResult Restaurants()
        {
            String area = (String)Session["Customer_Area"];
            List<double> distances = new List<double>();
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
            }

            List<Restaurant> tempList = new List<Restaurant>();

            RestaurantDAC dac = new RestaurantDAC();
            tempList = dac.SelectAll();
            List<RestaurantType> typelist = new List<RestaurantType>();
            RestaurantTypeDAC tdac = new RestaurantTypeDAC();
            typelist = tdac.SelectAll();
            String userarea = (String)Session["Customer_Area"];
            foreach (Restaurant r in tempList)
            {
                double dis = GetDrivingDistanceInMiles(userarea, r.Area);
                dis = dis * 1.6;
                distances.Add(dis);
            }

            ViewBag.types = typelist;
            ViewBag.Restaurants = tempList;
            ViewBag.distanceList = distances;



            return View();
        }
        public ActionResult AboutUs()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
            }
            return View();
        }

        public ActionResult RestaurantsByType(int id)
        {
            String area = (String)Session["Customer_Area"];
            List<double> distances = new List<double>();
            List<Restaurant> tempList = new List<Restaurant>();


            List<RestaurantType> resIdList = new RestaurantTypeDAC().SelectResIdsByTypeId(id);
            if (resIdList != null)
            {
                foreach (RestaurantType m in resIdList)
                {
                    Restaurant r = new RestaurantDAC().SelectById(m.Id);
                    tempList.Add(r);
                }
            }

            foreach (Restaurant r in tempList)
            {
                double dis = GetDrivingDistanceInMiles(area, r.Area);
                dis = dis * 1.6;
                distances.Add(dis);
            }

            ViewBag.Restaurants = tempList;
            ViewBag.distanceList = distances;


            return View();
        }
        

        [HttpPost]
        public ActionResult InsertCustomer(SignupModel model)
        {
            if(ModelState.IsValid)
            {
                model.RoleId = 1;
                int userid = new CustomerDAC().Insert(model);
                User user = new CustomerDAC().SelectById(userid);
                Session.Add("CURRENT_USER", user);
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string ControllerName, string ActionName, string QueryString)
        {
            string message = "";
            User user = new User();
            int a = 0;
            double d = 0.0;

            if (QueryString != null)
            {
                string s1 = HttpUtility.ParseQueryString(QueryString).Get("a");
                string s2 = HttpUtility.ParseQueryString(QueryString).Get("d");
                a = Convert.ToInt32(s1);
                d = Convert.ToDouble(s2);
            } 
                user = new CustomerDAC().SelectByPhonePassword(model.Phone, model.Password);
                if (user != null)
                {
                    Session.Add("CURRENT_USER", user);
                    if (model.Rememberme)
                    {
                        HttpCookie cookie = new HttpCookie("info");
                        cookie.Expires = DateTime.Today.AddDays(7);
                        cookie.Values.Add("PHONE_NUMBER", user.Phone);
                        cookie.Values.Add("PASSWORD", user.Password);
                        Response.SetCookie(cookie);
                    }
                    if (user.RoleId == 1)
                    {
                        if (ActionName == "RestaurantDisplay")
                        {
                            return Json(new { link = Url.Action(ActionName, ControllerName, new { a , d }) });
                        }
                        return Json(new { link = Url.Action(ActionName, ControllerName) });

                    }
                    else
                    {
                        return Json(new {link = Url.Action("ManageRestaurant", "Restaurant") });
                    }

                }
                else
                {
                    message = "Phone Or Password is Incorrect";
                    return Json(new { message });
                }
        }

        public JsonResult SearchRestaurant(string term)
        {
            List<Restaurant> resList = new RestaurantDAC().SelectRestaurantByTerm(term);
            List<string> rlistdisplay = new List<string>();
            if (resList != null)
            {
                foreach (Restaurant r in resList)
                {
                    rlistdisplay.Add(r.Name);
                }
            }
            return Json(rlistdisplay, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetSearchRestaurant(string term)
        {
            String area = (String)Session["Customer_Area"];
            List<double> distances = new List<double>();

            List<Restaurant> tempList = new List<Restaurant>();

            RestaurantDAC dac = new RestaurantDAC();
            tempList = dac.SelectRestaurantByName(term);
            String userarea = (String)Session["Customer_Area"];
            foreach (Restaurant r in tempList)
            {
                double dis = GetDrivingDistanceInMiles(userarea, r.Area);
                dis = dis * 1.6;
                distances.Add(dis);
            }

            ViewBag.Restaurants = tempList;
            ViewBag.distanceList = distances;
            return PartialView();
        }

        public ActionResult MyOrders()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
                List<Order> cusOrderList = new OrderDAC().SelectByCustomerId(user.Id);
                ViewBag.MyOrders = cusOrderList;
            }
            else 
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        public ActionResult CancelOrder(int orderid)
        {
            Order o = new Order();
            o.Id = orderid;
            o.OrderStatusId = 4;
            new OrderDAC().UpdateOrderStatus(o);
            return RedirectToAction("MyOrders");
        }

        public ActionResult OrderDetails(int orderid)
        {
            List<CustomerOrderViewModel> covm = new List<CustomerOrderViewModel>();
            List<OrderDetails> odlist = new OrderDetailsDAC().SelectAllOrderDetails(orderid);
            foreach (OrderDetails od in odlist)
            {
                CustomerOrderViewModel odvm = new CustomerOrderViewModel();
                odvm.orderdetails = od;
                odvm.fooditem = new FoodItemDAC().SelectById(od.FoodItemId);
                covm.Add(odvm);
            }
            covm.TrimExcess();
            ViewBag.OrderDetailsList = covm;

            CustomerOrderViewModel obj = new CustomerOrderViewModel();
            obj.payment = new PaymentDAC().SelectByOrderId(orderid);

            return View(obj);
        }

        public ActionResult RiderMap(int riderid)
        {
            ViewBag.RiderID = riderid;
            return View();
        }

        public async Task<ActionResult> ForgotPassword(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new System.Net.Mail.MailMessage();
                message.To.Add(new MailAddress(model.Email));
                message.From = new MailAddress("lazylahore@gmail.com");
                message.Subject = "Recover Password";
                message.Body = string.Format(body, "LazyLahore", "lazylahore@gmail.com", "Open this link to recover your password" + "@http://localhost:50098/Home/ResetPassword");
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "lazylahore@gmail.com",
                        Password = "lazylahore123"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult PasswordUpdate(ResetPasswordModel model)
        {
            new CustomerDAC().PasswordUpdate(model);
            return RedirectToAction("Index", "Home");
        }

        public JsonResult doPhoneNumberExist(ResetPasswordModel model)
        {

            List<User> users = new CustomerDAC().SelectAllCustomers(1);
            if (users != null)
            {
                foreach (User u in users)
                {
                    if (u.Phone == model.Phone)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult isPhoneNumberAvailable(SignupModel model)
        {
            
            List<User> users = new CustomerDAC().SelectAllCustomers(1);
            if (users != null)
            {
                foreach (User u in users)
                {
                    if (u.Phone == model.Phone)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            } 
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}