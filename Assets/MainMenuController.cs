using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	private DisruptiveApi.ApiClient api = new DisruptiveApi.ApiClient();

	public Dropdown roomFarLeft;
	public Dropdown roomFarRight;
	public Dropdown roomNearLeft;
	public Dropdown roomNearRight;

	public Dropdown containerOne;
	public Dropdown containerTwo;

	public Button startButton;

	void Start () {
		StartCoroutine(api.GetSensors(GetSensors));
	}

	private void GetSensors(List<Sensor> sensors)
	{
		roomFarLeft.options.Clear();
		foreach (Sensor sensor in sensors) {
			string sensorLabel = (sensor.id + " (" + sensor.temperature + "°c) '" + sensor.name + "'");

			// Populate room sensor dropdowns
			roomFarLeft.options.Add (new Dropdown.OptionData(){ text=sensorLabel });
			roomFarRight.options.Add (new Dropdown.OptionData(){ text=sensorLabel });
			roomNearLeft.options.Add (new Dropdown.OptionData(){ text=sensorLabel });
			roomNearRight.options.Add (new Dropdown.OptionData(){ text=sensorLabel });

			// Populate container sensor dropdowns
			containerOne.options.Add (new Dropdown.OptionData(){ text=sensorLabel });
			containerTwo.options.Add (new Dropdown.OptionData(){ text=sensorLabel });
		}


		print (sensors.Count);
	}

	private void StartButton() {
		
	}
	// Start Button:
	// Read selected room sensor id's from dropdowns (x4)
	// Read selected container sensor id's from dropdowns (x2)
	// Read target temperature from textboxes (x2)
	// Save Sensor id's to static map
	// Save target temperatures to map
}
