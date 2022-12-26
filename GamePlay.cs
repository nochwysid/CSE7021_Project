using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.IO;
using System;
using System.Text;
using System.Xml.Linq;

public class GamePlay : MonoBehaviour
{
    /// <summary>
    /// Logic for playing the game. Should all logic be here, or distributed across the parts? 
    /// Should votes be stored in the items, or collected here?
    /// </summary>
    public GameObject item;
    public byte[] image;
    private byte[] encryptionKey = GenerateEncryptionKey();
    public string[] mydata;
    void Start()
    {
        //item.GetPhotonView().transform.rotation.eulerAngles.y = item.GetComponentInParent<Camera>().transform.rotation.eulerAngles.y
        mydata = new string[4];
        mydata[0] = "";
        mydata[1] = "";
        mydata[2] = "";
        mydata[3] = "";
        
        Material material = item.GetComponent<Material>();
        int crc = material.ComputeCRC();
        Debug.Log("material crc: " + crc.ToString() + ", item name: " + item.name);

    }

    //static byte[] image = (byte[])item.GetComponent<Material>(); // load the image from a file or other source

    // populate the data array
    public void generateWinnerData(string name, string address, string dtime)
    {
        mydata[0] = name;
        mydata[1] = address;
        mydata[2] = dtime;
    }
    // overriding to allow extra info, if desired
    public void generateWinnerData(string name, string address, string dtime, string other)
    {
        mydata[0] = name;
        mydata[1] = address;
        mydata[2] = dtime;
        mydata[3] = other;
    }
    private NFT CreatNew(byte[] image, string[] mydata, byte[] enKey)
    {
        for (int i = 0; i < mydata.Length-1; i++)
        {
            if (mydata[i] == "") { Debug.LogError("NFT requires name, crypto wallet address, and timestamp"); return null;}
        }
        
        if(image == null)
        {Debug.LogError("NFT requires as the basis, and thus, must not be null."); return null;}
        
        return new NFT(image, mydata, enKey);
    }


    private static byte[] GenerateEncryptionKey()
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] key = new byte[32]; // 32 bytes (256 bits) for AES key
                rng.GetBytes(key);
            return key;
        }
    }
}