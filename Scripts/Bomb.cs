using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody), typeof(MeshRenderer))]
public class Bomb : MonoBehaviour
{
    private const string ModeProperty = "_Mode";
    private const string SrcBlendProperty = "_SrcBlend";
    private const string DstBlendProperty = "_DstBlend";
    private const string ZWriteProperty = "_ZWrite";
    private const string AlphaTestOnKeyword = "_ALPHATEST_ON";
    private const string AlphaBlendOnKeyword = "_ALPHABLEND_ON";
    private const string AlphaPremultiplyOnKeyword = "_ALPHAPREMULTIPLY_ON";
    private const int RenderQueueTransparent = 3000;

    [SerializeField] private float _fadeDuration = 2f;

    private MeshRenderer _renderer;
    private Material _material;

    public event Action<Bomb> Exploded;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.material;
    }

    private void OnEnable()
    {
        ConfigureMaterialForTransparency();
        StartCoroutine(FadeOut());
    }

    public Material GetMaterial()
    {
        return _material;
    }

    private void ConfigureMaterialForTransparency()
    {
        _material.SetFloat(ModeProperty, 2);
        _material.SetInt(SrcBlendProperty, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _material.SetInt(DstBlendProperty, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _material.SetInt(ZWriteProperty, 0);
        _material.DisableKeyword(AlphaTestOnKeyword);
        _material.EnableKeyword(AlphaBlendOnKeyword);
        _material.DisableKeyword(AlphaPremultiplyOnKeyword);
        _material.renderQueue = RenderQueueTransparent;
    }

    private IEnumerator FadeOut()
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

        Exploded?.Invoke(this);
    }
}
