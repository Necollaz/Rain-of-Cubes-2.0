using UnityEngine;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField] private Text _cubeInfoText;
    [SerializeField] private Text _bombInfoText;

    private CubeSpawner _cubeSpawner;
    private BombSpawner _bombSpawner;

    public void Initialize(CubeSpawner cubeSpawner, BombSpawner bombSpawner)
    {
        _cubeSpawner = cubeSpawner;
        _bombSpawner = bombSpawner;

        _cubeSpawner.Created += UpdateCubeInfo;
        _bombSpawner.Created += UpdateBombInfo;
        _bombSpawner.BombExploded += UpdateBombInfo;

        UpdateCubeInfo();
        UpdateBombInfo();
    }

    private void OnDestroy()
    {
        _cubeSpawner.Created -= UpdateCubeInfo;
        _bombSpawner.Created -= UpdateBombInfo;
        _bombSpawner.BombExploded -= UpdateBombInfo;
    }

    private void UpdateCubeInfo() => _cubeInfoText.text = $"Создано кубов: {_cubeSpawner.TotalCreated}" +
        $"\nАктивные кубы: {_cubeSpawner.GetCountActive()}";

    private void UpdateBombInfo() => _bombInfoText.text = $"Создано бомб: {_bombSpawner.TotalCreated}" +
        $"\nАктивные бомбы: {_bombSpawner.ActiveBombs}";
}
