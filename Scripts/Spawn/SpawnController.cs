using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private InfoDisplay _infoDisplay;

    private void Start()
    {
        _infoDisplay.Initialize(_cubeSpawner, _bombSpawner);
    }

    private void Awake()
    {
        _cubeSpawner.CubeReleased += OnCubeReleased;
    }

    private void OnDestroy()
    {
        _cubeSpawner.CubeReleased -= OnCubeReleased;
    }

    private void OnCubeReleased(Cube cube)
    {
        _bombSpawner.Get(cube.transform.position);
    }
}
