using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LaboratorioCRUDAlumnos
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
