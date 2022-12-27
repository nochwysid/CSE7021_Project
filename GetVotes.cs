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
    private HashSet<GameObject> entities;
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
        item = this.gameObject;
        votes = 0;
        winners = new Hashtable();
        slots2 = new Hashtable();
        t1 = 0.0F;

        Material material = item.GetComponent<MeshRenderer>().material;
        int crc = material.ComputeCRC();
        //Debug.Log("material crc: " + crc.ToString() + ", item name: " + item.name);
        image = (byte[])Encoding.UTF8.GetBytes(material.ToString());
        //image = (byte[])Encoding.UTF8.GetBytes(item.GetHashCode().ToString());
    }
    public void vote(int hashcode, int guess)
    {   //entities.Add(other.tostring)
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
        if (votes > 0 && (t1 - Time.time) > 2.0F)
        {
            Debug.Log("-----line 62------");
            int hc = 0;
            if (this.votes != entities.Count) { Debug.LogError("Double dipping not allowed"); this.votes = entities.Count; }

            slots2.AsQueryable();
            HashSet<GameObject>.Enumerator ents = entities.GetEnumerator();
            while (ents.MoveNext()) { }
            slots2.AsParallel();

            //List<int> keylist = slots2.Keys;

            ICollection keylist = slots2.Keys;
            Debug.Log("------line 74-------");
            foreach (var player in keylist)
            {
                // 'value' is returned as object, so requires casting
                if (Mathf.Abs((int)slots2[player] - this.votes) < 3)// threshold for how close to be to the actual number of votes
                {
                    Debug.Log("------line80------");
                    winners.Add(player, (int)slots2[player]);
                    //int hc = winners[0][0];
                    //GameObject p = winners[0].Get<GameObject>();
                }
            }
        }
    }
}
