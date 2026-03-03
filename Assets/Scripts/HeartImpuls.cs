using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    public float beatSpeed = 1.5f;
    public float scaleAmount = 0.2f;
    public AnimationCurve beatCurve;

    Vector3 startScale;
    float timer;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime * beatSpeed;

        if (timer > 1f)
            timer = 0f;

        float curveValue = beatCurve.Evaluate(timer);
        transform.localScale = startScale * (1 + curveValue * scaleAmount);
    }
}
