using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort_Manager : MonoBehaviour
{
    #region private 변수 
    public List<GameObject> targetList = new List<GameObject>();
    private Rigidbody targetRb;
    private WaitForSeconds waitTime = new WaitForSeconds(0.05f);
    private bool isChanging = false;
    private Renderer targetRenderer;
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
            targetList[i].transform.position = new Vector3(i, Random.Range(0, 30), 0);
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
                Generate_co(Shuffle());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Generate_co(Sellect());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Generate_co(Insert());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Generate_co(Bubble());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Generate_co(MergeSort_co(0, objNum - 1));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Generate_co(QuickSort(0, objNum - 1));
            }
        }
    }

    private void Generate_co(IEnumerator co)
    {
        StartCoroutine(Generat(co));
    }
    private IEnumerator Generat(IEnumerator co)
    {
        isChanging = true;
        yield return StartCoroutine(co);
        yield return StartCoroutine(resetPosition());
        isChanging = false;
    }
    private IEnumerator resetPosition()
    {
        for (int i = 0; i < objNum; i++)
        {
            targetList[i].transform.position = new Vector3(targetList[i].transform.position.x, targetList[i].transform.localScale.y, 0);
            if (targetList[i].TryGetComponent(out Cube t))
            {
                t.SetKinematic(false);
            }
        }
        yield return null;
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
        for (int i = 0; i < objNum-1; i++)
        {
            int changeNum = i;
            GameObject Select_obj = targetList[i];
            if (targetList[i].TryGetComponent(out Renderer r))
            {
                r.material = changeMat[1];
            }
            targetRenderer = Select_obj.GetComponent<Renderer>();
            for (int j = i+1; j < objNum; j++)
            {
                if (Select_obj.transform.localScale.y > targetList[j].transform.localScale.y)
                {
                    if (Select_obj != targetList[i])
                    {
                        targetRenderer.material = changeMat[0];
                    }
                    Select_obj = targetList[j];
                    changeNum = j;
                    if (Select_obj.TryGetComponent(out Renderer t))
                    {
                        targetRenderer = t;
                    }
                    targetRenderer.material = changeMat[2];
                }
            yield return waitTime;
            }
            GameObject tempobj = targetList[i];
            targetList[i] = targetList[changeNum];
            targetList[changeNum] = tempobj;

            GameObject[] changePosition = new GameObject[2] { targetList[i], targetList[changeNum] };

            Vector3 temp = changePosition[1].transform.position;
            changePosition[1].transform.position = changePosition[0].transform.position;
            changePosition[0].transform.position = temp;
            r.material = changeMat[0];
            targetRenderer.material = changeMat[0];

        }
    }


    private IEnumerator Insert()
    {
        for (int i = 1; i < objNum-1; i++)
        {
            
            for(int j = i-1; j>=0; j--)
            {
                if (targetList[j].transform.localScale.y > targetList[j + 1].transform.localScale.y)
                {
                    GameObject temp = targetList[j + 1];
                    targetList[j + 1] = targetList[j];
                    targetList[j] = temp;

                    GameObject[] changePosition = new GameObject[2] { targetList[j], targetList[j+1] };

                    Vector3 tempPos = changePosition[0].transform.position;
                    changePosition[0].transform.position = changePosition[1].transform.position;
                    changePosition[1].transform.position = tempPos;
                    yield return waitTime;
                }
            }
        }
    }

    private IEnumerator Bubble()
    {
        for (int i = objNum-1; i >0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                GameObject[] changePosition = new GameObject[2] { targetList[j], targetList[j + 1] };

                foreach (GameObject Sellect in changePosition)
                {
                    if (Sellect.TryGetComponent(out Renderer r))
                    {
                        r.material = changeMat[1];
                    }
                }
                if (targetList[j + 1].transform.localScale.y < targetList[j].transform.localScale.y)
                {
                    GameObject temp = targetList[j + 1];
                    targetList[j + 1] = targetList[j];
                    targetList[j] = temp;


                    Vector3 tempPos = changePosition[0].transform.position;
                    changePosition[0].transform.position = changePosition[1].transform.position;
                    changePosition[1].transform.position = tempPos;
                    yield return waitTime;
                    foreach (GameObject Sellect in changePosition)
                    {
                        if (Sellect.TryGetComponent(out Renderer r))
                        {
                            r.material = changeMat[2];
                        }
                    }
                }
                    yield return waitTime;
                foreach (GameObject Sellect in changePosition)
                {
                    if (Sellect.TryGetComponent(out Renderer r))
                    {
                        r.material = changeMat[0];
                  }
                }
            }

        }
    }


    private IEnumerator MergeSort_co(int start, int end)
    {

        if (start < end)
        { 
            int mid = (start + end) / 2;

            yield return StartCoroutine(MergeSort_co(start, mid));
            yield return StartCoroutine(MergeSort_co(mid+1, end));
            yield return StartCoroutine(Merge(start,end,mid));
        }
    }

    private IEnumerator Merge(int start, int end, int mid)
    {
        WaitForSeconds wfs = new WaitForSeconds(0.01f);
        List<GameObject> tempList = new List<GameObject>();
        int i = start;
        int j = mid + 1;
        int copy = 0;
        int pivot = start;

        while (i <= mid && j <= end)
        {
            if (targetList[i].transform.localScale.y < targetList[j].transform.localScale.y)
            {
                tempList.Add(targetList[i++]);
                pivot++;
            }
            else if (targetList[i].transform.localScale.y > targetList[j].transform.localScale.y)
            {
                tempList.Add(targetList[j++]);
                pivot++;
            }
        }

        while (i <= mid)
        {
            tempList.Add(targetList[i++]);
            pivot++;
        }
        while (j <= end)
        {
            tempList.Add(targetList[j++]);
            pivot++;
        }

        for (int k = start; k <= end; k++)
        {
            targetList[k] = tempList[copy++];
            targetList[k].transform.position =
                Vector3.zero + Vector3.right * (k-1);
            if (targetList[k].TryGetComponent(out Renderer t))
            {
                t.material = changeMat[2];
                yield return wfs;
                t.material = changeMat[0];
            }
        }


    }

    private IEnumerator QuickSort(int start, int end)
    {
        int pivot = (int)targetList[start].transform.localScale.y;
        int sort_start = start;
        int sort_end = end;

        if (targetList[sort_start].TryGetComponent(out Renderer mat_04))
        {
            mat_04.material = changeMat[3];
        }
        while (start < end)
        {
            while (pivot <= (int)targetList[end].transform.localScale.y && start < end)
            {
                end--;
            }
            if(start > end )
            {
                break;
            }
            
            while (pivot >= (int)targetList[start].transform.localScale.y && start < end)
            {
                start++;
            }
            if (start > end)
            {
                break;
            }
            if (targetList[start].TryGetComponent(out Renderer mat_01))
            {
                mat_01.material = changeMat[2];
            }
            if (targetList[end].TryGetComponent(out Renderer mat_02))
            {
                mat_02.material = changeMat[2];
            }

            GameObject tempobj_1 = targetList[start];
            targetList[start] = targetList[end];
            targetList[end] = tempobj_1;

            Vector3 pos_01 = targetList[start].transform.position;
            targetList[start].transform.position = targetList[end].transform.position;
            targetList[end].transform.position = pos_01;

            yield return waitTime;

            mat_01.material = changeMat[0];
            mat_02.material = changeMat[0];

        }

 
      
        GameObject tempobj_2 = targetList[start];
        targetList[start] = targetList[sort_start];
        targetList[sort_start] = tempobj_2;

        Vector3 pos_02 = targetList[start].transform.position;
        targetList[start].transform.position = targetList[sort_start].transform.position;
        targetList[sort_start].transform.position = pos_02;

        yield return waitTime;

        mat_04.material = changeMat[0];

        if (sort_start < start)
        {
            yield return StartCoroutine(QuickSort(sort_start, start - 1));
        }
        if (sort_end > end)
        {
            yield return StartCoroutine(QuickSort(start + 1 , sort_end));
        }
    }

}
