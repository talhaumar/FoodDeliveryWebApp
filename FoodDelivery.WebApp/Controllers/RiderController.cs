using FoodDelivery.WebApp.DAC;
using FoodDelivery.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodDelivery.WebApp.Controllers
{
    public class RiderController : Controller
    {
        // GET: Rider
        public ActionResult ManageRider()
        {
            if (Session["CURRENT_USER"] != null)
            {
                User user = Session["CURRENT_USER"] as User;
                ViewBag.userName = user.Name;
                if(user.RoleId == 2)
                {
                    List<Rider> riders = new RiderDAC().SelectAll();
                    List<Area> alist = new AreaDAC().SelectAll();
                    RiderModel rm = new RiderModel();
                    rm.arealist = alist;
                    rm.riderlist = riders;
                    rm.rider = new Rider();
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
        public ActionResult InsertRider(RiderModel model)
        {
            if (ModelState.IsValid)
            {
                model.rider.RiderStatus = 0;
                int rid = new RiderDAC().Insert_And_GetID(model.rider);
                for (int i = 0; i < model.arealist.Count; i++)
                {
                    if (model.arealist[i].IsSelected)
                    {
                        new AreaDAC().RiderAreaInsert(rid, model.arealist[i].Id);
                    }
                }
            }
            else
            {
                return RedirectToAction("ManageRider",model);

            }
                
            
            return RedirectToAction("ManageRider");
        }

        public ActionResult EditRider(int id)
        {
            RiderModel rm = new RiderModel();
            rm.rider = new RiderDAC().SelectById(id);
            List<Area> riderAreaIds = new AreaDAC().SelectRiderAreaIdsByRiderId(id);
            List<Area> areas = new AreaDAC().SelectAll();
            if (areas != null)
            {
            foreach (Area rArea in areas)
            {
                rm.arealist.Add(rArea);
            }
            
                foreach (Area a in riderAreaIds)
                {
                    foreach (Area rarea in areas)
                    {
                        if (rarea.Id == a.Id && rarea.IsSelected == false)
                        {
                            rarea.IsSelected = true;
                        }

                    }
                }
                rm.arealist.TrimExcess();
            }
            return View(rm);
        }

        [HttpPost]
        public ActionResult UpdateRider(RiderModel model)
        {
            if(ModelState.IsValid)
            {
                new RiderDAC().Update(model.rider);
                List<Area> rAreaIds = new AreaDAC().SelectRiderAreaIdsByRiderId(model.rider.Id);

                for (int i = 0; i < model.arealist.Count; i++)
                {
                    if (model.arealist[i].IsSelected)
                    {
                        foreach (Area areaId in rAreaIds)
                        {
                            if (model.arealist[i].Id == areaId.Id)
                            {
                                break;
                            }
                            else
                            {
                                new AreaDAC().RiderAreaInsert(model.rider.Id, model.arealist[i].Id);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (Area areaId in rAreaIds)
                        {
                            if (model.arealist[i].Id == areaId.Id)
                            {
                                Area rArea_RecordId = new AreaDAC().SelectRiderAreaRecordIdByAreaId(areaId.Id);
                                new AreaDAC().DeleteRider_AreaRecordById(rArea_RecordId.Id);
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
                return View("ManageRider", model);
            }
            return RedirectToAction("ManageRider");
        }

        public ActionResult DeleteRider(int id)
        {
            new RiderDAC().Delete(id);
            return RedirectToAction("ManageRider");
        }

        public ActionResult ManageArea()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertArea(Area model)
        {
            if(ModelState.IsValid)
            {
                new AreaDAC().Insert(model);
            }
            else
            {
                return View("ManageRider", model);
            }
            
            return RedirectToAction("ManageRider");
        }
    }
}