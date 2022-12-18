using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Schema;
using UnityEngine;

public class Grab : MonoBehaviour
{ /* will add custom tage to each object, tag is location to put object*/
    public bool canGrab;
    public GameObject item; // the idea is that this is the item being voted on
    private byte[] image;

    // Start is called before the first frame update
    void Start()
    {
        canGrab = false;
        item = this.GetComponent<GameObject>();
        
    }
    public void vote(int hashcode, int guess)
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
}
