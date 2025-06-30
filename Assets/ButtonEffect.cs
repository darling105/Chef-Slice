using UnityEngine;

public class ButtonEffect : MonoBehaviour
{
   private Vector3 originalScale;
    public float scaleAmount = 0.1f;
    public float speed = 2f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = originalScale * scale;
    }
}
