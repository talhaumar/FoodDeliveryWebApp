using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult ManageRestaurant()      
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
                if(user.RoleId == 2)
                { 
                    List<Restaurant> restaurants = new RestaurantDAC().SelectAll();
                    List<RestaurantType> rt = new RestaurantTypeDAC().SelectAll();
                    RestaurantModel rm = new RestaurantModel();
                    rm.RestaurantType = rt;
                    rm.restaurantlist = restaurants;
                    rm.restaurant = new Restaurant();
                    return View(rm);
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
        public ActionResult InsertRestaurant(RestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["file"];
                string fname = System.IO.Path.GetFileName(file.FileName);
                string url = "~/images/restaurantimages/" + fname;
                string path = System.IO.Path.Combine(Server.MapPath(url));
                model.restaurant.ImageURL = fname;
                file.SaveAs(path);
                
                
                

                int rid = new RestaurantDAC().Insert_And_GetID(model.restaurant);
                for (int i = 0; i < model.RestaurantType.Count; i++)
                {
                    if (model.RestaurantType[i].IsSelected)
                    {
                        int rtypeid = model.RestaurantType[i].Id;
                        new RestaurantTypeDAC().ResTypeInsert(rid, rtypeid);
                    }
                }
            }
            else
            {
                return View("ManageRestaurant", model);
            }

            return RedirectToAction("ManageRestaurant");
        }

        public ActionResult ManageRestaurantType()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult InsertRestaurantType(RestaurantType model)
        {
            if(ModelState.IsValid)
            {
                new RestaurantTypeDAC().Insert(model);
            }
            else
            {
                return View("ManageRestaurantType",model);
            }
            
            return RedirectToAction("ManageRestaurant");
        }

        [HttpGet]
        public ActionResult EditRestaurant(int id)
        {
            ViewBag.Id = id;
            RestaurantModel rm = new RestaurantModel();
            rm.restaurant = new RestaurantDAC().SelectById(id);
            List<RestaurantType> rTypeIds = new RestaurantTypeDAC().SelectResTypeIdsByResId(id);
            List<RestaurantType> types = new RestaurantTypeDAC().SelectAll();
            foreach (RestaurantType rtype in types)
            {
                rm.RestaurantType.Add(rtype);
            }
            if(types != null)
            { 
                foreach(RestaurantType rt in rTypeIds)
                {
                    foreach(RestaurantType rtype in types)
                    {
                        if(rtype.Id == rt.Id && rtype.IsSelected == false)
                        {
                            rtype.IsSelected = true;
                        }

                    }
                }
                rm.RestaurantType.TrimExcess();
            }   
                return View(rm);
        }

        [HttpPost]
        public ActionResult ImageDelete(int rid, string imgurl)
        {
            new RestaurantDAC().DeleteRestaurantImage(rid);
            string fname = imgurl;
            string url = "~/images/restaurantimages/" + fname;
            string path = System.IO.Path.Combine(Server.MapPath(url));
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return Json(new { flag = true });
        }

        [HttpPost]
        public ActionResult UpdateRestaurant(RestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                if(Request.Files["file"] != null)
                {
                    HttpPostedFileBase file = Request.Files["file"];
                    string fname = System.IO.Path.GetFileName(file.FileName);
                    string url = "~/images/restaurantimages/" + fname;
                    string path = System.IO.Path.Combine(Server.MapPath(url));
                    model.restaurant.ImageURL = fname;
                    file.SaveAs(path);
                }

                new RestaurantDAC().Update(model.restaurant);
                List<RestaurantType> rTypeIds = new RestaurantTypeDAC().SelectResTypeIdsByResId(model.restaurant.Id);
                
                for (int i = 0; i < model.RestaurantType.Count; i++)
                {
                    if (model.RestaurantType[i].IsSelected)
                    {
                        foreach(RestaurantType rtypeId in rTypeIds)
                        {
                            if(model.RestaurantType[i].Id == rtypeId.Id)
                            {
                                break;
                            }
                            else
                            {
                                new RestaurantTypeDAC().ResTypeInsert(model.restaurant.Id, model.RestaurantType[i].Id);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (RestaurantType rtypeId in rTypeIds)
                        {
                            if (model.RestaurantType[i].Id == rtypeId.Id)
                            {
                                RestaurantType rtype_RecordId = new RestaurantTypeDAC().SelectResTypeRecordIdByTypeId(model.RestaurantType[i].Id);
                                new RestaurantTypeDAC().DeleteRes_TypeRecordById(rtype_RecordId.Id);
                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
            }
            else
            {
                return View("ManageRestaurant", model);
            }

            return RedirectToAction("ManageRestaurant");
            
        }

        public ActionResult DeleteRestaurant(int id)
        {
            Restaurant res = new RestaurantDAC().SelectById(id);
            new RestaurantDAC().DeleteRestaurant(id);
            string fname = res.ImageURL;
            string url = "~/Content/images/restaurantimages/" + fname;
            string path = System.IO.Path.Combine(Server.MapPath(url));
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return RedirectToAction("ManageRestaurant");
        }

        [HttpGet]
        public ActionResult ViewMenu(int id)
        {
            Restaurant r = new RestaurantDAC().SelectById(id);
            ViewBag.ResName = r.Name;
            ViewBag.Rid = id;
            List<Menu> menu = new List<Menu>();
            List<MenuCategory> cat = new List<MenuCategory>();
            MenuCategoryDAC get_cat = new MenuCategoryDAC();
            cat = get_cat.SelectCategoriesByRestaurant(id);
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
                menu.TrimExcess();
                ViewBag.menu = menu;
            }
            
            return View();
        }

       
        [HttpGet]
        public ActionResult AddCategory(int id)
        {
              ViewBag.Id = id;
              return View();
        }

        [HttpPost]
        public ActionResult InsertCategory(MenuCategory model)
        {
            int rid = model.ResId;
            if(ModelState.IsValid)
            {

                new MenuCategoryDAC().Insert(model);
            }
            else
            {
                return RedirectToAction("ViewMenu", new { id = rid });
            }

            return RedirectToAction("ViewMenu", new { id = rid });
        }

        [HttpGet]
        public ActionResult EditCategory(int rid, int cid)
        {
            MenuCategory mc = new MenuCategoryDAC().SelectById(cid);
            ViewBag.Id = rid;
            return View(mc);
        }
        
        [HttpPost]
        public ActionResult UpdateCategory(MenuCategory model)
        {
            int rid = model.ResId;
            if (ModelState.IsValid)
                {
                    new MenuCategoryDAC().Update(model);
                }
            else
                {
                    return View("EditCategory", new { rid, model.Id });
                }

            return RedirectToAction("ViewMenu", new { id = rid });
        }

        public ActionResult DeleteMenuCategory(int rid, int cid)
        {
            new MenuCategoryDAC().DeleteMenuCategory(cid);
            return RedirectToAction("ViewMenu", new { id = rid });
        }

        [HttpGet]
        public ActionResult AddFoodItem(int rid)
        {
            List<MenuCategory> categories = new MenuCategoryDAC().SelectCategoriesByRestaurant(rid);
            List<SelectListItem> tempList = null;
            if (categories != null)
            {
                tempList = new List<SelectListItem>();
                foreach (var c in categories)
                {
                    tempList.Add(new SelectListItem { Text = c.Name, Value = Convert.ToString(c.Id) });
                }
                tempList.TrimExcess();
            }
            ViewBag.Categories = tempList;
            ViewBag.ResId = rid;
            return View();
        }

        [HttpPost]
        public ActionResult InsertFoodItem(FoodItem model, int rid)
        {
            if(ModelState.IsValid)
            {
                model.MenuCatId = model.SelectedCategoryId;
                new FoodItemDAC().Insert(model);
            }
            else
            {
                return View("AddFoodItem",model);
            }

            return RedirectToAction("ViewMenu", new { id = rid });
        }

        [HttpGet]
        public ActionResult EditFoodItem(int rid, int fid)
        {
            FoodItem f = new FoodItemDAC().SelectById(fid);
            ViewBag.ResId = rid;
            return View(f);
        }

        [HttpPost]
        public ActionResult UpdateFoodItem(FoodItem model, int rid)
        {
            if (ModelState.IsValid)
            {
                new FoodItemDAC().Update(model);
            }
            else
            {
                return View("EditFoodItem", new { rid, model.Id });
            }

            return RedirectToAction("ViewMenu", new { id = rid });
        }

        public ActionResult DeleteFoodItem(int rid, int fid)
        {
            new FoodItemDAC().DeleteFoodItem(fid);
            return RedirectToAction("ViewMenu", new { id = rid });
        }

        public JsonResult isPhoneNumberAvailable(RestaurantModel model)
        {
            
            List<Restaurant> restaurants = new RestaurantDAC().SelectAll();
            if (restaurants != null)
            {
                foreach (Restaurant m in restaurants)
                {
                    if (m.Phone == model.restaurant.Phone)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}