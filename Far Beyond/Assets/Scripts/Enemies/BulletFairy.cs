using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFairy : MonoBehaviour
{
    //si la bala sale de los limites, que se destruya. 
    public float speedBullet = 5.0f;
    public float lifeTime = 3.0f;
    public Animator anim;
    void Update()
    {
        this.transform.position += new Vector3(speedBullet * Time.deltaTime, 0f, 0f);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            anim.Play("Bullet_die");
            Destroy(this.gameObject, 0.6f);

        }

    }
    public void Die()
    {
        Destroy(this.gameObject); //esto nos permite destruir la bala. 

    }
}
