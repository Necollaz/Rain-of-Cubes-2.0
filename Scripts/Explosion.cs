using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _force = 200f;
    [SerializeField] private float _radius = 50f;
    [SerializeField] private float _transparencySpeed = 1.5f;

    private IEnumerator ExplodeCoroutine(Bomb bomb)
    {
        Vector3 explosionPosition = bomb.transform.position;
        float explosionForce = CalculateForce(bomb);
        float explosionRadius = CalculateRadius(bomb);
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        while (bomb.GetComponent<Renderer>().material.color.a > 0)
        {
            Color color = bomb.GetComponent<Renderer>().material.color;
            color.a -= Time.deltaTime * _transparencySpeed;
            bomb.GetComponent<Renderer>().material.color = color;
            yield return null;
        }

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, 1.0f, ForceMode.Impulse);
            }
        }

        Destroy(bomb.gameObject);
    }

    public void Explode(Bomb bomb)
    {
        StartCoroutine(ExplodeCoroutine(bomb));
    }

    private float CalculateForce(Bomb bomb)
    {
        return _force / bomb.transform.localScale.magnitude;
    }

    private float CalculateRadius(Bomb bomb)
    {
        return _radius * bomb.transform.localScale.magnitude;
    }
}
