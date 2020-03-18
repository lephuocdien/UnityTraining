using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageObject : MonoBehaviour
{
    public List<GameObject> listObject = new List<GameObject>();
    public int NumberObjectNeedCreate=10;
    public GameObject[] objectRespawn=new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0;i< NumberObjectNeedCreate; i++)
        {
            var objecttemp = objectRespawn[Random.Range(0,objectRespawn.Length)];
            float x = Random.Range(-8,8);
            float z = Random.Range(-8, 8);
            Vector3 vtemp = new Vector3(x, 0.0f, z);
            Instantiate(objecttemp, vtemp, Quaternion.identity);
            listObject.Add(objecttemp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
