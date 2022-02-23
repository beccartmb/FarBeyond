using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllayBullet : MonoBehaviour
{
    //si la bala sale de los limites, que se destruya. 
    public Vector3 speedBullet = new Vector3(0, 0, 0); //necesita esto para que rebote en el suelo.
    //public float speedBullett = 50;
    public float lifeTime = 3.0f;//la vida de la bala.
    float bouncePower = 0.5f;//esto es la fuerza con la que va a rebotar. 
    float bounceCount = 0.5f; //esto es el contador de tiempo que rebota. 
    public Animator anim;
    private List<Collider2D> floors = new List<Collider2D>(); //necesaria para detectar los suelos, al igual que en illay.

    void Update()
    {
        movementBullet(); //aqui designamos el movimiento de la bala.
        if (lifeTime <= 0) //si la vida de la bala es menor que 0, destruirla con el metodo "destroyBullet" en el cual existe una animacion.
        {
            DestroyBullet();
        }
    }

    void DestroyBullet() //esto nos hara la animacion de destruir la bala.
    {
        anim.Play("Bullet_die");
        Destroy(this.gameObject, 0.6f); //el 0.6 es para que despues de destruirse se espere menos de un segundo para que se pueda efectuar bien la animacion de destruccion de la bala.
    }
    public void movementBullet() //y esto nos hara el movimiento.
    {
        this.transform.position += speedBullet * Time.deltaTime;
        lifeTime -= Time.deltaTime;

        if (speedBullet.y > 0)
        {
            bounceCount -= Time.deltaTime; //si la velocidad en Y es mayor que 0 (es decir, va hacia arriba) EL CONTADOR EMPIEZA A BAJAR para poder cambiar nuevamente.

            if (bounceCount < 0f) //que si la bala esta cayendo hacia abajo, volteamos el valor absoluto de las Y en negativo porque lo hemos indicado DELANTE DEL MATHF.
            {
                speedBullet.y = -Mathf.Abs(speedBullet.y); //esto es para asegurarme que la bala va hacia bajo, debera el negativo delante del mathf para que no le quite el signo.  
            }
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
            floors.Add(collision.collider);// aqui añadimos los suelos a la lista. todo aquel suelo con el que choque. 
            speedBullet.y = Mathf.Abs(speedBullet.y); // ABS lo que hace es quitarle el valor a cualquier signo, con esto nos aseguramos que vaya hacia arriba a cierta velocidad. 

            bounceCount = bouncePower; //aqui es cuando le decimos CUANTO TIEMPO VA A REBOTAR y con que fuerza. .
            bouncePower *= 0.5f; //aqui se va multiplicando por la mitad, que es equivalente a dividir entre dos y hara que progresivamente rebote menos. 
        }
        if (collision.GetContact(0).normal.x < -0.5f && collision.collider.GetComponent<PlatformEffector2D>() == null) //Aqui usamos normal.x porque determinamos que es la pared DERECHA mediante el rebote que haria al chocar con ella. 
        {
            DestroyBullet(); //aqui detectamos que si hay paredes, que se destruya la bala. 
        }
        //.normal significa que esta calculando hacia donde rebota el objeto que choca contra algo, es decir, recalcula el vector de transform para ponerlo en 0,0,0 a partir de tocar lo que sea.
        if (collision.GetContact(0).normal.x > +0.5f && collision.collider.GetComponent<PlatformEffector2D>() == null) //Aqui usamos normal.x porque determinamos que es la pared IZQUIERDA mediante el rebote que haria al chocar con ella. SI NO TIENE EFFECTOR PLATAFORM SE CONSIDERA SUELO O PARED
        {
            DestroyBullet(); //aqui ponemos que si detecta paredes, que se destruya la bala. 
        }
    }
}

