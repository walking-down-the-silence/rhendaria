using System.Collections.Generic;
using System.Threading.Tasks;
using Rhendaria.Abstraction.Actors;

namespace Rhendaria.Abstraction.Services
{
    public interface IScoreCalculatingService
    {
        Task<int> CalculateScore(IPlayerActor winner, ICollection<IPlayerActor> loosers);
    }
}