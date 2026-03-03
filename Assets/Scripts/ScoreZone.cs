using UnityEngine;
using UnityEngine.SceneManagement;

public class PassDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Ստուգում ենք՝ արդյոք գնդակն է (օգտագործիր քո գնդակի Tag-ը)
        if (other.CompareTag("Ball"))
        {
            // Կանչում ենք ScoreManager-ը
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddRandomScore();
            }

            // Անջատում ենք Trigger-ը, որպեսզի նույն հարկի համար 1-ից ավել անգամ չհաշվի
            gameObject.SetActive(false);
        }
    }
}