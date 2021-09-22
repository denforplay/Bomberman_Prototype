namespace Plugins.Pool.Interfaces
{
    public interface IPoolable
    {
        IObjectPool Origin { get; set; }
        void ReturnToPool();
    }
}
