using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllayBullet : MonoBehaviour
{
    //si la bala sale de los limites, que se destruya. 
    public Vector3 speedBullet = new Vector3(0, 0, 0);
    public float speedBullett = 50;
    public float lifeTime = 3.0f;
    public float bounceCount = 2.0f; //esto es el contador de tiempo que rebota. 
    public Animator anim;
    private List<Collider2D> floors = new List<Collider2D>(); //necesaria para detectar los suelos. 
    private List<Collider2D> wallOnRight = new List<Collider2D>(); //necesaria para detectar las paredes a la derecha. 
    private List<Collider2D> wallOnLeft = new List<Collider2D>(); //necesaria para detectar las paredes a la izquierda. 

    void Update()
    {
        movementBullet();
        if (lifeTime <= 0)
        {
            anim.Play("Bullet_die");
            Destroy(this.gameObject, 0.6f);
        }
        
    }
    public void movementBullet()
    {
        this.transform.position += speedBullet * Time.deltaTime;
        lifeTime -= Time.deltaTime;

        if (floors.Count > 0)
        {
            speedBullet = new Vector3(10, 5, 0);
            this.transform.position += speedBullet * Time.deltaTime;
            lifeTime -= Time.deltaTime;
            bounceCount -= Time.deltaTime;
        }
        else if (floors.Count<=0)
        {
            speedBullet = new Vector3(10, -5, 0);
            this.transform.position += speedBullet * Time.deltaTime;
            lifeTime -= Time.deltaTime;
        }
    }
    public void Die()
    {
        Destroy(this.gameObject); //esto nos permite destruir la bala. 
    }

    private void OnCollisionEnter2D(Collision2D collision) //esto me va a permitir AÑADIR A LA LISTA cuando el jugador TOQUE y pase lo que hay dentro del IF.
    {
        if (collision.GetContact(0).normal.y > 0.5f) //ponemos "0" al lado de contact porque recuerda que en programacion se empieza a contar desde 0. 
        {
            floors.Add(collision.collider);
        }
        if (collision.GetContact(0).normal.x < -0.5f && collision.collider.GetComponent<PlatformEffector2D>() == null) //Aqui usamos normal.x porque determinamos que es la pared DERECHA mediante el rebote que haria al chocar con ella. 
        {
            wallOnRight.Add(collision.collider); //como es la derecha, rebotaria hacia la izquierda, por eso, el 0.5 ESTA EN NEGATIVO. 
        }
        if (collision.GetContact(0).normal.x > +0.5f && collision.collider.GetComponent<PlatformEffector2D>() == null) //Aqui usamos normal.x porque determinamos que es la pared IZQUIERDA mediante el rebote que haria al chocar con ella. SI NO TIENE EFFECTOR PLATAFORM SE CONSIDERA SUELO O PARED
        {
            wallOnLeft.Add(collision.collider); //como es la izquierda, rebotaria hacia la derecha, por eso, el 0.5 ESTA EN POSITIVO. 
        }
    }
}

