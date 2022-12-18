using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{ 
    public GameObject hands;
    public GameObject head;

    public GameObject ball;
    public GameObject ring_orn_5_gold;
    public GameObject ring_orn_5_fbx;
    public GameObject hat;
    public GameObject fbxGlasses1;
    public GameObject Steampunk_Goggles;
    bool inHand = false;
    bool onHead = false;
    public bool[] onPlayer = new bool[6];
    string[] names = new string[6];
    GameObject[] GOs = new GameObject[6];
    Vector3[] V3 = new Vector3[6];
    Transform[] Tran = new Transform[6];
    Collider[] Col = new Collider[6];
    Rigidbody[] RBs = new Rigidbody[6];

    // Start is called before the first frame update
    void Start()
    {
        GOs[0] = ball;
        GOs[1] = ring_orn_5_gold;
        GOs[2] = ring_orn_5_fbx;
        GOs[3] = hat;
        GOs[4] = fbxGlasses1;
        GOs[5] = Steampunk_Goggles;
        for(int i = 0; i < 6; i++)
        {
            V3[i]   = GOs[i].transform.position;
            Tran[i] = GOs[i].transform;
            Col[i]  = GOs[i].GetComponent<SphereCollider>();
            RBs[i]  = GOs[i].GetComponent<Rigidbody>();
            onPlayer[i] = false;
        }
        Debug.Log("Started");
         //ballPos = ball.transform.position;
         //ballcol = ball.GetComponent<SphereCollider>();
         //ballbod = ball.GetComponent<Rigidbody>();
    }
    public void putback()
    {
        for (int i = 0; i < 6; i++)
        { GOs[i].transform.SetParent(null); GOs[i].transform.localPosition = V3[i]; onPlayer[i] = false; }
    }

    public void getobjectID(GameObject obj)
    {
        bool fired = Input.GetButtonDown("Fire1");
        bool tap = false;
        if (Input.touchSupported) { if (Input.touchCount > 0) { tap = true; } }
        Debug.Log("GetButtonDown: " + fired + ", touchSupported: " + tap);
        if (fired || tap)
        {   putback();
            // switch statement didn't work here
                if (obj.name == GOs[0].name)
                {
        Debug.Log("this name: " + this.name + ", object 0 name: " + obj.name);
                    if (!onPlayer[0])
                    {
                        //GOs[0].transform.SetParent(hands.transform);
                        //GOs[0].transform.localPosition = new Vector3(0, -0.35f, 0.5f);
                        obj.transform.SetParent(hands.transform);
                        obj.transform.localPosition = new Vector3(0, -0.35f, 0.5f);
                    onPlayer[0] = true;
                    }
                    else if (onPlayer[0])
                    {
                        this.GetComponent<PlayerGrab>().enabled = false;
                        //GOs[0].transform.SetParent(null);
                        //GOs[0].transform.localPosition = V3[0];
                        obj.transform.SetParent(null);
                        obj.transform.localPosition = V3[0];
                    onPlayer[0] = false;
                    }
                }
                else if (obj.name == GOs[1].name)
                {
        Debug.Log("this name: " + this.name + ", object 1 name: " + obj.name);
                    if (!onPlayer[1])
                    {
                        GOs[1].transform.SetParent(hands.transform);
                        GOs[1].transform.localPosition = new Vector3(0, -0.35f, 0.5f);
                        onPlayer[1] = true;
                    }
                    else if (onPlayer[1])
                    {
                        this.GetComponent<PlayerGrab>().enabled = false;
                        GOs[1].transform.SetParent(null);
                        GOs[1].transform.localPosition = V3[1];
                        onPlayer[1] = false;
                    }
                }
                else if (obj.name == GOs[2].name)
                {
        Debug.Log("this name: " + this.name + ", object 2 name: " + obj.name);
                    if (!onPlayer[2])
                    {
                        GOs[2].transform.SetParent(hands.transform);
                        GOs[2].transform.localPosition = new Vector3(0, -0.35f, 0.5f);
                        onPlayer[2] = true;
                    }
                    else if (onPlayer[2])
                    {
                        this.GetComponent<PlayerGrab>().enabled = false;
                        GOs[2].transform.SetParent(null);
                        GOs[2].transform.localPosition = V3[2];
                        onPlayer[2] = false;
                    }
                }

                else if (obj.name == GOs[3].name)
                {
        Debug.Log("this name: " + this.name + ", object 3 name: " + obj.name);
                    if (!onPlayer[3])
                    {
                        GOs[3].transform.SetParent(head.transform);
                        GOs[3].transform.localPosition = new Vector3(0, -1.65f, 0.0f);
                        onPlayer[3] = true;
                    }
                    else if (onPlayer[3])
                    {
                        this.GetComponent<PlayerGrab>().enabled = false;
                        GOs[3].transform.SetParent(null);
                        GOs[3].transform.localPosition = V3[3];
                        onPlayer[3] = false;
                    }
                }
                else if (obj.name == GOs[4].name)
                {
        Debug.Log("this name: " + this.name + ", object 4 name: " + obj.name);
                    if (!onPlayer[4])
                    {
                        GOs[4].transform.SetParent(head.transform);
                        GOs[4].transform.localPosition = new Vector3(0, -1.65f, 0.15f);
                        onPlayer[4] = true;
                    }
                    else if (onPlayer[4])
                    {
                        this.GetComponent<PlayerGrab>().enabled = false;
                        GOs[4].transform.SetParent(null);
                        GOs[4].transform.localPosition = V3[4];
                        onPlayer[4] = false;
                    }
                }
                else if (obj.name == GOs[5].name)
                {
        Debug.Log("this name: " + this.name + ", object 5 name: " + obj.name);
                    if (!onPlayer[5])
                    {
                        GOs[5].transform.SetParent(head.transform);
                        GOs[5].transform.localPosition = new Vector3(0, -1.65f, 0.15f);
                        onPlayer[5] = true;
                    }
                    else if (onPlayer[5])
                    {
                        this.GetComponent<PlayerGrab>().enabled = false;
                        GOs[5].transform.SetParent(null);
                        GOs[5].transform.localPosition = V3[5];
                        onPlayer[5] = false;
                    }
                }
            //}
        }
    }
}
