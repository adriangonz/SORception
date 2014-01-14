using Eggplant.Filters;
using System.Web;
using System.Web.Mvc;

namespace Eggplant
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
