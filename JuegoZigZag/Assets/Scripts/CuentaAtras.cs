using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class CuentaAtras : MonoBehaviour {
    
    public Image imagen;
    public Sprite[] numeros;
    private Button boton;

    // Start is called before the first frame update
    void Start() {
        boton = GameObject.FindAnyObjectByType<Button>();
        //boton = GameObject.FindWithTag("BotonInicio").GetComponent<Button>();
        boton.onClick.AddListener(Inicio);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void Inicio() {
        imagen.gameObject.SetActive(true);
        boton.gameObject.SetActive(false);

        StartCoroutine(CuentaRegresiva());
    }

    IEnumerator CuentaRegresiva() {
        for(int i = 0; i < numeros.Length; i++) {
            imagen.sprite = numeros[i];
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene("Escena01");
    }
}
