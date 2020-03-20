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

    private Vector3 currentPosNeedGoto;
    List<Vector3> positionPrefabs = new List<Vector3>();

    public bool h = false;
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
        currentPosNeedGoto = obj;
        //  transform.position = Vector3.MoveTowards(transform.position, obj, step);
       
        transform.GetComponent<Animator>().SetTrigger("isWalking");
        transform.LookAt(obj);
        float distance = Vector3.Distance(transform.position, obj);
        
        if (end==true)
        {
           // Debug.Log("MovePlayerToObject end" + obj);
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
                    prefab.Spawn(hit.point);
                    //Instantiate(prefab, hit.point, Quaternion.identity);
                    //Debug.Log("Add positon " + hit.point);
                    positionPrefabs.Add(hit.point);
                }
            }
            

        }
    }
    // Update is called once per frame
    void Update()
    {
        //
       /*
        if(GameObject.FindGameObjectsWithTag("Pick Up").Length > 0)
        {
            //transform.GetChild(0).GetComponent<Animator>().enabled = false;
        }
        else
        {
           // transform.GetChild(0).GetComponent<Animator>().enabled = true;
        } 
        //
        */
        float step = speed * Time.deltaTime;
       //generate position of object after click mouse
        GenertateObject();
       
        //Vector3 movement = new Vector3(positionPrefabs[0].x, positionPrefabs[0].y, positionPrefabs[0].z);


        //transform.position = Vector3.MoveTowards(transform.position, positionPrefabs[0], step);
        ///get position of first object
        if (positionPrefabs.Count > 0)
        {
            Vector3 newpos = positionPrefabs[0];
            ///function to move player to next position
            MovePlayerToObject(newpos, 0, step);
            
        }
    }
    private int findindex(Vector3 ob)
    {
        int vt = -1;
        for (int i = 0; i < positionPrefabs.Count; i++)
        {
            Vector3 temp = positionPrefabs[i];
            if (temp.x == ob.x && temp.z == ob.z)
            {
                vt = i;
                break;
            }
        }
        return vt;
    }
    private void OnTriggerEnter(Collider other)
    {

      
        if (other.gameObject.CompareTag("Pick Up"))
        {
            // Debug.Log("Destroy"+ other.gameObject.transform.position);
            //other.gameObject.SetActive(false);

            //other.gameObject.transform.parent.gameObject.SetActive(false);
            other.gameObject.GetComponent<Animator>().SetTrigger("PickUpDie");

            // other.gameObject.transform.parent.gameObject.Kill();
            count++;
            ///may be set end=true here, but have a bug.
            

            Vector3 objp = other.gameObject.transform.position;
            if (objp.x == currentPosNeedGoto.x && objp.z==currentPosNeedGoto.z)
            {
                h = true;
               // Debug.Log("11111111111111111");
                end = true;
            }

           UnityEngine.Vector3 objpremove = new Vector3(objp.x, 0.0f, objp.z);
            int j= findindex(objpremove);
            if (j > 0)
            {
               // Debug.Log("Remove at " + j);
                positionPrefabs.RemoveAt(j);
            }
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
