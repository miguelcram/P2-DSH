using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DinamicaVidas : MonoBehaviour
{
    public List<Sprite> corazones;
    public int vidas;
    public SpriteRenderer spriteRender;

    private void Start()
    {
        //Se accede al elemento 0 de la lista corazones
        spriteRender.sprite = corazones[vidas-3];
    }

    private void Update()
    {
        //Hcemos que siempre haya como maximo 3 corazones
        if (corazones.Count > 3){
            corazones = corazones.GetRange(0, 3);
        }
    }

    //Sumar una vida
    public void SumarVida()
    {
        if (corazones.Count > vidas){
            vidas++;
            spriteRender.sprite = corazones[vidas-1];
        }
    }

    //Restar una vida
    public void RestarVida()
    {
        if (vidas > 0){
            vidas--;
            spriteRender.sprite = corazones[vidas+1];
        }else{
            GameOver();
        }
    }

    //Game Over
    public void GameOver() {
        SceneManager.LoadScene("Escena99FinPartida");
    }
}