using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private StatsDisplay _stats;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 20;

    private Pool<Cube> _cubePool;
    private Pool<Bomb> _bombPool;
    private int _totalCubesCreated;
    private int _totalBombsCreated;

    private void Awake()
    {
        InitializePool();
    }

    private void Start()
    {
        UpdateStatsDisplay();
        StartCoroutine(SpawnCubeCoroutine());
    }

    public void ReleaseCube(Cube cube)
    {
        CreateBombInstance(cube.transform.position);
        _cubePool.Release(cube);
        UpdateStatsDisplay();
    }

    private void CreateBombInstance(Vector3 position)
    {
        Bomb bomb = _bombPool.Get();
        bomb.transform.position = position;
        bomb.OnBombExploded += ExplodeBomb;
        bomb.gameObject.SetActive(true);
        _totalBombsCreated++;
        UpdateStatsDisplay();
    }

    private void ExplodeBomb(Bomb bomb)
    {
        _explosion.Explode(bomb);
        Destroy(bomb.gameObject);
        UpdateStatsDisplay();
    }

    private void InitializePool()
    {
        _cubePool = new Pool<Cube>(CreateCubeInstance, PrepareCube, cube => cube.gameObject.SetActive(false), _poolCapacity, _poolMaxSize);
        _bombPool = new Pool<Bomb>(() => Instantiate(_bombPrefab), null, null, _poolCapacity, _poolMaxSize);
    }

    private Cube CreateCubeInstance()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.OnReleased += ReleaseCube;
        _totalCubesCreated++;
        return cube;
    }

    private void PrepareCube(Cube cube)
    {
        cube.transform.position = _spawnPoint.transform.position;
        cube.SetInitialVelocity(Vector3.down);
        cube.gameObject.SetActive(true);
    }

    private IEnumerator SpawnCubeCoroutine()
    {
        while (true)
        {
            _cubePool.Get();
            UpdateStatsDisplay();
            yield return new WaitForSeconds(_repeatRate);
        }
    }

    private void UpdateStatsDisplay()
    {
        _stats.UpdateStats(_totalCubesCreated, _totalBombsCreated, _cubePool.CountActive, _cubePool.CountInactive);
    }
}
