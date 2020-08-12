using System.Web;
using System.Web.Mvc;

namespace ZumaneGas_OperationalSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
