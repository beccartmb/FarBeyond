using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllayBullet : MonoBehaviour
{
    //si la bala sale de los limites, que se destruya. 
    public float speedBullet = 8.0f;
    public float lifeTime = 3.0f;
    public Animator anim;
    public Rigidbody2D rbody;
    public Vector3 direction;
    private List<Collider2D> floors = new List<Collider2D>(); //necesaria para detectar los suelos. 
    private List<Collider2D> wallOnRight = new List<Collider2D>(); //necesaria para detectar las paredes a la derecha. 
    private List<Collider2D> wallOnLeft = new List<Collider2D>(); //necesaria para detectar las paredes a la izquierda. 

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //aqui estamos preparando el rigidbody de la bola el cual debera tener desactivado el IsKinematic.
        direction = rbody.velocity;
    }

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

    public void OnColliderEnter2D(Collider2D other)
    {
        rbody.velocity = new Vector2(direction.x, -direction.y);

        /*if (col.contacts[0].normal.x != 0) //https://www.youtube.com/watch?v=4XrazhLqLSQ manera de programar de este chico para hacer que caiga y rebote. 
        {
            Die();
        }*/ 
        if(floors.Count!=null)
        {
            /*direction = (this.transform.position - List.floors).normaliced;*/ //me gustaria poder poner que si toca dicha lista que se mueva hacia otra zona ¿como?
        }
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
//me gustaria meter que la pelota rebote en el suelo pero no se como. quiero que los detecte como el jugador y el propio suelo,
//mediante un Rigidbody y que este les haga rebotar pero, si meto el movimiento, ¿se le arignara inmediatamente?

