using System.Collections.Generic;
using System.Threading.Tasks;
using Rhendaria.Abstraction.Actors;

namespace Rhendaria.Abstraction.Services
{
    public interface ICollisionDetectingService
    {
        Task<CollisionResult> DetectCollision(IPlayerActor player, ICollection<IPlayerActor> opponents);
    }
}