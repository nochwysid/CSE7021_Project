using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    private string adress;
    private byte[] enKey;
    void Start()
    {
        if(adress == null)
        {
            setAdress();
        }
        Aes encryptor = Aes.Create();
        encryptor.GenerateKey();
        enKey = encryptor.Key;
    }

    // Update is called once per frame
    public string getAdress()
    {
        // only here to control access, instead of publicly visible property
        return adress;
    }
    public byte[] getEnKey()
    {
        // only here to control access, instead of publicly visible property
        return enKey;
    }
    private void setAdress()
    {
        if (true)
        {
            adress = "79f6cdf6d70e3098424b0f81cf562e58f78a7720a7df81457184ed46403f8654";
        }
        else
        {
            adress = "79f6cdf6d70e3098424b0f81cf562e58f78a7720a7df81457184ed46403f8654";
        }
    }
}
