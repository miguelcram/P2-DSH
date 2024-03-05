using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMov01 : MonoBehaviour {
    public Camera camara;
    public AudioSource audioSource;
    public Text contadorPuntos;

    public GameObject sueloPrincipal;
    public GameObject sueloPrefab;
    public GameObject sueloMeta;
   // public GameObject[] suelos;

    public float velocidad = 5;
    public float fuerzaSalto = 1.0f;

    private Vector3 offset;
    private float ValX, ValZ;
    private bool tocarSuelo = false;
    private Vector3 DireccionActual;
    private int numdeSuelos = 0;
    private int contsuelos = 0;
    private int numsueloscreados = 4;
    private int puntos = 1;
    
    List<GameObject> suelosTotales = new List<GameObject>();

    public Button botonIzquierda;
    public Button botonDerecha;
    public Button botonAdelante;

    // Start is called before the first frame update
    void Start() {
        offset = camara.transform.position;
        contadorPuntos.text = "Puntos: " + ControladorPuntos.Instance.totalPuntos;
        suelosTotales.Add(sueloPrincipal);
        CrearSueloInicial();
        DireccionActual = Vector3.forward;
        ControladorPuntos.Instance.totalPuntos = 0;

        botonIzquierda.onClick.AddListener(() => CambiarDireccion(Vector3.left));
        botonDerecha.onClick.AddListener(() => CambiarDireccion(Vector3.right));
        botonAdelante.onClick.AddListener(() => CambiarDireccion(Vector3.forward));
    }

    // Update is called once per frame
    void Update() {
        camara.transform.position = transform.position + offset;
        transform.Translate(DireccionActual * velocidad * Time.deltaTime);
        StartCoroutine (Flotando());
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Suelo") {
            StartCoroutine (BorrarSuelo(other.gameObject));
        }
    }

IEnumerator BorrarSuelo(GameObject suelo) {
    GameObject nuevoSuelo = null;
    float aleatorio = Random.Range(0.0f, 1.0f);
    if (numsueloscreados == 19) {
        aleatorio = 0.3f;
    }
    if (aleatorio > 0.5) {
        ValX += 6.0f;
    } else {
        ValZ += 6.0f;
    }

    if (numdeSuelos<10) {
        if (numsueloscreados > 18 ){
            if (numsueloscreados == 19) {
                nuevoSuelo = Instantiate(sueloMeta, new Vector3(ValX, 0, ValZ), Quaternion.identity);
            }
        } else {
            nuevoSuelo = Instantiate(sueloPrefab, new Vector3(ValX, 0, ValZ), Quaternion.identity);
        }
        numsueloscreados++;
        suelosTotales.Add(nuevoSuelo);
        numdeSuelos++;
    }
    yield return new WaitForSeconds(4);
    GameObject sueloBorrar = suelosTotales[0];
    sueloBorrar.GetComponent<Rigidbody>().isKinematic = false;
    sueloBorrar.GetComponent<Rigidbody>().useGravity = true;
    yield return new WaitForSeconds(1.5f);
    Destroy(suelosTotales[0]);
    suelosTotales.RemoveAt(0);
    numdeSuelos--;
}

    void CrearSueloInicial() {
        for (int i = 0; i < 3; i++) {
            ValZ += 6.0f;
            GameObject sueloInicial = Instantiate(sueloPrefab, new Vector3(ValX, 0, ValZ), Quaternion.identity);
            suelosTotales.Add(sueloInicial);
            numdeSuelos++;
        }
    }

    void CambiarDireccion(Vector3 nuevaDireccion) {
        DireccionActual = nuevaDireccion;
    }

    //Play sound when touch the ground
    void OnCollisionEnter(Collision collision) {
        if (!tocarSuelo && collision.gameObject.CompareTag("Suelo")) {
            tocarSuelo = true;
            audioSource.Play();
        }
        GetComponent<Rigidbody>().useGravity = false;
        if (collision.gameObject.CompareTag("Suelo")) {
            contsuelos++;
            ControladorPuntos.Instance.SumarPuntos(puntos);
            contadorPuntos.text = "Puntos: " + ControladorPuntos.Instance.totalPuntos;

            //Debug.Log("Numero de suelos: " + contsuelos);
            /*if (contsuelos == 20) {
                StartCoroutine(NextLevel());
            }*/
        }

        if (collision.gameObject.CompareTag("Meta")) {
            StartCoroutine(NextLevel());
        }
    }

    //Game Over
    void GameOver() {
        SceneManager.LoadScene("Escena99FinPartida");
    }

    IEnumerator NextLevel() {
        yield return new WaitForSeconds(0.5f);
        velocidad = 0;
        int Escena = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(Escena);
    }

    //Flotando
    IEnumerator Flotando() {
        // Lanzar el Raycast hacia abajo
        if (!Physics.Raycast(transform.position, Vector3.down, 0.5f)) {
            //Debug.Log("Flotando");
            GetComponent<Rigidbody>().useGravity = true;
            yield return new WaitForSeconds(1);
            if (!Physics.Raycast(transform.position, Vector3.down, 2f)){
                yield return new WaitForSeconds(0.5f);
                GameOver();
            }
        }
    }
}