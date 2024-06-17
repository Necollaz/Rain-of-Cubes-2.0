using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody), typeof(MeshRenderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 2f;

    private MeshRenderer _renderer;
    private Material _material;

    public delegate void BombExploded(Bomb bomb);
    public event BombExploded OnBombExploded;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.material;
    }

    private void OnEnable()
    {
        _material.SetFloat("_Mode", 2);
        _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _material.SetInt("_ZWrite", 0);
        _material.DisableKeyword("_ALPHATEST_ON");
        _material.EnableKeyword("_ALPHABLEND_ON");
        _material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _material.renderQueue = 3000;

        StartCoroutine(FadeOutAndExplode());
    }

    private IEnumerator FadeOutAndExplode()
    {
        float timer = 0f;
        Color startColor = _renderer.material.color;

        while(timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / _fadeDuration);
            _renderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        OnBombExploded?.Invoke(this);
    }
}
