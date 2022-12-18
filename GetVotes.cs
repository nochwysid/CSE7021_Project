using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Xml.Schema;

public class GetVotes : MonoBehaviour
{
    public bool canGrab;
    private bool novotes;
    public GameObject item; // the idea is that this is the item being voted on
    public GameObject[] players;
    private int votes;
    private Hashtable winners;
    private Hashtable slots2;  // this is for storing the ID and vote guess of players
    private float t1;
    private byte[] image;

    // Start is called before the first frame update
    void Start()
    {
        canGrab = false;
        novotes = true;
        //item = this.gameObject;
        //item = this.gameObject.GetComponent<GameObject>();
        //item = this.GetHashCode();
        //item = this.GetInstanceID();
        item = this.GetComponent<GameObject>();
        votes = 0;
        winners = new Hashtable();
        slots2 = new Hashtable();
        t1 = 0.0F;

        Material material = item.GetComponent<Material>();
        int crc = material.ComputeCRC();
        Debug.Log("material crc: " + crc.ToString() + ", item name: " + item.name);
        image = (byte[])Encoding.UTF8.GetBytes(item.GetComponent<Material>().ToString());
        //image = (byte[])Encoding.UTF8.GetBytes(item.GetHashCode().ToString());
    }
    public void vote(int hashcode, int guess)
    {
        this.votes++;
        if (novotes)
        {
            slots2.Add(hashcode, guess);
            novotes = false;
            t1 = Time.time;
        }
        else
        {
            if (!slots2.ContainsKey(hashcode))
            {
                slots2.Add(hashcode, guess);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //check time and votes
        if (votes > 0 && (t1 - Time.time) > 6.0F)
        {
            int hc = 0;

            slots2.AsQueryable();

            slots2.AsParallel();

            //List<int> keylist = slots2.Keys;

            ICollection keylist = slots2.Keys;

            foreach (var item in keylist)
            {
                // 'value' is returned as object, so requires casting
                if (Mathf.Abs((int)slots2[item] - votes) < 3)// threshold for how close to be to the actual number of votes
                {
                    winners.Add(item, (int)slots2[item]);
                    //int hc = winners[0][0];
                    //GameObject p = winners[0].Get<GameObject>();
                }
            }
        }
    }
}
