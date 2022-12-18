using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monolith : MonoBehaviour
{
    public AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        bool tap = false;
        if (Input.touchSupported) { if (Input.touchCount > 0) { tap = true; } }
        //Touch inputStruct = Input.GetTouch(0);
        //if (Input.GetButtonDown("Fire1") || inputStruct.tapCount > 0)
        if (Input.GetButtonDown("Fire1") || tap)
        {
            audioSource.Play();
        }
    }
}
