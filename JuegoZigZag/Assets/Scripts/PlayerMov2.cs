using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMov2 : MonoBehaviour {
    public Camera camara;
    public AudioSource audioSource;
    public GameObject sueloPrincipal;
    public GameObject[] suelos;

    public DinamicaVidas vidas_canvas;
    public Text puntuacion;
    public int puntos = 0;
    public int vida = 3;

    //Botones de control
    public Button botonIzquierda;
    public Button botonDerecha;
    public Button botonAdelante;
    public Button botonSaltar;

    public float velocidad = 5;
    public float fuerzaSalto = 1.0f;

    private Vector3 offset;
    private float ValX, ValZ;
    private bool tocarSuelo = false;
    private Vector3 DireccionActual;
    private int numdeSuelos = 0;
    List<GameObject> suelosTotales = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        offset = camara.transform.position;
        suelosTotales.Add(sueloPrincipal);
        CrearSueloInicial();
        DireccionActual = Vector3.forward;

        botonIzquierda.onClick.AddListener(() => CambiarDireccion(Vector3.left));
        botonDerecha.onClick.AddListener(() => CambiarDireccion(Vector3.right));
        botonAdelante.onClick.AddListener(() => CambiarDireccion(Vector3.forward));
        botonSaltar.onClick.AddListener(Saltar);

        vidas_canvas = GameObject.FindObjectOfType<DinamicaVidas>();
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
        if (aleatorio > 0.5) {
            ValX += 6.0f;
        } else {
            ValZ += 6.0f;
        }

        if (numdeSuelos<10) {
        //Tipo de suelo a instanciar
            float sueloAleatorio = Random.Range(0.0f, 3.0f);
            GameObject sueloPrefab;
            if (sueloAleatorio < 1.0f) {
                sueloPrefab = suelos[0];
            } else if (sueloAleatorio < 2.0f) {
                sueloPrefab = suelos[1];
            } else {
                sueloPrefab = suelos[2];
            }
            nuevoSuelo = Instantiate(sueloPrefab, new Vector3(ValX, 0, ValZ), Quaternion.identity);
            suelosTotales.Add(nuevoSuelo);
            numdeSuelos++;
        }
        yield return new WaitForSeconds(5);
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
    void OnCollisionEnter(Collision other) {
        if (!tocarSuelo && other.gameObject.CompareTag("Suelo")) {
            tocarSuelo = true;
            audioSource.Play();
        }
        GetComponent<Rigidbody>().useGravity = false;

        if(other.gameObject.tag == "Premio") {
            StartCoroutine(GanoPuntos(other.gameObject, puntos, puntuacion));
        }

        if (other.gameObject.tag == "Obstaculo") {
            StartCoroutine(PierdoVida());
        }
        
        if(other.gameObject.tag == "Vida") {
            StartCoroutine(GanoVida());
        }
    }

    IEnumerator GanoPuntos(GameObject other, int puntos, Text Puntuacion) {
        yield return new WaitForSeconds(1);
        //Aumentar puntos
        puntos++;
        Puntuacion.text = "Puntuación: " + puntos;
        Destroy(other.gameObject);
    }

    IEnumerator PierdoVida() {
        yield return new WaitForSeconds(1);
        vidas_canvas.RestarVida();
    }

    IEnumerator GanoVida() {
        yield return new WaitForSeconds(1);
        vidas_canvas.SumarVida();
    }

    //Flotando
    IEnumerator Flotando() {
        // Lanzar el Raycast hacia abajo
        if (!Physics.Raycast(transform.position, Vector3.down, 0.5f)) {
            Debug.Log("Flotando");
            GetComponent<Rigidbody>().useGravity = true;
            yield return new WaitForSeconds(1);
            if (!Physics.Raycast(transform.position, Vector3.down, 2f)){
                yield return new WaitForSeconds(0.5f);
                vidas_canvas.GameOver();
            }
        }
    }

    //Funcion para el boton saltar
    void Saltar() {
        if (tocarSuelo) {
            GetComponent<Rigidbody>().AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            tocarSuelo = false;
        }
    }

}