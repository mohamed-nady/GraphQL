using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestGraphQL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public object GraphQL(string query)
        {
            try
            {

                var dict = new GraphQlSchema().GraphQL.ExecuteQuery(query);
                return JsonConvert.SerializeObject(dict);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}