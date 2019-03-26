using System.Collections.Generic;

namespace Rhendaria.Web.Models
{
    public class GameModel
    {
        public CellModel PlayerCell { get; set; }

        public ICollection<CellModel> Cells { get; set; }
    }
}
