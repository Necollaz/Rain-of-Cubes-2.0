using System;
using UnityEngine;
using UnityEngine.Pool;

public class Pool<T> where T : MonoBehaviour
{
    private ObjectPool<T> _pool;

    public int CountActive => _pool.CountActive;
    public int CountInactive => _pool.CountInactive;

    public Pool(Func<T> createFunc, Action<T> onGetAction = null, Action<T> onReleaseAction = null, int defaultCapacity = 5, int maxSize = 20)
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
