using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsCode : MonoBehaviour
{
    public List<GameObject> hearts = new List<GameObject>();//Aquí vamos a meter los corazones uno a uno. 

    void Update() //esto va a leer cuantos corazones estan activos. 
    {
        if(GameManager.Instance.currentSave.playerHearts>=4)
        {
            hearts[3].SetActive(true); //entre corchetes pondremos el numero que le corresponde a la posicion. Es decir, la lista al ser publica añades tu a mano las cosas desde el inspector. 
            hearts[2].SetActive(true);
            hearts[1].SetActive(true);
            hearts[0].SetActive(true);
        }
        else if(GameManager.Instance.currentSave.playerHearts==3)
        {
            hearts[3].SetActive(false); //como la vida del jugador es 3, el unico que se desactiva es el numero 4.
            hearts[2].SetActive(true);
            hearts[1].SetActive(true);
            hearts[0].SetActive(true);
        }
        else if (GameManager.Instance.currentSave.playerHearts == 2)
        {
            hearts[3].SetActive(false); //como la vida del jugador es 2, los unicos que se desactivan es el 4 y el 3.
            hearts[2].SetActive(false);
            hearts[1].SetActive(true);
            hearts[0].SetActive(true);
        }
        else if (GameManager.Instance.currentSave.playerHearts == 1)
        {
            hearts[3].SetActive(false); //como la vida del jugador es 1, los unicos que se desactivan es el 4, el 3 y el 2.
            hearts[2].SetActive(false);
            hearts[1].SetActive(false);
            hearts[0].SetActive(true);
        }
        else if (GameManager.Instance.currentSave.playerHearts <= 0)
        {
            hearts[3].SetActive(false); //como la vida del jugador es 0, todos se desactivan.
            hearts[2].SetActive(false);
            hearts[1].SetActive(false);
            hearts[0].SetActive(false);
        }
    }
}
