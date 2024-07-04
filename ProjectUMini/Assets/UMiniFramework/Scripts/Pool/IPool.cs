using UnityEngine.UIElements;

namespace UMiniFramework.Scripts.Pool
{
    public interface IPool<T>
    {
        T Get();
        void Back(T obj);
    }
}