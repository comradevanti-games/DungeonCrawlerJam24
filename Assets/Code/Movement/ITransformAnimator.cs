using System.Threading.Tasks;

namespace DGJ24.Movement
{
    public interface ITransformAnimator
    {
        public Task SyncPosition();

        public Task SyncRotation();
    }
}
