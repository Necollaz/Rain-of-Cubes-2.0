using UnityEngine;
using UnityEngine.Pool;

public class Pool<T> where T : MonoBehaviour
{
    private ObjectPool<T> _pool;

    public int CountActive => _pool.CountActive;
    public int CountInactive => _pool.CountInactive;

    public Pool(System.Func<T> createFunc, System.Action<T> onGetAction = null, System.Action<T> onReleaseAction = null, int defaultCapacity = 5, int maxSize = 20)
    {
        _pool = new ObjectPool<T>(
            createFunc,
            actionOnGet: onGetAction,
            actionOnRelease: onReleaseAction,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize);
    }

    public T Get()
    {
        return _pool.Get();
    }

    public void Release(T obj)
    {
        _pool.Release(obj);
    }
}
