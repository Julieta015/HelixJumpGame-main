using UnityEngine;

public class LevelsColor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform mapManager; // Քո MapManager-ը
    Transform targetChild;
    private int lastProcessedLevel = -1;
    
    void LateUpdate()
    {
        if (BallBounce.completLevel > lastProcessedLevel)
        {
            ChangeColor();
            lastProcessedLevel = BallBounce.completLevel;
        }
    }
    void ChangeColor()
    {
        // Շղթան գնում է այսպես.
        // MapManager -> 2-րդ child (1) -> 2-րդ child (1) -> 2-րդ child (1)
        for (int i = 0; i < BallBounce.completLevel; i++)
        {
            Transform levelRow = mapManager.GetChild(i);
            Debug.Log(BallBounce.completLevel);
            Debug.Log("Level = " + Level.level);
            Debug.Log("i'm in for");
            targetChild = mapManager.GetChild(i).GetChild(0).GetChild(1).GetChild(1); 
            MeshRenderer renderer = targetChild.GetComponent<MeshRenderer>();
            Debug.Log(targetChild.name);
            if (renderer != null)
            {
                if (BallBounce.completLevel == i + 1)
                {
                    renderer.material.color = Color.yellow;
                    Level.level = BallBounce.completLevel  ;
                    Debug.Log(Level.level);
                    Lock colLock = mapManager.GetChild(i).GetComponentInChildren<Lock>(true);
                    OpenAllLocksInLevel(levelRow);
                    Lock2.Instance.OpenLock();
                }
                else {
                    Debug.Log("i'm in material if");
                    renderer.material.color = Color.green;
                    DisableLocks(levelRow);
                }

                
            }
        }

        void OpenAllLocksInLevel(Transform levelTransform)
        {
            // GetComponentsInChildren (հոգնակի) - սա գտնում է ԲՈԼՈՐ կողպեքները այս լեվելի տակ
            Lock[] allLocks = levelTransform.GetComponentsInChildren<Lock>(true);
            foreach (Lock l in allLocks)
            {
                l.OpenLock();
            }
            Lock2[] allLocks2 = levelTransform.GetComponentsInChildren<Lock2>(true);
            foreach (Lock2 l2 in allLocks2) l2.OpenLock();
        }

        void DisableLocks(Transform levelTransform)
        {
            Lock[] allLocks = levelTransform.GetComponentsInChildren<Lock>(true);
            foreach (Lock l in allLocks)
            {
                // Եթե սցենան նորից է բացվել, ուղղակի անջատում ենք անցած լեվելների կողպեքները
                if (l.locks != null) l.locks.SetActive(false);
            }

            Lock2[] allLocks2 = levelTransform.GetComponentsInChildren<Lock2>(true);
            foreach (Lock2 l2 in allLocks2)
            {
                if (l2.lock1 != null) l2.lock1.SetActive(false);
                if (l2.lock2 != null) l2.lock2.SetActive(false);
            }
        }

    }
    
}
