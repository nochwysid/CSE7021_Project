using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menue : MonoBehaviour
{
    public Image myImg;
    public TMPro.TMP_Dropdown myMenu;
    public int guess, idx, ID;
    public string item_name;
    public GameObject ball;
    public GameObject ring_orn_5_gold;
    public GameObject ring_orn_5_fbx;
    public GameObject hat;
    public GameObject fbxGlasses1;
    public GameObject Steampunk_Goggles;
    public GameObject[] GOs;
    TMPro.TMP_Dropdown[] obj;
    public void Start()
    {
        GOs = new GameObject[7];
        GOs[0] = ball;
        GOs[1] = ring_orn_5_gold;
        GOs[2] = ring_orn_5_fbx; 
        GOs[3] = hat; 
        GOs[4] = fbxGlasses1; 
        GOs[5] = Steampunk_Goggles;
        //GOs[6] =  
    }
    public void Selector()
    {
        //neeed to get player instance id, but this keeps throwing nullreference esceptions
        //ID = GetComponent<Player>().GetInstanceID();
        ID = (int)7030703;
        //guess = (int)System.Math.Pow(10,myMenu.value);
        obj = this.GetComponents<TMP_Dropdown>();
        if (obj[0].name == "Likes")
        {
            guess = (int)System.Math.Pow(10, obj[0].value); 
            idx = obj[1].value;
            item_name = obj[1].itemText.text;
            Debug.Log("item_name: "+item_name);
        }
        else
        {
            guess = (int)System.Math.Pow(10, obj[1].value);
            idx = obj[0].value;
            GOs[idx].gameObject.GetComponent<GetVotes>().vote(ID.GetHashCode(), guess);
            item_name = obj[0].itemText.text;
            Debug.Log("item_name: " + item_name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
