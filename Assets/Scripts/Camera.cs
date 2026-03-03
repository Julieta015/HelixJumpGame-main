using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform ball; // Քաշիր գնդակը այս դաշտի մեջ
    private float offset; // Սկզբնական հեռավորությունը
    public float lerpSpeed = 5f; // Որքան սահուն հետևի տեսախցիկը

    void Start()
    {
        // Հաշվում ենք սկզբնական տարբերությունը տեսախցիկի և գնդակի միջև
        offset = transform.position.y - ball.position.y;
    }

    void LateUpdate()
    {
        // Հաշվում ենք թիրախային բարձրությունը (Y)
        float targetY = ball.position.y + offset;
        Vector3 currentPos = transform.position;

        // ԿԱՐԵՎՈՐ. Տեսախցիկը իջնում է միայն այն դեպքում, 
        // եթե թիրախային բարձրությունը ավելի ցածր է, քան տեսախցիկի ներկա բարձրությունը
        if (targetY < currentPos.y)
        {
            // Սահուն իջեցնում ենք տեսախցիկը
            currentPos.y = Mathf.Lerp(currentPos.y, targetY, lerpSpeed * Time.deltaTime);
            transform.position = currentPos;
        }
    }
}