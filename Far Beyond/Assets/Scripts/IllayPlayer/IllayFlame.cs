using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllayFlame : MonoBehaviour
{
    //si la bala sale de los limites, que se destruya. 
    public float lifeTime = 5.0f;
    public Animator anim;

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            anim.Play("Flame_die");
            Destroy(this.gameObject, 1.0f);
        }
    }
    public void Die()
    {
        Destroy(this.gameObject); //esto nos permite destruir la bala. 
    }
    
    /*void OnTriggerEnter(Collider other)//esto es por si es una colision en area.
    {

        SlimesCode slimes = other.GetComponent<SlimesCode>(); //desde aqui vamos a gestionar la muerte del slime. 

        if (slimes != null) //si la bala del jugador impacta contra el ALIEN éste muere. 
        {
            slimes.Die();
            Destroy(this.gameObject);
        }
    }*/
}
