using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    public Transform mapManager; // Տար MapManager-ը այստեղ
    public float speed = 5f;     // Տեղափոխման արագությունը
    public Vector3 offset = new Vector3(0, 5, -10); // Տեսախցիկի հեռավորությունը լեվելից

    public void GoToLevel()
    {

        if (mapManager.childCount >= 8)
        {
            Transform level5 = mapManager.GetChild(BallBounce.completLevel);
            Vector3 targetPosition = level5.position + offset;

            // Սկսում ենք սահուն շարժումը
            StopAllCoroutines();
            StartCoroutine(SmoothMove(targetPosition));
        }
    }

    IEnumerator SmoothMove(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            // Lerp-ը ապահովում է սահուն անցումը
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            yield return null;
        }
        transform.position = target;
    }
}