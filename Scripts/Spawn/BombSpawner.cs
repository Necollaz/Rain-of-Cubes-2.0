using UnityEngine;
using System;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private Explosion _explosion;

    public event Action BombExploded;

    private int _activeBombs;

    public int ActiveBombs => _activeBombs;

    protected override void Awake()
    {
        base.Awake();
        _explosion.Initialize(_pool);
    }

    public override Bomb Get(Vector3 position)
    {
        Bomb bomb = base.Get(position);
        _activeBombs++;
        bomb.Exploded += Explode;
        return bomb;
    }

    protected override void InitializePool()
    {
        _pool = new Pool<Bomb>(
            CreateInstance,
            bomb => bomb.gameObject.SetActive(true),
            bomb => bomb.gameObject.SetActive(false),
            _poolCapacity,
            _poolMaxSize
        );
    }

    private void Explode(Bomb bomb)
    {
        _explosion.Explode(bomb);
        bomb.Exploded -= Explode;
        _activeBombs--;
        BombExploded?.Invoke();
    }
}
