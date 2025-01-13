using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort_Manager : MonoBehaviour
{
    #region private 변수 
    private List<GameObject> targetList = new List<GameObject>();
    private Rigidbody targetRb;
    private WaitForSeconds waitTime = new WaitForSeconds(0.05f);
    private bool isChanging = false;
    #endregion

    #region public 변수 
    public int objNum = 100;
    public GameObject target;
    public Material[] changeMat = new Material[2];
    #endregion

    void Start()
    {
        for (int i = 0; i < objNum; i++)
        {
            targetList.Add(Instantiate(target));
            targetList[i].transform.position = new Vector3(i, Random.Range(0,25), 0);
            targetList[i].transform.localScale = new Vector3(1.0f, 1.0f + i, 1.0f);
        }
        StartCoroutine(Shuffle());
    }

   
    void Update()
    {
        if (isChanging == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Shuffle());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(Sellect());
            }
        }
    }

    private IEnumerator Shuffle()
    {
        isChanging = true;
        for (int i = 0; i < objNum; i++)
        {
            int changeNum = Random.Range(0, objNum - 1);

            GameObject temp = targetList[i];
            targetList[i] = targetList[changeNum];
            targetList[changeNum] = temp;

            GameObject[] changePosition = new GameObject[2] { targetList[i], targetList[changeNum] };
            foreach (GameObject target in changePosition)
            {
                if (target.TryGetComponent(out Renderer r))
                {
                    r.material = changeMat[1];
                }
            }
            Vector3 tempPosition = changePosition[1].transform.position;
            changePosition[1].transform.position = changePosition[0].transform.position;
            changePosition[0].transform.position = tempPosition;
            yield return waitTime;

            foreach (GameObject target in changePosition)
            {
                if (target.TryGetComponent(out Renderer r))
                {
                    r.material = changeMat[0];
                }
            }
        }
        isChanging = false;
    }


    private IEnumerator Sellect()
    {
        isChanging = true;
        for (int i = 0; i < objNum; i++)
        {
            GameObject Select_obj = targetList[i];
            if (Select_obj.TryGetComponent(out Renderer r))
            {
                r.material = changeMat[1];
            }
            for (int j =i-1; j >= 0; j--)
            {
                if (targetList[j+1].transform.localScale.y < targetList[j].transform.localScale.y)
                {
                    GameObject tempobj = targetList[j+1];
                    targetList[j+1] = targetList[j];
                    targetList[j] = tempobj;
                    
                }
            }



            GameObject[] changePosition = new GameObject[2] { targetList[i], Select_obj };
           
            yield return waitTime;
            Vector3 temp = changePosition[1].transform.position;
            changePosition[1].transform.position = changePosition[0].transform.position;
            changePosition[0].transform.position = temp;

            yield return new WaitForSeconds(1.0f);
            Debug.Log("한번");
            r.material = changeMat[0];
        }
        
        isChanging = false;
    }

}
