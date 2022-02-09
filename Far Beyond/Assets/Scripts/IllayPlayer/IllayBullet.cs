using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllayBullet : MonoBehaviour
{
    //si la bala sale de los limites, que se destruya. 
    public float speedBullet = 5.0f;
    public float lifeTime = 3.0f;
    public Animator anim; 

    void Update()
    {
        this.transform.position += new Vector3(speedBullet * Time.deltaTime,0f, 0f);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            //anim.Play("Bullet_die");
            Destroy(this.gameObject);
            
        }

    }
    public void Die()
    {
        Destroy(this.gameObject); //esto nos permite destruir la bala. 
        
    }
    
    /*void OnTriggerEnter2D(Collider2D other)//esto es por si es una colision en area.
    {
        
        SlimesCode slimes = other.GetComponent<SlimesCode>(); //desde aqui vamos a gestionar la muerte del slime. 

        if (slimes != null) //si la bala del jugador impacta contra el ALIEN éste muere. 
        {
            slimes.Die();
            Destroy(this.gameObject);
            
        } //Quitamos esto de aquí porque para que funcione la muerte de la bala se tiene que dar solo en un script.
          //Si lo ponemos en los dos peta y no lo hace. Está todo puesto en el código del slime.

    }*/
}
