using Rhendaria.Abstraction;

namespace Rhendaria.Web.Models
{
    public class CellModel
    {
        public string Nickname { get; set; }

        public int Color { get; set; }

        public Vector2D Position { get; set; }

        public int Score { get; set; }
    }
}
