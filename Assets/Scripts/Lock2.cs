using UnityEngine;

public class Lock2 : MonoBehaviour
{
    public GameObject lock1;
    public GameObject lock2;
    public static Lock2 Instance;


    void Awake()
    {
        Instance = this;
    }
    
    public void OpenLock()
    {

        Rigidbody rbl1 = lock1.GetComponent<Rigidbody>();
        rbl1.isKinematic = false;
        rbl1.useGravity = true;

        Rigidbody rbl2 = lock2.GetComponent<Rigidbody>();
        lock2.transform.Rotate(0f, -60f, 0f);
        rbl2.isKinematic = false;
        rbl2.useGravity = true;
        Destroy(lock1, 1f);
        Destroy(lock2, 1f);
    }
}
