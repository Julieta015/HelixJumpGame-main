/*using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BallBounce : MonoBehaviour
{
    public float bounceForce = 1f;
    public float spinSpeed = 180f; // աստիճան/վայրկյան, գնդակի պտույտը իր առանցքի շուրջ
    [Header("Bounce sound")]
    public AudioClip segmentTouchSound; // Քաշիր ձայնի ֆայլը Inspector-ում
    Rigidbody rb;
    AudioSource audioSource;
    private bool change = false;
    public static int completLevel = 1;
    private int completSeg = 0;
    private bool istuch = false;
    private bool isfire = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Գնդակը ամբողջ խաղի ընթացքում պտտվում է իր տեղում (Y առանցքի շուրջ)
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.6f))
        {
            if (!isfire)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    istuch = true;
                    if (segmentTouchSound != null && audioSource != null)
                        audioSource.PlayOneShot(segmentTouchSound);
                    rb.linearVelocity = Vector3.zero;
                    rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                }
                else if (hit.collider.CompareTag("Danger"))
                {

                    SceneManager.LoadScene(2);


                }
                else if (hit.collider.CompareTag("LevelComplet"))
                {
                    if (change == false)
                    {
                        Debug.Log("level completed");
                        completLevel++;
                        Level.level = completLevel;
                        change = true;
                    }
                    StartCoroutine(WaitAndLog());
                    SceneManager.LoadScene(1);
                }

                if (!istuch && completSeg == 3)
                {
                    isfire = true;
                    MeshRenderer renderer = GetComponent<MeshRenderer>();
                    renderer.material.color = Color.red;
                }
                else
                {
                    MeshRenderer renderer = GetComponent<MeshRenderer>();
                    renderer.material.color = Color.blue;
                }

            }
            else
            {
                istuch = false;
                completSeg = 0;
                isfire = false;
            }
            
        }
    }

    IEnumerator WaitAndLog()
    {
        Debug.Log("Սպասում ենք...");

        // Սպասել 1 վայրկյան
        yield return new WaitForSeconds(1f);

        Debug.Log("1 վայրկյանն անցավ:");
    }
    void ReloadLevel()
    {
        // Վերցնում ենք ընթացիկ ակտիվ scene-ի ինդեքսը և նորից բեռնում այն
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreZone"))
        {
            other.enabled = false;
            TowerGenerator.Instance.Done();
            Debug.Log("Score Zone-ն աշխատեց!");
        }
    }*

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreZone"))
        {
            // 1. Անմիջապես ջնջում ենք ScoreZone-ը, որ էլ չխանգարի
            Destroy(other.gameObject);
            completSeg++;
            // 2. Գրում ենք Console-ում, որ տեսնենք աշխատեց թե չէ
            Debug.Log("ScoreZone-ին կպա! Հիմա պետք է թափվի հաջորդ հարթակը:");

            if (TowerGenerator.Instance != null)
            {
                TowerGenerator.Instance.Done();
                TowerGenerator.Instance.RemovePassedRings();
            }
        }
    }
}*/

/*using UnityEngine;
using UnityEngine.SceneManagement;

public class BallBounce : MonoBehaviour
{
    public float bounceForce = 4f;
    public float spinSpeed = 180f;
    public AudioClip segmentTouchSound;

    Rigidbody rb;
    AudioSource audioSource;
    MeshRenderer ballRenderer;

    private bool change = false;
    public static int completLevel = 1;
    private int completSeg = 0; // Քանի հարկ է անցել օդում
    private bool isfire = false; // Super Jump վիճակ

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        ballRenderer = GetComponent<MeshRenderer>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 1. Ստուգում ենք Raycast-ով, թե ինչին ենք կպել
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.7f))
        {
            string tag = hit.collider.tag;

            // ԲԱՑԱՌՈՒԹՅՈՒՆ. Եթե վերջին հարթակն է (Finish), ապա կապ չունի` կրակ է, թե չէ
            if (tag == "LevelComplet")
            {
                if (!change)
                {
                    Debug.Log("Level Completed!");
                    completLevel++;
                    Level.level = completLevel;
                    change = true;

                    // Պահպանում ենք լեվելը, որ սկինը փոխելիս չկորի
                    PlayerPrefs.SetInt("SavedLevel", completLevel);
                    PlayerPrefs.Save();

                    SceneManager.LoadScene(1);
                }
                return; // Դուրս ենք գալիս, որ սուպեր ուժը չաշխատի այստեղ
            }

            // 2. Եթե վերջին հարթակը չէ, բայց գնդակը կրակ է (Super Jump)


            if (isfire)
            {
                // Կոտրում ենք հարթակը անկախ նրանից, թե ինչ թեգ ունի
                BreakPlatform(collision.gameObject);
                StopFireMode();
                return;
            }


            if (tag == "Ground")
            {
                NormalBounce();
            }
            else if (tag == "Danger")
            {
                SceneManager.LoadScene(2); // Պարտություն
            }
            else if (tag == "LevelComplet")
            {
                if (!change)
                {
                    completLevel++;
                    Level.level = completLevel;
                    change = true;
                    SceneManager.LoadScene(1);
                }
            }

            // 3. Սովորական բախում (երբ կրակ չէ)
            if (tag == "Ground")
            {
                NormalBounce();
            }
            else if (tag == "Danger")
            {
                // Պարտություն
                SceneManager.LoadScene(2);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreZone"))
        {
            Destroy(other.gameObject);
            completSeg++;

            // Եթե 3 և ավելի հարկ է անցել առանց կպնելու
            if (completSeg >= 3)
            {
                StartFireMode();
            }

            if (TowerGenerator.Instance != null)
            {
                TowerGenerator.Instance.Done();
                TowerGenerator.Instance.RemovePassedRings();
            }
        }
    }

    void NormalBounce()
    {
        completSeg = 0; // Զրոյացնում ենք հաշվիչը, որովհետև կպանք հարթակին
        if (segmentTouchSound != null)
            audioSource.PlayOneShot(segmentTouchSound);

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }

    void StartFireMode()
    {
        isfire = true;
        ballRenderer.material.color = Color.red;
    }

    void StopFireMode()
    {
        isfire = false;
        completSeg = 0;
        ballRenderer.material.color = Color.blue;

        // Սուպեր հարվածից հետո էլի պետք է թռչի վերև
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }

    void BreakPlatform(GameObject platform)
    {
        // Գտնում ենք հարթակի հիմնական օբյեկտը և ջարդում
        if (TowerGenerator.Instance != null)
        {
            // Կախված նրանից, թե ինչպես է կոչվում քո ջարդելու ֆունկցիան
            TowerGenerator.Instance.Done();
        }
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallBounce : MonoBehaviour
{
    [Header("Bounce")]
    public float bounceForce = 4f;     // սա իրականում "վերևի արագությունն" է (միշտ նույնը)
    public float spinSpeed = 180f;
    public List<Material> materials;
    public float splashFadeDelay = 0.8f; // Որքան մնա անփոփոխ
    public float splashFadeTime = 0.3f;  // Որքան տևի անհետանալը

    [Header("Audio")]
    public AudioClip segmentTouchSound;

    Rigidbody rb;
    AudioSource audioSource;
    MeshRenderer ballRenderer;
    public GameObject splashPrefab;
    private bool change = false;
    public static int completLevel = 1;

    private int completSeg = 1;     // քանի ScoreZone է անցել առանց հարթակ դիպչելու
    private bool isFire = false;    // fire mode

    void Start()
    {
        Level.level = completLevel;

        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        ballRenderer = GetComponent<MeshRenderer>();
        Material levelMaterial = GetLevelMaterial();
        if (ballRenderer != null && levelMaterial != null)
        {
            ballRenderer.material = levelMaterial;
        }
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        // ցանկալի (կայուն ֆիզիկա)
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }


    void OnCollisionEnter(Collision collision)
    {
        // Ստուգում ենք տակինը (ինչի վրա է կանգնել)
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.7f))
            return;

        string tag = hit.collider.tag;

        // ✅ Finish / LevelComplet
        if (tag == "LevelComplet")
        {
            if (!change)
            {
                if (Level.level == completLevel || completLevel == 1) // || 
                {
                    Debug.Log("Level Completed!");                  
                    
                    completLevel++;
                    Level.level = completLevel;
                    //StartCoroutine(cloud.CloudMove());
                }
                
                change = true;

                SceneManager.LoadScene(1);
            }
            return;
        }

        // ✅ Fire mode — հարվածում ենք ու անմիջապես անջատում
        if (isFire)
        {
            BreakPlatform(collision.gameObject);
            StopFireModeAndBounce();
            return;
        }

        // ✅ Normal collisions
        if (tag == "Ground")
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 spawnPos = contact.point + new Vector3(0, 0.001f, 0);

            // 2. Ռանդոմ պտույտ (0-ից 360 աստիճան)
            float randomY = Random.Range(0f, 360f);
            Quaternion splashRot = Quaternion.Euler(90f, randomY, 0f);

            GameObject splash = Instantiate(splashPrefab, spawnPos, splashRot);
            splash.transform.SetParent(collision.transform);
            SpriteRenderer sp = splash.GetComponent<SpriteRenderer>();
            Material levelMaterial = GetLevelMaterial();
            if (sp != null && levelMaterial != null)
            {
                sp.color = levelMaterial.color;
            }
            // 3. Կանչում ենք անհետացման ֆունկցիան հենց այս սկրիպտից
            StartCoroutine(FadeAndDestroySplash(splash));

            NormalBounce();
        }
        else if (tag == "Danger")
        {
            SceneManager.LoadScene(2);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ScoreZone")) return;
        other.enabled = false;
        Destroy(other.gameObject);
        completSeg++;
        if (LevelProgressBar.Instance != null)
        {
            LevelProgressBar.Instance.IncrementProgress();
        }
        // եթե 3 և ավել հարկ անցավ առանց դիպչելու՝ fire mode
        if (completSeg >= 3)
            StartFireMode();

        if (TowerGenerator.Instance != null)
        {
            TowerGenerator.Instance.Done();
            TowerGenerator.Instance.RemovePassedRings();
        }
    }

    IEnumerator FadeAndDestroySplash(GameObject splash)
    {
        yield return new WaitForSeconds(splashFadeDelay);

        if (splash != null)
        {
            SpriteRenderer sr = splash.GetComponent<SpriteRenderer>();

            Material levelMaterial = GetLevelMaterial();
            if (sr != null && levelMaterial != null)
            {
                sr.color = levelMaterial.color;
            }

            if (sr != null)
            {
                float elapsed = 0;
                Color startColor = sr.color;

                while (elapsed < splashFadeTime)
                {
                    if (splash == null) break; // Եթե հարթակը թափվեց ու splash-ը ջնջվեց
                    elapsed += Time.deltaTime;
                    float alpha = Mathf.Lerp(1f, 0f, elapsed / splashFadeTime);
                    sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                    yield return null;
                }
            }
            if (splash != null) Destroy(splash);
        }
    }
    // -------------------- Bounce Logic --------------------

    void NormalBounce()
    {
        completSeg = 0;

        if (segmentTouchSound != null)
            audioSource.PlayOneShot(segmentTouchSound);

        // ✅ Bounce միայն երբ ներքև է գնում, որ էներգիա չկուտակվի
        if (rb.linearVelocity.y <= 0f)
        {
            Vector3 v = rb.linearVelocity;
            v.y = bounceForce;          // միշտ նույն բարձրության համար նույն Y արագություն
            rb.linearVelocity = v;
        }
    }

    void StartFireMode()
    {
        isFire = true;
        if (ballRenderer != null)
            ballRenderer.material.color = Color.red;
    }

    void StopFireModeAndBounce()
    {
        isFire = false;
        completSeg = 0;

        Material levelMaterial = GetLevelMaterial();
        if (ballRenderer != null && levelMaterial != null)
            ballRenderer.material = levelMaterial;



        // ✅ Fire-ից հետո էլի նույն ստատիկ bounce
        Vector3 v = rb.linearVelocity;
        v.y = bounceForce;
        rb.linearVelocity = v;
    }

    // -------------------- Platform Break --------------------

    void BreakPlatform(GameObject platform)
    {
        // Եթե ուզում ես կոնկրետ սեգմենտը փշրվի՝
        // այստեղ կանչիր քո "Ring" կամ "TowerGenerator" break մեթոդը։
        // Հիմա թողնում եմ քո մոտ եղածը՝ Done()
        if (TowerGenerator.Instance != null)
            TowerGenerator.Instance.Done();
    }

    Material GetLevelMaterial()
    {
        return GetMaterialByIndex(materials, Level.level -1);
    }

    Material GetMaterialByIndex(List<Material> source, int rawIndex)
    {
        if (source == null || source.Count == 0)
            return null;

        int safeIndex = ((rawIndex % source.Count) + source.Count) % source.Count;
        return source[safeIndex];
    }
}
