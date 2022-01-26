using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePlayerRight : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            player.isSlidingRight = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            player.isSlidingRight = false;
        }
    }
}
