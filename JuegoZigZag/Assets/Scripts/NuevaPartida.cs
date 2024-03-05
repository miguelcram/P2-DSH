using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NuevaPartida : MonoBehaviour {

    public void Reinicio() {
        SceneManager.LoadScene(1);
    }

    public void MenuInicial() {
        SceneManager.LoadScene(0);
    }

    public void Nivel2() {
        SceneManager.LoadScene("Escena04 Nivel2");
    }

    public void SiguienteNivel() {
        int Escena = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(Escena);
    }
}