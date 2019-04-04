using Microsoft.AspNetCore.Mvc;

namespace RhendariaObserver.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static string NameOf<TController>(this ControllerBase controller) where TController : ControllerBase
        {
            return typeof(TController).Name.Replace(nameof(Controller), string.Empty);
        }
    }
}
