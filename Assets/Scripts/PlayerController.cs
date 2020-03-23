﻿using System.Collections;
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

   // public bool h = false;
    private bool end = false;

    //public bool playparticle = false;


    public  GameObject particleExplosive;
    public GameObject particleDie;


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
        transform.LookAt(obj);
        transform.position = Vector3.MoveTowards(transform.position, obj, step);
       
        transform.GetComponent<Animator>().SetTrigger("isWalking");
       
        float distance = Vector3.Distance(transform.position, obj);
        
        if (end==true)
        {
           // Debug.Log("MovePlayerToObject end" + obj);
            //remove this item
            positionPrefabs.RemoveAt(index);
            end = false;
            currentPosNeedGoto = Vector3.zero;
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
             
                if (hit.transform.name== "Ground")
                {
                    
                    GameObject abc = prefab.Spawn(hit.point);
                                    
                 
                    int j = Random.Range(1, 4);
                    if (j == 1)
                        abc.transform.GetChild(0).GetComponent<Animator>().SetInteger("colorofmeeeee", 1);
                    else if (j == 2)
                        abc.transform.GetChild(0).GetComponent<Animator>().SetInteger("colorofmeeeee", 2);
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

    IEnumerator DoAnimation()
    {
        //animation.CrossFade("MyAnimation");
        yield return new WaitForSeconds(2f); // wait for two seconds.
        Debug.Log("This happens 2 seconds later. Tada.");
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


                Debug.Log("Meet red cube ");
                //  Instantiate(particleDie, other.transform.position, Quaternion.identity);

                //Animator animate = transform.GetComponent<Animator>();
               // animate.GetCurrentAnimatorStateInfo(0).IsTag;
               // transform.GetComponent<Animator>().SetInteger("CharacterDie", 1);//red
                // transform.GetComponent<Animator>().ResetTrigger("emptypickup");

               
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
