using System.Collections.Generic;

namespace Rhendaria.Web.Models
{
    public class GameZoneViewModel
    {
        public SpriteStateViewModel Player { get; set; }

        public ICollection<SpriteStateViewModel> Sprites { get; set; }
    }
}
