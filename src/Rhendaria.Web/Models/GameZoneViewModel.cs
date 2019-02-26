using Rhendaria.Web.Controllers;
using System.Collections.Generic;

namespace Rhendaria.Web.Models
{
    public class GameZoneViewModel
    {
        public SpriteStateViewModel Player { get; set; }

        public ICollection<SpriteStateViewModel> Sprites { get; set; }

        public ZoneViewModel Zone { get; set; }
    }
}
