using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Cargo4You.Data;
using Cargo4You.Models;

namespace Cargo4You.Filters
{
    public class UserActivity : IActionFilter

    {
        private readonly CargoDbContext cargoDbContext;
        public UserActivity(CargoDbContext cargoDbContext)
        {
            this.cargoDbContext = cargoDbContext;
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var data = "";
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            var url = $"{controllerName}/{actionName}";

            if (!string.IsNullOrEmpty(context.HttpContext.Request.QueryString.Value))
            {
                data = context.HttpContext.Request.QueryString.Value;
            }

            else
            {
                var userData = context.ActionArguments.FirstOrDefault();
                var stringUserData = JsonConvert.SerializeObject(userData);
                data = stringUserData;

            }

            StoreUserActivities(data, url);

        }



        public void StoreUserActivities(string data, string url)
        {

            var userActivity = new UserActivityModel
            {
                Data = data,
                Url = url,

            };

            cargoDbContext.UserActivities.Add(userActivity);
            cargoDbContext.SaveChanges();
        }
    }
}
