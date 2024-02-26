using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PolvosEstrella : MonoBehaviour {
    public GameObject particulas;

    void OnDestroy() {
        UnityEngine.Vector3 posactual = new UnityEngine.Vector3(transform.position.x, 0.5f, transform.position.z);
        GameObject particulasInstanciadas = Instantiate(particulas, posactual, particulas.transform.rotation);
        Destroy(particulasInstanciadas, 2.0f);
    }
}