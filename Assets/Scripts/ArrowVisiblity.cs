using UnityEngine;

public class ArrowVisibility : MonoBehaviour
{
    public GameObject arrowButton; // Քաշիր քո UI սլաքը այստեղ
    public Transform levelsHolder; // Լեվելների պապկան
    public float hideDistance = 2f; // Ինչքան մոտ լինի, որ սլաքը կորի

    void Update()
    {
        // 1. Ստուգում ենք՝ արդյոք ունենք հասած լեվել
        int targetIndex = BallBounce.completLevel; // Կամ այն լեվելը, որին պետք է նայել

        if (levelsHolder != null && levelsHolder.childCount > targetIndex)
        {
            Transform targetLevel = levelsHolder.GetChild(targetIndex);

            // 2. Հաշվում ենք հեռավորությունը տեսախցիկի և լեվելի միջև
            // Միայն Y առանցքով ենք ստուգում, քանի որ լեվելները ուղղահայաց են
            float distance = Mathf.Abs(transform.position.y - (targetLevel.position.y + 5f));

            // 3. Եթե հեռավորությունը փոքր է hideDistance-ից, սլաքը անջատում ենք
            if (distance < hideDistance)
            {
                arrowButton.SetActive(false);
            }
            else
            {
                arrowButton.SetActive(true);
            }
        }
    }
}