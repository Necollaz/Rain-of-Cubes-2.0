using UnityEngine;
using System.Collections;
using System;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 20;

    private Pool<Cube> _cubePool;
    public event Action<Cube> CubeReleased;
    public event Action CubeCreated;
    private int _totalCreated;

    public int TotalCreated => _totalCreated;


    private void Awake()
    {
        InitializePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnCubeCoroutine());
    }

    public int GetCountActive()
    {
        return _cubePool.CountActive;
    }

    public void ReleaseCube(Cube cube)
    {
        _cubePool.Release(cube);
        CubeReleased?.Invoke(cube);
    }

    public Cube GetCube()
    {
        var cube = _cubePool.Get();
        CubeCreated?.Invoke();
        return cube;
    }

    private void InitializePool()
    {
        _cubePool = new Pool<Cube>(CreateCubeInstance, PrepareCube, cube => cube.gameObject.SetActive(false), _poolCapacity, _poolMaxSize);
    }

    private Cube CreateCubeInstance()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.OnReleased += ReleaseCube;
        return cube;
    }

    private void PrepareCube(Cube cube)
    {
        _totalCreated++;
        CubeCreated?.Invoke();
        cube.transform.position = _spawnPoint.transform.position;
        cube.SetInitialVelocity(Vector3.down);
        cube.gameObject.SetActive(true);
    }

    private IEnumerator SpawnCubeCoroutine()
    {
        while (true)
        {
            _cubePool.Get();
            yield return new WaitForSeconds(_repeatRate);
        }
    }
}
