using UnityEngine;

public class Lock : MonoBehaviour
{
    public GameObject locks;

    public static Lock Instance;


    void Awake()
    {
        Instance = this;
    }
    
    public void OpenLock()
    {

        Rigidbody rb = locks.GetComponent<Rigidbody>();
        locks.transform.Rotate(0f, 0f, -20f);
        rb.isKinematic = false;
        rb.useGravity = true;
        Destroy(locks, 1f);
    }
}
