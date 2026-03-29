using UnityEngine;

public class CameraFollowMap : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // Շարժվելու արագությունը
    public float minY = 0f;          // Ամենացածր կետը (Level 1)
    public float maxY = 50f;         // Ամենաբարձր կետը (Level 20)

    private Vector3 lastMousePosition;

    void Update()
    {
        // Երբ սեղմում ես մկնիկի ձախ կոճակը կամ մատով դիպչում ես էկրանին
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            // Հաշվում ենք, թե որքան է շարժվել մկնիկը/մատը
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Շարժում ենք տեսախցիկը Y (վերև) և Z (առաջ) առանցքներով
            float moveY = delta.y * scrollSpeed * Time.deltaTime;

            // Քանի որ մեր Map-ը թեքությամբ է բարձրանում, շարժում ենք և՛ Y-ը, և՛ Z-ը
            Vector3 newPos = transform.position + new Vector3(0, -moveY, -moveY * 0.5f);

            // Սահմանափակում ենք, որ տեսախցիկը քարտեզից դուրս չգնա
            newPos.y = Mathf.Clamp(newPos.y, minY + 5f, maxY);

            transform.position = newPos;
            lastMousePosition = Input.mousePosition;
        }
    }
}