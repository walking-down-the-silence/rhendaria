using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rhendaria.Abstraction.Actors;
using Rhendaria.Abstraction.Services;

namespace Rhendaria.Engine.Services
{
    public class ScoreCalculationService : IScoreCalculatingService
    {
        public Task<int> CalculateScore(IPlayerActor winner, ICollection<IPlayerActor> loosers)
        {
            var sum = loosers
                .Select(async looser => await looser.GetSize())
                .Sum(task => task.Result);

            return Task.FromResult(sum);
        }
    }
}