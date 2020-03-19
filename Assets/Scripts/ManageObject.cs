using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageObject : MonoBehaviour
{
    public List<GameObject> listObjectPool = new List<GameObject>();
    public int NumberObjectNeedCreate=10;
    public GameObject objectToPool;
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0;i<NumberObjectNeedCreate;i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            listObjectPool.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
