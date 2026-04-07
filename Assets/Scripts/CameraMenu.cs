using UnityEngine;
using UnityEngine.EventSystems;

public class CameraFollowMap : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // Շարժվելու արագությունը
    public float minZ = 0f;          // Ամենացածր կետը (Level 1)
    public float maxZ = 40f;         // Ամենաբարձր կետը (Level 20)

    private Vector3 lastMousePosition;

    void Update()
    {
        if (CameraMove.IsAutoMoving)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // Երբ սեղմում ես մկնիկի ձախ կոճակը կամ մատով դիպչում ես էկրանին
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            // Հաշվում ենք, թե որքան է շարժվել մկնիկը/մատը
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Շարժում ենք տեսախցիկը Y և Z առանցքներով, բայց ոչ X
            float moveZ = delta.y * scrollSpeed;

            Vector3 newPos = transform.position + new Vector3(0f, 0f, -moveZ);

            // Սահմանափակում ենք, որ տեսախցիկը քարտեզից դուրս չգնա
            newPos.z = Mathf.Clamp(newPos.z, minZ + 5f, maxZ);

            transform.position = newPos;
            lastMousePosition = Input.mousePosition;
        }
    }
}
