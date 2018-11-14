using Microsoft.AspNetCore.Mvc;

namespace Rhendaria.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static string NameOf<TController>(this Controller controller) where TController : Controller
        {
            return typeof(TController).Name.Replace(nameof(Controller), string.Empty);
        }
    }
}
