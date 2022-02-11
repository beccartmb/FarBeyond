using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FairyStates //recordamos de nuevo que el enum sirve para hacer estados.
{
    Patrol, Chase, Attack //si quieres mas estados, añade mas, podras acceder de uno en uno en ellos. 
}

public class FairysCode : MonoBehaviour
{
    public float chaseRange;
    public float attackRange;
    public float speedFairy = 8;
    bool hasAttackFinished = false; //ponemos que el ataque no esta acabado por ahora, para que se pueda activar con facilidad. 
    public Animator anim;
    public List<transform> PatroPointsFairy = new List<transform>(); //punto de patruyaje del hada. 
    int nextPatrolPointFairy = 0; //el contador empieza en 0 ¿recuerdas?
    public int FairyLife = 16;

    public FairyStates currentStates = FairyStates.Patrol; //vamos a necesitar referenciar el enum poniendo .algo

    #region SINGLETON
    public static SlimesCode Instance { get; private set; }

    private void Awake() //esto junto con el intance de arriba convierte nuestro personaje en un singleton (mucho mas comodo para juegos de un jugador) ya que nos permite acceder a este codigo desde otros codigos. 
    {
        Instance = this;
    }
    #endregion

    public void Start()
    {
        anim = GetComponent<animator>(); //llamos aqui la animacion, sera necesaria, ya sabes. 
        StartCoroutine(FMSCoroutine()); //y aqui la courtina para que siempre ejecute los enum. 
    }

    IEnumerator FMSCoroutine()
    {
        while (true) //creamos un Ienumeretor por cada acción.
        {
            //ahora vamos a ejecutar los estados. 
            switch (currentStates)
            {
                case FairyStates.Patrol:
                    yield return Patrol();
                    break;
                case FairyStates.Chase:
                    yield return Chase();
                    break;
                case FairyStates.Attack:
                    yield return Attack();
                    break;
            }
        }



    }



}
