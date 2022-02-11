using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicScript : MonoBehaviour
{
    float cooldown;
    public float timeToPlayCinematic = 10.0f; //aqui pondremos lo que dura el video de la cinematica. 

    public void Update()
    {
        cooldown += Time.deltaTime; //eso controla que el tiempo que pasa sea el correcto, como en la vida real.
        if (cooldown >= timeToPlayCinematic) //esto es para que cuente cuanto tiempo va a estar la animacion en marcha. una vez acabada dicha animacion, pasa a la escena.
        {
            SceneManager.LoadScene("Hallway");
        }
    }

}
