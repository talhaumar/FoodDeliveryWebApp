using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult ManageOrder()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
                if(user.RoleId == 2)
                {
                    List<Order> olist = new OrderDAC().SelectAllOrders();
                    ViewBag.orderList = olist;
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

        [HttpGet]
        public ActionResult ViewOrder(int oid, int osid, int uid, int rid)
        {
            OrderViewModel ovm = new OrderViewModel();
            ovm.order = new OrderDAC().SelectById(oid);

            List<Rider> rlist = new RiderDAC().SelectAll();
            List<OrderViewModel> ovmList = new List<OrderViewModel>();
            foreach (Rider r in rlist)
            {
                OrderViewModel om = new OrderViewModel();
                om.rider = r;
                List<Area> riderAreaIds = new AreaDAC().SelectRiderAreaIdsByRiderId(r.Id);
                foreach (Area a in riderAreaIds)
                {
                    Area area = new AreaDAC().SelectById(a.Id);
                    a.Name = area.Name;
                }
                om.rAreas = riderAreaIds;
                ovmList.Add(om);
            }
            ovmList.TrimExcess();
            ViewBag.RiderInfo = ovmList;

            List<SelectListItem> tempList3 = new List<SelectListItem>();
            tempList3.Add(new SelectListItem { Text = "UnPaid", Value = Convert.ToString(0) });
            tempList3.Add(new SelectListItem { Text = "Paid", Value = Convert.ToString(1) });
            ViewBag.PaymentStatus = tempList3;

            Restaurant res = new RestaurantDAC().SelectById(rid);
            ViewBag.ResName = res.Name;

            User u = new CustomerDAC().SelectById(uid);
            List<Order> vivalist = new OrderDAC().SelectByPhone(u.Phone,1);
            int count = 0;
            if (vivalist != null)
            {
                foreach (Order item in vivalist)
                {
                    ++count;
                }
            }
            
            if(count > 0)
            {
                ViewBag.flag = true;
                ViewBag.Star = "**";
            }
            ViewBag.uName = u.Name;


            Payment p = new PaymentDAC().SelectByOrderId(oid);
            ovm.payment = p;

            return View(ovm);
        }

        public ActionResult AssignRider(int orderid, int riderid)
        {
            Order o = new Order();
            o.Id = orderid;
            o.RiderId = riderid;
            new OrderDAC().UpdateOrderRider(o);
            o.OrderStatusId = 2;
            new OrderDAC().UpdateOrderStatus(o);
            Order order = new OrderDAC().SelectById(orderid);

            return RedirectToAction("ViewOrder", new { oid = order.Id, osid = order.OrderStatusId, uid = order.UserId, rid = order.ResId });
        }

        public ActionResult UpdatePaymentStatus(OrderViewModel model, int oid)
        {
            Payment p = new Payment();
            p.PaymentStatus = model.payment.SelectPaymentStatusId;
            p.OrderId = oid;
            new PaymentDAC().UpdatePaymentStatus(p);
            Order order = new OrderDAC().SelectById(oid);

            return RedirectToAction("ViewOrder", new { oid = order.Id, osid = order.OrderStatusId, uid = order.UserId, rid = order.ResId });
        }

        public ActionResult DeleteOrder(int id)
        {
            new OrderDAC().DeleteOrder(id);
            return RedirectToAction("ManageOrder");
        }

    }
}