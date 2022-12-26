using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Pun.UtilityScripts;

using ChatSDK;
using ChatSDK.MessageBody;

using UnityEngine;

//
public class PlayerChat : MonoBehaviour
{
    PhotonView pw;
    float y = 0.0F;
    float playerspeed = 0.3F;
    Quaternion lastDirection = Quaternion.identity;
    Vector3 lastPosition = Vector3.zero;
    float m_Angle = 0F;
    float m_Distance = 0F;
    private float n_Angle = 0F;
    private float n_Distance = 0F;

    void Start()
    {
        Debug.LogWarning("PlayerChat script started. Double check that this was intentional.");
        pw = GetComponent<PhotonView>();
        if (pw.IsMine)
        {
            Debug.Log("Your name is "+ this.name);
            
            //GetComponent<Renderer>().material.color = Color.gray;
        }
        else 
        {
            Debug.Log("Name of other player is "+ pw.name);
            //GetComponent<GvrReticlePointer>().enabled = false;
            GetComponent<Camera>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update fucntion in PlayerChat is being called");
        if (pw.IsMine)
        {
            //movement();
        }
        
    }
    void movement()
    {
        var tr = transform;

        DeviceOrientation orient = Input.deviceOrientation;
        int ocompare = orient.CompareTo(orient);
        //Debug.Log("DeviceOrientation: " + orient.ToString() + ", : " + ((float)orient).ToString()+", ocompare: "+ocompare.ToString());
        Vector3 velocity = Input.acceleration;
        velocity.y = 0;
        Compass ompass = Input.compass;
        Vector3 direct = ompass.rawVector;
        //Debug.Log("DeviceOrientation: " + d
        Debug.Log("movement fucntion in PlayerChat is being called");
        bool tap = false;
        if (Input.touchSupported) { if (Input.touchCount > 0) { tap = true; } }
        if (Input.GetButtonDown("Fire1") || tap)
        {
            m_Angle = Quaternion.Angle(lastDirection, this.GetComponent<Transform>().localRotation);
            m_Distance = Vector3.Distance(lastPosition, this.GetComponent<Transform>().localPosition);
            //transform.position = transform.position + Camera.main.transform.forward * playerspeed * Time.deltaTime;

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, this.GetComponent<Transform>().localPosition, this.m_Distance * Time.deltaTime * PhotonNetwork.SerializationRate);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, this.GetComponent<Transform>().localRotation, this.m_Angle * Time.deltaTime * PhotonNetwork.SerializationRate);

            lastPosition = transform.localPosition;
            lastDirection = transform.localRotation;

        }else if (Input.acceleration.magnitude > 0.05f)
        {
            n_Angle = Vector3.Angle(Camera.main.transform.position, this.GetComponent<Transform>().position);
            n_Distance = Vector3.Distance(Camera.main.transform.position, this.GetComponent<Transform>().position);

            if (Input.acceleration.magnitude > 1.0f)
            {
                playerspeed = Input.acceleration.sqrMagnitude;
            }
            else
            {
                playerspeed = Input.acceleration.magnitude;
            }
            direct = Input.compass.rawVector;
            direct.y = 0;

            this.GetComponent<CharacterController>().enabled = false;
            tr.position = this.GetComponent<Transform>().position + direct * playerspeed * Time.deltaTime * 100;
            tr.position.Set(tr.position.x, 0f, tr.position.z);
            this.GetComponent<CharacterController>().enabled = true;

            tr.rotation = Quaternion.RotateTowards(tr.rotation, this.GetComponent<Transform>().rotation, this.n_Angle * Time.deltaTime * playerspeed);
        }
    }
    //agora SDK
      //Options options = new Options(appkey);
      //hub = new ChatSDK.ConnectionHub();
      //ChatSDK.ConnectionListener
      //  ChatSDK.IClient.
      //SDKClient 
       // .Instance.InitWithOptions(options);
       
      //SDKClient.Instance.LoginWithAgoraToken(username, token, null);
      //Message msg = Message.CreateTextSendMessage(toChatUsername, content);
      //SDKClient.Instance.ChatManager.SendMessage(ref msg, null);
    
}
