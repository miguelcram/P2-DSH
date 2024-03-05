using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicaVidas : MonoBehaviour
{
    public GameObject vida;
    private NuevaPartida NuevaPartida;
    
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
                NuevaPartida.Nivel2();
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

}
