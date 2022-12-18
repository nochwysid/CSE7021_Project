using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class NFT : MonoBehaviour
{
    void Start()
    {
        // don't know what needs doing here ... yet
    }
 
    // to hold data after encryption
    private byte[] _3fcedc35a461afb;

    // raw data of the NFT, to allow later reference
    public byte[] Image { get; set; } // issues with namespaces, so this is raw form
    public string[] Data { get; set; }
    public string Description { get; set; }
    public byte[] byteName { get; set; }
    public byte[] byteAddress { get; set; }
    public byte[] byteTimeStamp { get; set; }
    public int[] dL = new int[4];


    // to hold encryption key for the NFT
    private readonly byte[] _5a453e29e7d9be5;

    public NFT(byte[] image, string[] data, byte[] encryptKey)
    {
        // store the plaintext data
        Image = image; // remember, this is 'bytes[]'
        Data = data;
        
        // convert to bytes
        byteName = Encoding.UTF8.GetBytes(data[0]);
        byteAddress = Encoding.UTF8.GetBytes(data[1]);
        byteTimeStamp = Encoding.UTF8.GetBytes(data[2]);
        
        dL[0] = image.Length;
        dL[1] = byteName.Length;
        dL[2] = byteAddress.Length; 
        dL[3] = byteTimeStamp.Length;
        // store encryption key
        _5a453e29e7d9be5 = encryptKey;

        // encrypt the data
        EncryptData();
    }

    private void EncryptData()
    {
        // symmetric algorithm, since encrypt/decrypt locally
        Aes encryptor = Aes.Create();
        encryptor.Key = _5a453e29e7d9be5;

        // Microsofts fondness for 'streams'
        using (MemoryStream ms = new MemoryStream())
        {
            // create a CryptoStream to encrypt the data
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                // write the raw byte data to the MamoryStream using CryptoStream
                cs.Write(Image, 0, dL[0]);
                cs.Write(byteName, dL[0] + 1, dL[1]);
                cs.Write(byteAddress, dL[1] + 1, dL[2]);
                cs.Write(byteTimeStamp, dL[2] + 1, dL[3]);
            }

            // store encrypted data 
            _3fcedc35a461afb = ms.ToArray();
        }
    }
    private void sellNFT(byte[] image, string[] newData, byte[] encryptionKey)
    {
        if(verifyOwnership(image, newData, encryptionKey))
        {
            /*
             * here is where we would make calls to external libraries. We would make a transaction,
             * and once verified, call transferOwnership(name, description);
             */
            //transferOwnership(name, description);
        }
    }

    private bool transferOwnership(string[] newData)
    {
        this.Data = newData;
        
        // convert new data to bytes for use in encryption
        byteName = Encoding.UTF8.GetBytes(newData[0]);
        byteAddress = Encoding.UTF8.GetBytes(newData[1]);
        byteTimeStamp = Encoding.UTF8.GetBytes(newData[2]);
        
        // set the lengths of the new data chunks
        dL[1] = byteName.Length;
        dL[2] = byteAddress.Length;
        dL[3] = byteTimeStamp.Length;
        
        // encrypt the new data
        EncryptData();

        // return true if the above steps successfully set object data to new owner's info
        return verifyOwnership(this.Image, this.Data, this._3fcedc35a461afb);
    }


    private bool verifyOwnership(byte[] img, string[] data, byte[] enKey)
    {
        Aes encryptor = Aes.Create();
        encryptor.Key = enKey;

        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                byte[] thisName = Encoding.UTF8.GetBytes(data[0]);
                byte[] thisAddress = Encoding.UTF8.GetBytes(data[1]);
                byte[] thisTimeStamp = Encoding.UTF8.GetBytes(data[2]);
                // to convert back 'string str = Encoding.UTF8.GetString(bytes);'
                // can substitue 'ASCII' for 'UTF8'

                //this info not store at the class or global level because it needs to be verified
                int a = img.Length;
                int b = a + thisName.Length + 1;
                int c = b + thisAddress.Length;
                int d = c + thisTimeStamp.Length;
                // write to the memory stream, data chunks aligned
                cs.Write(img, 0, a);
                cs.Write(thisName, a+1, b);
                cs.Write(thisAddress, b+1, c);
                cs.Write(thisTimeStamp, c+1, d);
            }

            // temporarily store the encrypted data to be compared
            byte[] verifyData = ms.ToArray();
            return(verifyData.Equals(this._3fcedc35a461afb));
        }
    }
}
