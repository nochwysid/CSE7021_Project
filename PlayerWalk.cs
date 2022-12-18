using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Google.XR.CardBoard;


public class PlayerWalk : MonoBehaviour
{
    public int playerspeed;
    float y = 0.0F;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerWalk script started");
        // CapsuleCollider[] caps = GetComponents<CapsuleCollider>();
        // for(int i = 0; i < coliderList.Length; i++)
        // {
        //     caps[i].isTrigger = true;
        //     caps[i].enabled = true;
        //     caps[i].height = 7;
        // }
        
    }

    // Update is called once per frame
    void Update()
    {
        bool tap = false;
        if (Input.touchSupported) { if (Input.touchCount > 0) { tap = true; } }
        //Touch inputStruct = Input.GetTouch(0);
        //if (Input.GetButtonDown("Fire1") || inputStruct.tapCount > 0)
        if (Input.GetButton("Fire1") || tap)
        {
            transform.position = transform.position + Camera.main.transform.forward * playerspeed * Time.deltaTime;
            //Debug.Log("transform.position: " + transform.position.ToString()+ "transform.rotation: " + transform.rotation.ToString() );  
            //transform.rotation = transform.rotation + Camera.main.transform.rotation;
        
        }
        y = Camera.main.transform.localEulerAngles.y - transform.eulerAngles.y;
        //Debug.Log("angle: "+y.ToString());
        //y = GvrReticlePointer.transform.eulerAngles.y - transform.eulerAngles.y;
        //y = Camera.main.transform.eulerAngles.y - transform.eulerAngles.y;
        transform.Rotate(0,y*0.01F,0);
        y = 0.0F;
    }
}
