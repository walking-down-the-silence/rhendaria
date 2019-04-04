using System.Threading.Tasks;
using Orleans;

namespace Rhendaria.Abstraction.Actors
{
    public interface IMessageContainer : IGrainWithStringKey
    {
        Task Subscribe(IClientEventListener clientEventListener);
        Task InsertMessage(string message);
    }

    public interface IClientEventListener : IGrainObserver
    {
        void PushMessage(string message);
    }
}
