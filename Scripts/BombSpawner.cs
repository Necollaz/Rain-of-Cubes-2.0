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
    public event Action BombExploded;

    private int _totalCreated;
    private int _activeBombs;

    public int TotalCreated => _totalCreated;
    public int ActiveBombs => _activeBombs;

    private void Awake()
    {
        InitializePool();
        _explosion.Initialize(_bombPool);
    }

    private void InitializePool()
    {
        _bombPool = new Pool<Bomb>(
            () => Instantiate(_bombPrefab),
            bomb => bomb.gameObject.SetActive(true),
            bomb => bomb.gameObject.SetActive(false),
            _poolCapacity,
            _poolMaxSize);
    }

    public void CreateBomb(Vector3 position)
    {
        Bomb bomb = _bombPool.Get();
        _totalCreated++;
        _activeBombs++;
        BombCreated?.Invoke();
        bomb.transform.position = position;
        bomb.BombExploded += ExplodeBomb;
        bomb.gameObject.SetActive(true);
    }

    private void ExplodeBomb(Bomb bomb)
    {
        _explosion.Explode(bomb);
        bomb.BombExploded -= ExplodeBomb;
        _activeBombs--;
        BombExploded?.Invoke();
    }
}
