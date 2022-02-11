using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomZone : MonoBehaviour
{
    public Animator anim;
    void OnTriggerEnter2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();

        if (player != null)
        {

            anim.Play("Big_camera_water");

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();

        if (player != null)
        {
            anim.Play("Small_camera_water");
        }
    }
}

