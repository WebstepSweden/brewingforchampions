using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour {

    public string janne;
    public GameObject test;
    private DisruptiveApi.ApiClient api = new DisruptiveApi.ApiClient();
    // Use this for initialization
    void Start () {
        StartCoroutine(poll());
    }
	

    void ResponseHandler(float temperature)
    {
        print(temperature);
    }

    private IEnumerator poll()
    {
        while (true)
        {
            StartCoroutine(api.GetSensors("206847491", ResponseHandler));
            yield return new WaitForSeconds(5f);
        }
    }


	// Update is called once per frame
	void Update () {
		
	}
}
