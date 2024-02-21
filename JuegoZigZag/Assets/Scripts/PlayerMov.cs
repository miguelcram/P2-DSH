using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMov : MonoBehaviour {
    public Camera camara;
    public AudioSource audioSource;
    public GameObject Suelo;
    public float velocidad = 5;

    private Vector3 offset;
    private float ValX, ValZ;
    private bool tocarSuelo = false;
    private Vector3 DireccionActual;

    // Start is called before the first frame update
    void Start() {
        offset = camara.transform.position;
        CrearSueloInicial();
        DireccionActual = Vector3.forward;
    }

    // Update is called once per frame
    void Update() {
        camara.transform.position = transform.position + offset;
        if (Input.GetKeyUp(KeyCode.Space)) {
            CambiarDireccion();
        }
        transform.Translate(DireccionActual * velocidad * Time.deltaTime);
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Suelo") {
            StartCoroutine (BorrarSuelo(other.gameObject));
        }
    }

    IEnumerator BorrarSuelo(GameObject suelo) {
        float aleatorio = Random.Range(0.0f, 1.0f);
        if (aleatorio > 0.5) {
            ValX += 6.0f;
        } else {
            ValZ += 6.0f;
        }
        Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        yield return new WaitForSeconds(5);
        suelo.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        suelo.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2);
        Destroy(suelo);
    }

    /*-------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator BorrarSuelo(GameObject suelo) {
        float tiempoEspera = 2.0f;
        bool Ultimacolision = false;

        float aleatorio = Random.Range(0.0f, 1.0f);
        if (aleatorio > 0.5) {
            ValX += 6.0f;
        } else {
            ValZ += 6.0f;
        }

        while (tiempoEspera > 0 && !Ultimacolision) {
            yield return new WaitForSeconds(0.5f);
            tiempoEspera -= 0.5f;
        }

        if (!Ultimacolision) {
            Destroy(suelo);
        }


        Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        yield return new WaitForSeconds(2);
        suelo.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        suelo.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2);
        Destroy(suelo);
    }

    ---------------------------------------------------------------------------------------------------------------------------------------------*/

    void CrearSueloInicial() {
        for (int i = 0; i < 3; i++) {
            ValZ += 6.0f;
            Instantiate(Suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        }
    }

    void CambiarDireccion() {
        if (DireccionActual == Vector3.forward) {
            DireccionActual = Vector3.right;
        } else {
            DireccionActual = Vector3.forward;
        }
    }

    //Play sound when touch the ground
    void OnCollisionEnter(Collision collision) {
        if (!tocarSuelo && collision.gameObject.CompareTag("Suelo")) {
            tocarSuelo = true;
            audioSource.Play();
        }
    }
}
