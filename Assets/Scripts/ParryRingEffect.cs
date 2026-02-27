using UnityEngine;

public class ParryRingEffect : MonoBehaviour
{
    [SerializeField] string radiusRef = "_Radius"; // Shader GraphのReference名
    [SerializeField] float duration = 0.25f;
    [SerializeField] float maxRadius = 0.8f;

    Material mat;
    float t;

    void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null) { Debug.LogError($"[{name}] SpriteRenderer not found"); enabled = false; return; }
        mat = sr.material;          // ★個体専用マテリアルになる
        mat.SetFloat(radiusRef, 0f);
    }

    void Update()
    {
        t += Time.deltaTime;
        float p = Mathf.Clamp01(t / duration);
        mat.SetFloat(radiusRef, p * maxRadius);

        if (t >= duration) Destroy(gameObject);
    }
}