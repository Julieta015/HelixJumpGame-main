using System.Security.Cryptography;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    public Transform ball; 
    public Material finish;
    public GameObject ringPrefab;
    public int ringCount = 15;
    public float ringHeight = 0.6f;
    public float rotationSpeed = 0.3f;
    private float lastMouseX;
    public static TowerGenerator Instance;
    void Start()
    {
        Instance = this;
        
        GenerateTower();
        AccessLastRingSegments();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMouseX = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            float deltaX = Input.mousePosition.x - lastMouseX;
            lastMouseX = Input.mousePosition.x;

            transform.Rotate(0f, -deltaX * rotationSpeed, 0f);
        }
        
        

    }

    void GenerateTower()
    {
        for (int i = 0; i < ringCount; i++)
        {
            Vector3 pos = new Vector3(0, -i * ringHeight, 0);

            GameObject ring = Instantiate(
                ringPrefab,
                pos,
                Quaternion.identity,
                transform
            );

            // փոքր random rotation, որ չլինեն նույն gap-երը
            ring.transform.Rotate(0, Random.Range(0, 360f), 0);
        }
    }

    void AccessLastRingSegments()
    {
        // 1. Գտնում ենք Tower-ի ամենավերջին child-ը (վերջին հարթակը)
        // Tower-ը պետք է լինի այն օբյեկտը, որի վրա այս սկրիպտն է, կամ պետք է հղում ունենաս դրան
        int lastIndex = transform.childCount - 1;
        Transform lastRing = transform.GetChild(lastIndex);

        Debug.Log("Վերջին հարթակն է՝ " + lastRing.name);

        // 2. Անցնում ենք այդ վերջին հարթակի բոլոր child-ների վրայով
        foreach (Transform segment in lastRing)
        {
            // 3. Զտում ենք գլանը (Cylinder)
            if (segment.name.Contains("Cylinder"))
            {
                continue;
            }

            MeshRenderer mr = segment.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.material = finish;

            }

            segment.tag = "LevelComplet";
        }
    }

    public void RemovePassedRings()
    {
        if (transform.childCount == 0) return;
        if (ball == null) return;

        Transform topRing = transform.GetChild(0);
        Debug.Log("topring = ..");
        Debug.Log(topRing.name);
        // եթե գնդակը արդեն այդ օղակից ներքև է
        if (ball.position.y > topRing.position.y)
        {
            Debug.Log("Deleting");
            Destroy(topRing.gameObject);
        }
    }

    public void Done()
    {
        if (transform.childCount > 0)
        {
            Transform firstRing = transform.GetChild(0);

            // Վերցնում ենք հենց Tower-ի (այս սկրիպտի) դիրքը որպես կենտրոն
            Vector3 towerCenter = new Vector3(transform.position.x, firstRing.position.y, transform.position.z);

            foreach (Transform segment in firstRing)
            {
                
                if (segment.name.Contains("ScoreZone") || segment.name.Contains("ScoreZone")) continue;

                segment.SetParent(null);
                Rigidbody rb = segment.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.linearVelocity = Vector3.zero;

                    // ՈՒՂՂՈՒՄ. հաշվում ենք ուղղությունը Tower-ի առանցքից դեպի սեգմենտ
                    Vector3 direction = (segment.position - towerCenter).normalized;
                    direction.y = 0; // Ոչ մի վերև թռիչք

                    // Եթե direction-ը 0 է ստացվում, ստիպում ենք մի կողմ գնալ
                    if (direction.magnitude < 0.1f) direction = segment.right;

                    // Օգտագործիր VelocityChange - սա անտեսում է զանգվածը
                    rb.AddForce(direction * 0.3f, ForceMode.VelocityChange);

                    // Ավելացրու պատահական պտույտ
                    rb.angularVelocity = new Vector3(Random.Range(-5, 5), 5f, Random.Range(-5, 5));
                }
                //Destroy(segment.gameObject, 3f);
            }
            Destroy(firstRing.gameObject);
        }
    }


}
