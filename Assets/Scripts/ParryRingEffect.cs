using UnityEngine;

public class ParryRingEffect : MonoBehaviour
{
    [SerializeField] string radiusRef = "_Radius";
    [SerializeField] float duration = 0.75f;
    [SerializeField] float maxRadius = 0.75f;

    Material mat;
    float t;

    void Awake()
    {
        var sr = GetComponent<SpriteRenderer>();
        mat = sr.material;
        mat.SetFloat(radiusRef, 0f);
    }

    void Update()
    {
        t += Time.deltaTime;
        float p = Mathf.Clamp01(t / duration);
        mat.SetFloat(radiusRef, p * maxRadius);

        if (p >= 0.75f) Destroy(gameObject);
    }
}