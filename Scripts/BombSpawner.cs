using System;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 20;

    private Pool<Bomb> _bombPool;
    public event Action BombCreated;
    private int _totalCreated;

    public int TotalCreated => _totalCreated;
    public int CountActive => _bombPool.CountActive;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _bombPool = new Pool<Bomb>(() => Instantiate(_bombPrefab), null, bomb => bomb.gameObject.SetActive(false), _poolCapacity, _poolMaxSize);
    }

    public void CreateBomb(Vector3 position)
    {
        Bomb bomb = _bombPool.Get();
        _totalCreated++;
        BombCreated?.Invoke();
        bomb.transform.position = position;
        bomb.BombExploded += ExplodeBomb;
        bomb.gameObject.SetActive(true);
    }

    private void ExplodeBomb(Bomb bomb)
    {
        _explosion.Explode(bomb);
        Destroy(bomb.gameObject);
    }
}
