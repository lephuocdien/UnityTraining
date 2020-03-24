using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public   GameObject prefab;

    private Vector3 currentPosNeedGoto;
    List<Vector3> positionPrefabs = new List<Vector3>();

   // public bool h = false;
    private bool end = false;

    //public bool playparticle = false;


    public GameObject particleExplosive;
    public   GameObject particleDie;

    private bool ontriggerE = false;
    public Image greenImage;
    public Text btn;
    private float TimePassed = 0;
    public static PlayerController _instane;
    private bool _pausegame = true;

    public static PlayerController getInstance()
    {
        if (_instane == null)
        {
            Debug.Log("erorrrrrrrrrrrrrrrr");
        }
        

        return _instane;
    }
    public void Awake()
    {
        if (_instane == null)
            _instane = this;
    }
    public void PlayGame()
    {
        _pausegame = false;
        gameObject.GetComponent<Animator>().enabled = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        ShowSetCounttext();
        wintext.text = "";
        greenImage.GetComponent<Image>().fillAmount = 1.0f;
        gameObject.GetComponent<Animator>().enabled = false;

      
        /*       
        GameObject gameObjectArray = GameObject.FindGameObjectWithTag("optionmenu");
        gameObjectArray.SetActive(false);

        GameObject gameObjectArray1 = GameObject.FindGameObjectWithTag("playgame");
        gameObjectArray1.SetActive(false);
        */


    }
    public  void pausegame()
    {
        


        _pausegame = true;
        gameObject.GetComponent<Animator>().enabled = false;

       
    }
    public void resumeGame()
    {
        _pausegame = false;
        gameObject.GetComponent<Animator>().enabled = true;
    }
    public void MovePlayerToObject(Vector3 obj, int index,float step)
    {

        //
        Material aa = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
        ChangeRenderMode(aa, BlendMode.Opaque);
        for (float ft = 0; ft <= 1; ft += 0.1f)
        {
            Color c = aa.color;
            c.a = ft;
            aa.color = c;
        }
        //
        currentPosNeedGoto = obj;
        transform.LookAt(obj,Vector3.up);
         transform.position = Vector3.MoveTowards(transform.position, obj, step);
       // transform.localPosition = Vector3.Lerp(transform.position, obj, 1);

        transform.GetComponent<Animator>().SetTrigger("isWalking");
       
        float distance = Vector3.Distance(transform.position, obj);
        
        if (end==true)
        {
            // Debug.Log("MovePlayerToObject end" + obj);
            //remove this item
            ontriggerE = false;
            positionPrefabs.RemoveAt(index);
            end = false;
            currentPosNeedGoto = Vector3.zero;
        }
    }
    void GenertateObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
                return;

            }

            RaycastHit hit;
            Ray ray = nonVRCamera.ScreenPointToRay(Input.mousePosition);
           
            if (Physics.Raycast(ray, out hit))
            {
             
                if (hit.transform.name== "Ground")
                {
                    
                    GameObject abc = prefab.Spawn(hit.point);
                                    
                 
                    int j = Random.Range(1, 4);
                    if (j == 1)
                        abc.transform.GetChild(0).GetComponent<Animator>().SetInteger("colorofmeeeee", 1);
                    else if (j == 2)
                        abc.transform.GetChild(0).GetComponent<Animator>().SetInteger("colorofmeeeee", 1);
                    else
                        abc.transform.GetChild(0).GetComponent<Animator>().SetInteger("colorofmeeeee", 3);
                    positionPrefabs.Add(hit.point);
                }
            }
            

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!_pausegame)
        {
            float t = Time.time - TimePassed;
            btn.text = t.ToString();
            //
            //if (playparticle)
            //    gameObject.GetComponentInParent<ParticleSystem>().Play();
            //else
            //    gameObject.GetComponentInParent<ParticleSystem>().Stop();
            //
            // transform.GetChild(0).GetComponent<Animator>().SetTrigger("ChangeColorBlue");

            if (GameObject.FindGameObjectsWithTag("Pick Up").Length > 0)
            {
                transform.GetComponent<Animator>().ResetTrigger("emptypickup");
                //transform.GetComponent<Animator>().enabled = false;
            }
            else
            {
                //this mean use idle state
                transform.GetComponent<Animator>().SetTrigger("emptypickup");
                transform.GetComponent<Animator>().ResetTrigger("isWalking");

            }
            //

            float step = speed * Time.deltaTime;
            //generate position of object after click mouse
            GenertateObject();

            if (positionPrefabs.Count > 0)
            {
                Vector3 newpos = positionPrefabs[0];

                if (ontriggerE == false)
                {
                    MovePlayerToObject(newpos, 0, step);
                }
                else
                {
                    //coroutine here
                    StartCoroutine(ActionAfterSecond(3.0f, newpos, 0, step));

                    //  MovePlayerToObject(newpos, 0, step);
                }

            }
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
    /// <summary>
    /// /////////////////////////////////////////////////
    /// </summary>
     public enum BlendMode
     {
         Opaque,
         Cutout,
         Fade,
         Transparent
     }
    public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
        }

    }
    IEnumerator ActionAfterSecond(float second, Vector3 obj, int index, float step)
    {
        yield return new WaitForSeconds(second);
       // Debug.Log("Hii :" + Time.time);
        ChangeAlphaToValue(1.0f,obj,index,step);
    }
    private void ChangeAlphaToValue(float value, Vector3 obj, int index, float step)
    {
        Instantiate(particleDie, transform.localPosition, Quaternion.identity);
        transform.GetComponent<Animator>().SetTrigger("hoisinh");
        transform.GetComponent<Animator>().SetInteger("CharacterDie", 0);
        transform.LookAt(obj);
        MovePlayerToObject(obj, 0, step);

        Material aa = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
        ChangeRenderMode(aa, BlendMode.Transparent);
        for (float ft = value; ft >= 0; ft -= 0.1f)
        {
            Color c = aa.color;
            c.a = ft;
            aa.color = c;
        }
    }
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {

      
        if (other.gameObject.CompareTag("Pick Up"))
        {
            

            transform.GetComponent<Animator>().SetTrigger("isFight");
            other.gameObject.transform.GetComponent<Animator>().SetTrigger("PickUpDie");           
            count++;
            // other.gameObject.tran.GetComponent<Animator>().
            int vlcolor = other.gameObject.transform.GetComponent<Animator>().GetInteger("colorofmeeeee");
            if(vlcolor==1)//mean red cube
            {
              
                Instantiate(particleExplosive, other.transform.position, Quaternion.identity);

                ontriggerE = true;
                transform.GetComponent<Animator>().SetInteger("CharacterDie",1);
                Debug.Log("Meet red cube ");

                greenImage.GetComponent<Image>().fillAmount =((10- count) / 10.0f);
                


                //  Instantiate(particleDie, other.transform.position, Quaternion.identity);

                //Animator animate = transform.GetComponent<Animator>();
                // animate.GetCurrentAnimatorStateInfo(0).IsTag;
                // transform.GetComponent<Animator>().SetInteger("CharacterDie", 1);//red
                // transform.GetComponent<Animator>().ResetTrigger("emptypickup");
                //  StartCoroutine(ActionAfterSecond(3));
                //transform.localPosition = Vector3.zero;
                //Instantiate(particleDie, transform.localPosition, Quaternion.identity);
                // StartCoroutine(ActionAfterSecond(3));



            }


            Vector3 objp = other.gameObject.transform.position;
            if (objp.x == currentPosNeedGoto.x && objp.z==currentPosNeedGoto.z)
            {
              
               
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
