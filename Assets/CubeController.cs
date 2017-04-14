using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{


    private DisruptiveApi.ApiClient api = new DisruptiveApi.ApiClient();
    // Use this for initialization
    void Start()
    {
        StartCoroutine(api.GetSensors(Callback));


    }
   

    private void Callback(List<Sensor> response)
    {
        print(response.Count);
    }
}
