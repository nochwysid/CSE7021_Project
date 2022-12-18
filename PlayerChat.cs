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
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("PlayerChat script started. Double check that this was intentional.");
        pw = GetComponent<PhotonView>();
        if (pw.IsMine)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pw.IsMine)
        {
            movement();
        }
        
    }
    void movement()
    {
        bool tap = false;
        if (Input.touchSupported) { if (Input.touchCount > 0) { tap = true; } }
        if (Input.GetButtonDown("Fire1") || tap)
        {
            transform.position = transform.position + Camera.main.transform.forward * playerspeed * Time.deltaTime;

        }
        y = Camera.main.transform.localEulerAngles.y - transform.eulerAngles.y;
        transform.Rotate(0, y * 0.01F, 0);
        y = 0.0F;
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
