using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : MonoBehaviour
{
    //si la bala sale de los limites, que se destruya. 
    public float lifeTime = 7.0f; //aqui determinamos la vida de la flame bullet(7 segundos) aunque luego durara el tiempo de stamina que el jugador tenga. 
    public Animator anim;//y aqui designamos un anim de cada objeto que sera necesario para llamarlo en otro script.

   
   /* void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void Die()
    {
        Destroy(this.gameObject); //esto nos permite destruir la bala. 
    }
    void OnTriggerEnter(Collider other)//esto es por si es una colision en area.
    {

        /*SlimesCode slimes = other.GetComponent<SlimesCode>(); //desde aqui vamos a gestionar la muerte del slime. 

        if (slimes != null) //si la bala del jugador impacta contra el ALIEN éste muere. 
        {
            SlimesCode.Instance.SlimeLife -= 3;
        }
        */
   /* }*/
}
