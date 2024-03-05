using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPuntos : MonoBehaviour {

    public static ControladorPuntos Instance;
    public int totalPuntos;

    // Awake is called when the script instance is being loaded.
    void Awake() {
        if(ControladorPuntos.Instance == null) {
            ControladorPuntos.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public void SumarPuntos(int puntos) {
        totalPuntos += puntos;
    }
}
