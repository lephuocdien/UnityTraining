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
    List<Vector3> positionPrefabs = new List<Vector3>();
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        ShowSetCounttext();
        wintext.text = "";
    }
    public void MovePlayerToObject(Vector3 obj, float step)
    {
        transform.position = Vector3.MoveTowards(transform.position, obj, step);
    }
    void GenertateObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Press mouse button");
            RaycastHit hit;
            Ray ray = nonVRCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("hit ttttttttt" + hit.transform.name);
                if (hit.transform.name == "Ground")
                {
                    Instantiate(prefab, hit.point, Quaternion.identity);
                    positionPrefabs.Add(hit.point);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        // float movehorizontal = Input.GetAxis("Horizontal");
        //  float movevertical = Input.GetAxis("Vertical");
        // Vector3 movement = new Vector3(movehorizontal,0.0f,movevertical);
        // rb.AddForce(movement * speed);
        GenertateObject();
        ///move player
        Vector3 movement = new Vector3(positionPrefabs[0].x, positionPrefabs[0].y, positionPrefabs[0].z);


        //transform.position = Vector3.MoveTowards(transform.position, positionPrefabs[0], step);
        foreach (var item in positionPrefabs)
        {
            MovePlayerToObject(item, step);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
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
