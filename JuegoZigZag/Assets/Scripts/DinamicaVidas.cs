using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DinamicaVidas : MonoBehaviour
{
    public Sprite[] corazones;
    public int vidas;
    public SpriteRenderer SpriteRenderer;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>(); // Initialize the spriteRender variable
        //Se accede al elemento 0 de la lista corazones
        SpriteRenderer.sprite = corazones[vidas-3];
    }

    private void Update()
    {
        //Hcemos que siempre haya como maximo 3 corazones
        if (corazones.Length > 3){
            vidas = 3;
        }
    }

    //Sumar una vida
    public void SumarVida()
    {
        if (corazones.Length > vidas){
            vidas++;
            SpriteRenderer.sprite = corazones[vidas-1];
        }
    }

    //Restar una vida
    public void RestarVida()
    {
        if (vidas > 0){
            vidas--;
            SpriteRenderer.sprite = corazones[vidas+1];
        }else{
            GameOver();
        }
    }

    //Game Over
    public void GameOver() {
        SceneManager.LoadScene("Escena99FinPartida");
    }
}