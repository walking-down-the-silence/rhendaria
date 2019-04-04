using System.Threading.Tasks;
using Orleans;

namespace Rhendaria.Abstraction.Actors
{
    public interface IMessageContainer : IGrainWithStringKey
    {
        Task InsertMessage();
    }
}
