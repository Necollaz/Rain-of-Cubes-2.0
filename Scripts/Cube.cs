using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _minTimeLife = 2.0f;
    [SerializeField] private float _maxTimeLife = 5.0f;
        
    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _disappearCoroutine;
    private bool _hasTouchedPlatform = false;

    public event System.Action<Cube> OnReleased;

    public static int TotalBlueCubes { get; private set; }
    public static int ActiveBlueCubes { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Reset();
    }

    private void OnDisable()
    {
        if (_renderer.material.color == Color.blue)
        {
            ActiveBlueCubes--;
        }
    }

    public void SetInitialVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    private void Reset()
    {
        _hasTouchedPlatform = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _renderer.material.color = Color.red;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Platform _) && _hasTouchedPlatform == false)
        {
            _hasTouchedPlatform = true;
            _renderer.material.color = Color.blue;
            StartDisappearing();
            TotalBlueCubes++;
            ActiveBlueCubes++;
        }
    }

    private void StartDisappearing()
    {
        if (_disappearCoroutine != null)
        {
            StopCoroutine(_disappearCoroutine);
        }

        _disappearCoroutine = StartCoroutine(DisappearAfterDelay());
    }

    private IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(Random.Range(_minTimeLife, _maxTimeLife));
        OnReleased?.Invoke(this);
    }
}