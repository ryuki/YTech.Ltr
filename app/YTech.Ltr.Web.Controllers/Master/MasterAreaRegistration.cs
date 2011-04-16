using System.Web.Mvc;

namespace YTech.Ltr.Web.Controllers.Master
{
    public class MasterAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Master"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                    "Master_default",
                    "Master/{controller}/{action}/{id}",
                    new { action = "Index", id = "" }
                );
        }
    }
}
