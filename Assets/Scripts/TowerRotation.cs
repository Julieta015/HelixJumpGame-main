using UnityEngine;
using UnityEngine.InputSystem;

public class TowerRotatingNewInput : MonoBehaviour
{
    public float sensitivity = 0.5f;

    void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.isPressed)
        {
            // Վերցնում ենք մկնիկի շարժը
            float mouseDeltaX = Mouse.current.delta.x.ReadValue();

            // Հիմա մենք պտտում ենք ԾՆՈՂԻՆ իր տեղական Y առանցքի շուրջ
            // Սա կպտտի նաև ներսի գլանը՝ իր առանցքի շուրջ
            transform.Rotate(0, -mouseDeltaX * sensitivity, 0, Space.Self);
        }
    }
}