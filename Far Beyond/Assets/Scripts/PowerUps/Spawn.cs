using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public float spawnTimeLimit = 12.0f;
    public float cooldownRespawn = 6.0f;
    public GameObject powerUpO2;

    void Update()
    {
        cooldownRespawn += Time.deltaTime;
        if (cooldownRespawn >= spawnTimeLimit) //esto es para que dispare aleatoriamente contando los aliens de la lista. 
        {
            Instantiate(powerUpO2, this.transform.position, Quaternion.identity); //crear el power Up en la unicacion del Spawn
            cooldownRespawn = 0;
            spawnTimeLimit = Random.Range(6f, 12f); //spawnearse entre el segundo 6 y el segundo 12
        }
    }
}
