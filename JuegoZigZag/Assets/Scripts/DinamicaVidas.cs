using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DinamicaVidas : MonoBehaviour
{
    public GameObject vida;
    private Scene reinicio;
    
    // Start is called before the first frame update
    void Start()
    {
        float offset = 1.0f;  // Separación entre vidas
        //Creamos 3 vidas separadas por un offset
        for (int i = 0; i < 3; i++)
        {
            Instantiate(vida, new Vector3(i * offset, 0, 0), Quaternion.identity);
        }
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Obstaculo")){            
            RestarVida();
            if(VidasPerdidas() >= 3){
                ReiniciarNivel();
            }
        }
    }

    void RestarVida()
    {
        Destroy(GameObject.FindWithTag("Vida"));
    }

    int VidasPerdidas()
    {
        GameObject[] vidas = GameObject.FindGameObjectsWithTag("Vida");
        if(vidas != null){
            return 3 - vidas.Length;
        }else{ return 0; }
    }

    void ReiniciarNivel()
    {
        reinicio = SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(reinicio);
    }
}
