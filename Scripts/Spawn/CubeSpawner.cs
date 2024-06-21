using System;
using System.Collections;
using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _repeatRate = 1f;

    public event Action<Cube> CubeReleased;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(SpawnCoroutine());
    }

    public void Release(Cube cube)
    {
        _pool.Release(cube);
        CubeReleased?.Invoke(cube);
    }

    public int GetCountActive()
    {
        return Cube.ActiveBlueCubes;
    }

    protected override void InitializePool()
    {
        _pool = new Pool<Cube>(
            CreateInstance,
            Prepare,
            cube => cube.gameObject.SetActive(false),
            _poolCapacity,
            _poolMaxSize
        );
    }

    private void Prepare(Cube cube)
    {
        _totalCreated++;
        Created?.Invoke();
        cube.transform.position = _spawnPoint.position;
        cube.SetInitialVelocity(Vector3.down);
        cube.gameObject.SetActive(true);
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Get(_spawnPoint.position);
            yield return new WaitForSeconds(_repeatRate);
        }
    }
}
