using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMov4 : MonoBehaviour {
    public Camera camara;
    public AudioSource audioSource;
    public Text contadorPuntos;
    public Text contadorEstrellas;

    public GameObject sueloPrincipal;
    public GameObject[] suelos;

    public Button botonIzquierda;
    public Button botonDerecha;
    public Button botonAdelante;
    public Button botonSaltar;

    public float velocidad = 5;
    public float fuerzaSalto = 4.0f;

    private Vector3 offset;
    private float ValX, ValZ;
    private bool tocarSuelo = false;
    private Vector3 DireccionActual;

    private int numdeSuelos = 0;
    private int contsuelos = 0;
    private int numsueloscreados = 4;

    private int totalEstrellas = 0;
    private int puntos = 1;
    private int puntosEstrella = 3;

    List<GameObject> suelosTotales = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        offset = camara.transform.position;
        contadorPuntos.text = "Puntos: " + ControladorPuntos.Instance.totalPuntos;
        contadorEstrellas.text = "Estrellas: " + totalEstrellas;
        suelosTotales.Add(sueloPrincipal);
        CrearSueloInicial();
        DireccionActual = Vector3.forward;

        botonIzquierda.onClick.AddListener(() => CambiarDireccion(Vector3.left));
        botonDerecha.onClick.AddListener(() => CambiarDireccion(Vector3.right));
        botonAdelante.onClick.AddListener(() => CambiarDireccion(Vector3.forward));
        botonSaltar.onClick.AddListener(Saltar);
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
    if (numdeSuelos<10) {
        float aleatorio = Random.Range(0.0f, 1.0f);
        if (numsueloscreados == 19) {
            aleatorio = 0.3f;
        }
        if (aleatorio > 0.5) {
            ValX += 6.0f;
        } else {
            ValZ += 6.0f;
        }

        //Tipo de suelo a instanciar
        int sueloAleatorio = Random.Range(0,3);
        GameObject sueloPrefab;
        if (numsueloscreados < 20 ){
            if (numsueloscreados == 19) {
                sueloAleatorio = 4;
            }
            switch(sueloAleatorio) {
                case 1: sueloPrefab = suelos[1];
                        break;
                case 2: sueloPrefab = suelos[2];
                        break;
                case 3: sueloPrefab = suelos[2];
                        break;
                case 4: sueloPrefab = suelos[3];
                        break;
                default: sueloPrefab = suelos[0];
                        break;
            }
            nuevoSuelo = Instantiate(sueloPrefab, new Vector3(ValX, 0, ValZ), Quaternion.identity);
            suelosTotales.Add(nuevoSuelo);
            numdeSuelos++;
            numsueloscreados++;
        }
    }
    yield return new WaitForSeconds(4);
    GameObject sueloBorrar = suelosTotales[0];
    sueloBorrar.GetComponent<Rigidbody>().isKinematic = false;
    sueloBorrar.GetComponent<Rigidbody>().useGravity = true;
    yield return new WaitForSeconds(3);
    Destroy(suelosTotales[0]);
    suelosTotales.RemoveAt(0);
    numdeSuelos--;
}

    void CrearSueloInicial() {
        for (int i = 0; i < 3; i++) {
            ValZ += 6.0f;
            GameObject sueloInicial = Instantiate(suelos[0], new Vector3(ValX, 0, ValZ), Quaternion.identity);
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
        yield return new WaitForSeconds(0.3f);
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

    void Saltar() {
        if (tocarSuelo) {
            GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            tocarSuelo = false;
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Premio")) {
            totalEstrellas++;
            ControladorPuntos.Instance.SumarPuntos(puntosEstrella);
            contadorEstrellas.text = "Estrellas: " + totalEstrellas;
            contadorPuntos.text = "Puntos: " + ControladorPuntos.Instance.totalPuntos;
            Destroy(other.gameObject);
        }
    }
}