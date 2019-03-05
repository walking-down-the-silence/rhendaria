using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Engine.Services
{
    public class ScoreCalculationService : IScoreCalculatingService
    {
        public async Task<int> CalculateScore(IPlayerActor winner, ICollection<IPlayerActor> loosers)
        {
            var tasks = loosers.Select(looser => looser.GetState());
            var sizes = await Task.WhenAll(tasks);
            return sizes.Select(x => x.SpriteSize).Sum();
        }
    }
}