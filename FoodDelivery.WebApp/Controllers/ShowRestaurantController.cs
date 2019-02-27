using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Controllers
{
    public class ShowRestaurantController : Controller
    {
        // GET: ShowRestaurant

        public ActionResult RestaurantDisplay(int a , double d)
        {
            int deliveryCharges = 200;
            if(d > 10)
            {
                deliveryCharges = Convert.ToInt32(d * 20);
            }
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
                ViewBag.User = user;
            }
            Session["deliveryCharges"] = deliveryCharges;
            Session["Restaurant"] = a;
            Session["Cart"] = null;
            Session["total"] = null;
            Session["subtotal"] = null;
            Restaurant Res = new Restaurant();
            RestaurantDAC get_Res = new RestaurantDAC();
            Res = get_Res.SelectById(a);
            List<Menu> menu = new List<Menu>();
            List<MenuCategory> cat = new List<MenuCategory>();
            MenuCategoryDAC get_cat = new MenuCategoryDAC();
            cat = get_cat.SelectCategoriesByRestaurant(Res.Id);
            if (cat != null)
            {
                foreach (MenuCategory x in cat)
                {
                    Menu c = new Menu();
                    c.Menu_Cat = x;
                    FoodItemDAC list = new FoodItemDAC();
                    c.items = list.SelectAllItems(x.Id);


                    menu.Add(c);

                }
            }
            menu.TrimExcess();

            ViewBag.menu = menu;
            ViewBag.Restaurant = Res;
            ViewBag.distance = d;
            ViewBag.delivery = deliveryCharges;


            return View();
        }

        public ActionResult deleteitem(int id)
        {
            int index = isExisting(id);
            List<item> Cart = (List<item>)Session["cart"];
            Session["subtotal"] = (int)Session["subtotal"] - (Cart[index].quantity * Cart[index].fitem.Price);
            Session["total"] = (int)Session["subtotal"] + (int)Session["deliveryCharges"];
            Cart.RemoveAt(index);
            Session["cart"] = Cart;
            return RedirectToAction("Shopping");
        }

        public ActionResult Shopping()
        {

            return View();
        }

        public ActionResult Checkout(Order model)
        {
            Order order = new Order();
            int oid;

            model.OrderDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt"));
            model.DeliveryCharges = (int)Session["deliveryCharges"];
            model.OrderStatusId = 1;
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                model.UserId = user.Id;
                oid = new OrderDAC().Insert_And_GetID(model);
            }
            else
            {
                oid = new OrderDAC().Insert_GuestOrder_And_GetID(model);
            }
            
            List<item> Cart = (List<item>)Session["cart"];
            foreach (item it in Cart)
            {
                OrderDetails od = new OrderDetails();
                od.FoodItemId = it.fitem.Id;
                od.Quantity = it.quantity;
                od.OrderId = oid;
                new OrderDetailsDAC().Insert(od);
                Payment pay = new Payment();
                pay.PaymentTime = "00:00";
                pay.Amount = (int)Session["total"];
                pay.PaymentStatus = 0;
                pay.OrderId = oid;
                new PaymentDAC().Insert(pay);

            }

            return RedirectToAction("Restaurants", "Home");
        }

        private int isExisting(int id)
        {
            List<item> Cart = (List<item>)Session["cart"];
            for (int i = 0; i < Cart.Count; i++)

                if (Cart[i].fitem.Id == id)

                    return i;

            return -1;


        }

        [HttpGet]
        public ActionResult OrderNow(int id)
        {

            if (Session["cart"] == null)
            {
                List<item> Cart = new List<item>();
                FoodItemDAC get_item = new FoodItemDAC();
                item cart_item = new item();
                cart_item.fitem = get_item.SelectById(id);
                cart_item.quantity = 1;
                Cart.Add(cart_item);
                Session["cart"] = Cart;
                Session["total"] = (int)Session["deliveryCharges"];
                Session["subtotal"] = 0;
                Session["total"] = (int)Session["total"] + cart_item.fitem.Price;
                Session["subtotal"] = (int)Session["subtotal"] + cart_item.fitem.Price;


            }
            else
            {
                List<item> Cart = (List<item>)Session["cart"];
                int index = isExisting(id);
                if (index == -1)
                {
                    FoodItemDAC get_item = new FoodItemDAC();
                    item cart_item = new item();
                    cart_item.fitem = get_item.SelectById(id);
                    cart_item.quantity = 1;
                    Cart.Add(cart_item);
                    Session["cart"] = Cart;
                    Session["total"] = (int)Session["total"] + cart_item.fitem.Price;
                    Session["subtotal"] = (int)Session["subtotal"] + cart_item.fitem.Price;
                }
                else
                {

                    Cart[index].quantity++;
                    Session["total"] = (int)Session["total"] + Cart[index].fitem.Price;
                    Session["subtotal"] = (int)Session["subtotal"] + Cart[index].fitem.Price;
                }
                Session["cart"] = Cart;


            }
            return RedirectToAction("Shopping");
        }
    }
}