using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodDelivery.WebApp.Models
{
    public static class ModelHelper
    {
        public static Restaurant ToRestaurant(this RestaurantModel model)
        {
            Restaurant r = new Restaurant();
            //p.Id = model.Id;
            //p.Name = model.Name;
            //p.Price = model.Price;
            //p.Rank = model.Rank;
            //p.Description = model.Description;
            //p.IsActive = model.IsActive;

            //string[] dParts = model.DealingSince.Split('-');
            //p.DealingSince = new DateTime(Convert.ToInt32(dParts[2]), Convert.ToInt32(dParts[1]), Convert.ToInt32(dParts[0]));

            //p.Category = new Category { Id = model.SelectedCategoryId };
            //p.MadeIn = new Country { Id = model.MadeInId };
            //if (model.ImageUrls != null)
            //{
            //    for (int i = 0; i < model.ImageUrls.Count; i++)
            //    {
            //        p.Images.Add(new ProductImage { ImageUrl = model.ImageUrls[i], Rank = i + 1 });
            //    }
            //}
            return r;
        }
    }
}