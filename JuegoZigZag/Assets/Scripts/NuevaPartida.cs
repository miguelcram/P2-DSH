using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NuevaPartida : MonoBehaviour {

    public void Reinicio() {
        SceneManager.LoadScene("Escena01");
    }

    public void MenuInicial() {
        SceneManager.LoadScene("Escena00Inicio");
    }

    public void Nivel2() {
        SceneManager.LoadScene("Escena03");
    }
}