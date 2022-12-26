using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class NFT : MonoBehaviour
{ 
    // to hold data after encryption
    private byte[] _3fcedc35a461afb;

    // raw data of the NFT, to allow later reference
    public byte[] Image { get; set; } // issues with namespaces, so this is raw form
    public string[] Data { get; set; }
    public string Description { get; set; }
    public byte[] byteName { get; set; }
    public byte[] byteAddress { get; set; }
    public byte[] byteTimeStamp { get; set; }
    public byte[] byteNotes { get; set; }
    public int[] dL = new int[5];


    // to hold encryption key for the NFT
    private readonly byte[] _5a453e29e7d9be5;
    public static byte[] Combine(byte[] first, byte[] second)
    {
        //amazing that it took so long to find this solution, clumsy and inefficient, but it's the only one that works
        byte[] bytes = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
        Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
        return bytes;
    }
    
    public NFT(byte[] image, string[] data, byte[] encryptKey)
    {
        // store the plaintext data
        Image = image; // remember, this is 'bytes[]'
        Data = data;
        
        // store encryption key
        _5a453e29e7d9be5 = encryptKey;
        
        // convert string to byte, Combine is a convenience function to concatenate byte arrays, which is otherwhise difficult
        byteName = Combine(image, Encoding.UTF8.GetBytes(data[0]));
        byteAddress = Combine(byteName, Encoding.UTF8.GetBytes(data[1]));
        byteTimeStamp = Combine(byteAddress, Encoding.UTF8.GetBytes(data[2]));
        byteNotes = Combine(byteTimeStamp, Encoding.UTF8.GetBytes(data[3]));

        EncryptData(byteNotes);

    }

    private void EncryptData(byte[] inputBytes)
    {
        // symmetric algorithm, since encrypt/decrypt locally
        // actually, this is bad. no need to decrypt, just verify when transfering
        Aes encryptor = Aes.Create();
        encryptor.Key = _5a453e29e7d9be5;

        // Microsofts fondness for 'streams'
        using (MemoryStream ms = new MemoryStream())
        {
            // create a CryptoStream to encrypt the data
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                // write the raw byte data to the MamoryStream using CryptoStream
                cs.Write(inputBytes, 0, inputBytes.Length);
            }

            // store encrypted data 
            _3fcedc35a461afb = ms.ToArray();
        }
    }
    
    private void sellNFT(byte[] image, string[] newData, byte[] encryptionKey)
    {
        bool verified = verifyOwnership(image, newData, encryptionKey);
        if(verified)
        {
            /*
             * here is where we would make calls to external libraries. We would make a transaction,
             * and once verified, call transferOwnership(name, description);
             */
            //transferOwnership(name, description);
        }
        else { Debug.Log("Verification failed, carefully check input data."); }
    }

    private bool transferOwnership(string[] newData)
    {
        this.Data = newData;

        // convert new data to bytes for use in encryption
        byteName = Combine(this.Image, Encoding.UTF8.GetBytes(newData[0]));
        byteAddress = Combine(byteName, Encoding.UTF8.GetBytes(newData[1]));
        byteTimeStamp = Combine(byteAddress, Encoding.UTF8.GetBytes(newData[2]));
        byteNotes = Combine(byteTimeStamp, Encoding.UTF8.GetBytes(newData[3]));

        // encrypt the new data
        EncryptData(byteNotes);

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
                byte[] thisName = Combine(img, Encoding.UTF8.GetBytes(data[0]));
                byte[] thisAddress = Combine(thisName, Encoding.UTF8.GetBytes(data[1]));
                byte[] thisTimeStamp = Combine(thisAddress, Encoding.UTF8.GetBytes(data[2]));
                byte[] thisNote = Combine(thisTimeStamp, Encoding.UTF8.GetBytes(data[3]));
                // to convert back 'string str = Encoding.UTF8.GetString(bytes);'
                // can substitue 'ASCII' for 'UTF8'
                cs.Write(thisNote, 0, thisNote.Length); 
            }

            // temporarily store the encrypted data to be compared
            byte[] verifyData = ms.ToArray();
            return(verifyData.Equals(this._3fcedc35a461afb));
        }
    }
    public string showEncryptedData()
    {
        //string datatoshow = Encoding.UTF8.GetString(this._3fcedc35a461afb);
        //return this._3fcedc35a461afb.ToString();
        string hash = String.Empty;
        foreach (byte ef170b in this._3fcedc35a461afb)
        {
            hash += ef170b.ToString("x2");
        }
        return hash;
    }
}
/*// this didn't work, preserved here for posterity
         //(IEnumerable<byte>)
        // convert to bytes
        int len = image.Length;
        List<byte> inputData = new List<byte>(image);
        byteName = Encoding.UTF8.GetBytes(data[0]);
        byteAddress = Encoding.UTF8.GetBytes(data[1]);
        byteTimeStamp = Encoding.UTF8.GetBytes(data[2]);
        byteNotes = Encoding.UTF8.GetBytes(data[3]);
        inputData.InsertRange(len, new List<byte>(Encoding.UTF8.GetBytes(data[0])));
        len += byteName.Length;
        inputData.InsertRange(len, new List<byte>(Encoding.UTF8.GetBytes(data[1])));
        len += byteAddress.Length;
        inputData.InsertRange(len, new List<byte>(Encoding.UTF8.GetBytes(data[2])));
        len += byteTimeStamp.Length;
        inputData.InsertRange(len, new List<byte>(Encoding.UTF8.GetBytes(data[3])));

        byte[] ac81a3e05c68743a370 = inputData.ToArray();

        // encrypt the data
        EncryptData(ac81a3e05c68743a370);

        dL[0] = image.Length;
        dL[1] = byteName.Length;
        dL[2] = byteAddress.Length; 
        dL[3] = byteTimeStamp.Length;
        dL[4] = byteNotes.Length;

 */
/*
                 //this info not store at the class or global level because it needs to be verified
                int a = img.Length;
                int b = a + thisName.Length + 1;
                int c = b + thisAddress.Length;
                int d = c + thisTimeStamp.Length;
                int e = d + thisNote.Length;
                // write to the memory stream, data chunks aligned
                cs.Write(img, 0, a);
                cs.Write(thisName, a+1, b);
                cs.Write(thisAddress, b+1, c);
                cs.Write(thisTimeStamp, c+1, d);
                cs.Write(thisNote, d + 1, e);


 */