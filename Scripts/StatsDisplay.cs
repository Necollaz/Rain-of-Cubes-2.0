using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] private Text _text;

    private int _totalCubesCreated;
    private int _totalBombsCreated;
    private int _activeCubes;
    private int _inactiveCubes;

    public void UpdateStats(int totalCubesCreated, int totalBombsCreated, int activeCubes, int inactiveCubes)
    {
        _totalCubesCreated = totalCubesCreated;
        _totalBombsCreated = totalBombsCreated;
        _activeCubes = activeCubes;
        _inactiveCubes = inactiveCubes;

        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"Cоздано кубов: {_totalCubesCreated}\n" +
                     $"Создано бомб: {_totalBombsCreated}\n" +
                     $"Активные кубы: {_activeCubes}\n" +
                     $"Неактивные кубы: {_inactiveCubes}";
    }
}
