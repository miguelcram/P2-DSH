using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarPuntos : MonoBehaviour
{
    public Text contadorPuntos;
    // Start is called before the first frame update
    void Start() {
        contadorPuntos.text = "" + ControladorPuntos.Instance.totalPuntos;
    }

}
