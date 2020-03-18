using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb ;
    public float speed;
    private int count;
    public Text counttext;
    public Text wintext;
    public Camera nonVRCamera;
    public GameObject prefab;
    // ArrayList positionPrefabs = new ArrayList();
    //List<Vector3> positionPrefabs = new List<Vector3>();

    List<Vector3> positionPrefabs = new List<Vector3>();
   

    private bool end = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        ShowSetCounttext();
        wintext.text = "";
    }
    public void MovePlayerToObject(Vector3 obj, int index,float step)
    {
        
        transform.position = Vector3.MoveTowards(transform.position, obj, step);
        float distance = Vector3.Distance(transform.position, obj);
        if(distance <0.7)
        {
            end = true;
        }
      
        if (end==true)
        {
            Debug.Log("MovePlayerToObject end" + obj);
            //remove this item
            positionPrefabs.RemoveAt(index);
            end = false;
        }
    }
    void GenertateObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            RaycastHit hit;
            Ray ray = nonVRCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
             
                if (hit.transform.name == "Ground")
                {
                    Instantiate(prefab, hit.point, Quaternion.identity);
                    Debug.Log("Add positon " + hit.point);
                    positionPrefabs.Add(hit.point);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
       
        GenertateObject();
       
        //Vector3 movement = new Vector3(positionPrefabs[0].x, positionPrefabs[0].y, positionPrefabs[0].z);


        //transform.position = Vector3.MoveTowards(transform.position, positionPrefabs[0], step);
        if (positionPrefabs.Count > 0)
        {
            Vector3 newpos = positionPrefabs[0];
            MovePlayerToObject(newpos, 0, step);
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Pick Up"))
        {
            Debug.Log("Destroy"+ other.gameObject.transform.position);
            //other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            count++;
            Vector3 objp = other.transform.position;
            positionPrefabs.Remove(objp);
            ShowSetCounttext();
        }
    }
    void ShowSetCounttext()
    {
        counttext.text = "Count:" + count;
        if(count>=12)
        {
            wintext.text = "You Win!";
        }
    }
}
