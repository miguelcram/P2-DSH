using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMov : MonoBehaviour {
    public Camera camara;
    public AudioSource audioSource;
    public GameObject sueloActual, suelo, suelo1, suelo2;
    public float velocidad = 5;
    public int NivelActual;

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

        // Lanzar el Raycast hacia abajo
        if (Physics.Raycast(transform.position, Vector3.down, 3f)) {
        
        } else {
            Debug.Log("Flotando");
            GetComponent<Rigidbody>().useGravity = true;
            StartCoroutine(GameOver());
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Suelo") || other.gameObject.CompareTag("SueloObs1") || other.gameObject.CompareTag("SueloObs2")) {
            StartCoroutine(BorrarSuelo(other.gameObject));
        }
    }

    IEnumerator BorrarSuelo(GameObject suelo) {
        float aleatorio = Random.Range(0.0f, 1.0f);
        if (aleatorio > 0.5) {
            ValX += 6.0f;
        } else {
            ValZ += 6.0f;
        }

        //Segun el nivel, spawnea un suelo de un tipo u otro
        NivelActual = SceneManager.GetActiveScene().buildIndex;
        if(NivelActual == 1){
            sueloActual = suelo;
        }else if(NivelActual == 2){
            float aleatorio1 = Random.Range(0.0f, 1.0f);
            if (aleatorio1 > 0.5) {
                sueloActual = suelo;
            } else {
                sueloActual = suelo1;
            }
        }else if(NivelActual == 3){
            float aleatorio2 = Random.Range(0.0f, 1.0f);
            if (aleatorio2 > 0.5) {
                sueloActual = suelo;
            } else {
                sueloActual = suelo2;
            }
        }
        Instantiate(sueloActual, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        yield return new WaitForSeconds(5);
        sueloActual.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        sueloActual.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2);
        Destroy(sueloActual);
    }

    void CrearSueloInicial() {
        for (int i = 0; i < 3; i++){
            ValZ += 6.0f;
            Instantiate(suelo, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        }
    }

    void CambiarDireccion() {
        if (DireccionActual == Vector3.forward) {
            DireccionActual = Vector3.right;
        } else if(DireccionActual == Vector3.right){
            DireccionActual = Vector3.left;
        } else {
            DireccionActual = Vector3.forward;
        }
    }

    //Play sound when touch the ground
    void OnCollisionEnter(Collision collision) {
        if (!tocarSuelo && (collision.gameObject.CompareTag("suelo") 
                            || collision.gameObject.CompareTag("suelo1")
                            || collision.gameObject.CompareTag("suelo2"))) {
            tocarSuelo = true;
            audioSource.Play();
        }
        GetComponent<Rigidbody>().useGravity = false;
    }

    //Game Over
    IEnumerator GameOver() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Escena99");
    }
}