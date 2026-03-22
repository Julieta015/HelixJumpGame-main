using UnityEngine;

public class BallLogic : MonoBehaviour
{
    public Transform levelsHolder; // Լեվելների պապկան
    public float jumpForce = 10f;   // Թռիչքի բարձրությունը
    public float moveSpeed = 5f;   // Լեվելից լեվել անցնելու արագությունը

    private Rigidbody rb;
    private int lastTargetLevel = 0;
    int currentLevelIndex;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        // Վերցնում ենք ընթացիկ բացված լեվելի ինդեքսը
        currentLevelIndex = Level.level - 1;

        if (Level.level == 0)
        {
            currentLevelIndex = 0;
            lastTargetLevel = -1;
        }

        if (levelsHolder != null && levelsHolder.childCount > currentLevelIndex)
        {
            Transform targetLevel = levelsHolder.GetChild(currentLevelIndex).GetChild(0).GetChild(1).GetChild(1);

            // 1. Շարժում ենք գնդակը դեպի լեվելի X և Z կոորդինատները
            Vector3 targetPos = new Vector3(targetLevel.position.x, transform.position.y, targetLevel.position.z);
            rb.MovePosition(Vector3.Lerp(rb.position, targetPos, Time.deltaTime * moveSpeed * (currentLevelIndex + 5)));

            // 2. Եթե գնդակը հասել է նոր լեվելին, կարող ենք թարմացնել վիճակը
            if (currentLevelIndex != lastTargetLevel)
            {
                lastTargetLevel = currentLevelIndex;
                Debug.Log("Գնդակը տեղափոխվում է լեվել: " + currentLevelIndex);
            }
        }
    }

    // 3. Թռիչքի մեխանիզմը (երբ դիպչում է գետնին)
    private void OnCollisionEnter(Collision collision)
    {
        // Ստուգում ենք, որ դիպչում է հենց լեվելին (Floor)
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Level"))
        {
            // Զրոյացնում ենք արագությունը և տալիս նոր թռիչքի ուժ դեպի վերև
            rb.linearVelocity = new Vector3(0, jumpForce , 0);
        }
    }
}