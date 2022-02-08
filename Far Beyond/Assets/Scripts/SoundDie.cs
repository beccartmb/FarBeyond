using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDie : MonoBehaviour
{

    public float soundDie;

    void Start()
    {
        Destroy(gameObject, soundDie);
    }

    
}
