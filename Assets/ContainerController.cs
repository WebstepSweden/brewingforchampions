using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour {

	private DisruptiveApi.ApiClient api = new DisruptiveApi.ApiClient();

	public float smoothing = 1f;
	public Vector3 target;

	public float maxX = 4.5f;
	public float maxZ = 9.5f;


	// 16c -> -9.5f

	// ((temp-16)/24*18-9.5f)

	// 40c -> 9.5f

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(poll());
	}

	IEnumerator MovementCoroutine (Vector3 target)
	{
		while(Vector3.Distance(transform.position, target) > 0.05f)
		{
			transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);

			yield return null;
		}

		print("Keg reached the target.");

		yield return new WaitForSeconds(3f);

		print("MovementCoroutine is now finished.");
	}

	void ResponseHandler(float temperature)
	{
		print(temperature);
		var target = new Vector3 (transform.position.x, transform.position.y, GetPositionFromTemperature(temperature));

		StartCoroutine (MovementCoroutine (target));
	}

	private IEnumerator poll()
	{
		while (true)
		{
			StartCoroutine(api.GetSensors("206847491", ResponseHandler));
			yield return new WaitForSeconds(5f);
		}
	}

	private float GetPositionFromTemperature (float temperature)
	{
		var position = ((temperature-16)/24*18-maxZ);
		return position;
	}

	// Update is called once per frame
	void Update () {

	}
}
