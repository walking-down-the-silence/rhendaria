using System.Collections.Generic;

namespace Rhendaria.Web.Models
{
    public class GameModel
    {
        public PlayerModel Player { get; set; }

        public ICollection<SpriteModel> Sprites { get; set; }

        public ZoneModel Zone { get; set; }
    }
}
