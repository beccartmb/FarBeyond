using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialog : MonoBehaviour
{
    bool isPlayerInTriggerDialog; //este bool va a ser declarado aqui dentro, para ver si esta dentro el jugador o no. 

    public void Update()
    {
        if (isPlayerInTriggerDialog && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("interation");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        IllayPlayer illayPlayer = collision.GetComponent<IllayPlayer>();
        if (illayPlayer != null)
        {
            isPlayerInTriggerDialog = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IllayPlayer illayPlayer = collision.GetComponent<IllayPlayer>();
        if (illayPlayer != null)
        {
            isPlayerInTriggerDialog = false;
        }
    }
}
