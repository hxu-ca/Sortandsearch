using ProductionScheduleSheet.App_Start;
using System;
using System.Web.Http;

namespace ProductionScheduleSheet
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }

    }
}