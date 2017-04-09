﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContainerController : MonoBehaviour {

	private DisruptiveApi.ApiClient api = new DisruptiveApi.ApiClient();

	public ParticleSystem ice;
	public ParticleSystem explosion;
	public TextMesh temperatureLabel;
    public PathFinder pathFinder;

	public TextMesh roomTempFarLeft;
	public TextMesh roomTempFarRight;
	public TextMesh roomTempNearLeft;
	public TextMesh roomTempNearRight;

    private string targetSensor1 = "b3l1r8i15hbg00eb1pf0";
    private string targetSensor2 = "b3l1rca1frig00d8q7tg";
    public string targetSensorId;
	public Light roomLightFarLeft;
	public Light roomLightFarRight;
	public Light roomLightNearLeft;
	public Light roomLightNearRight;

    public float targetTemperature = 22;
	public float maxTemperature = 40;
	public float minTemperature = 0;

	public string tempSuffix = "°c";

	// Gamla: "206847491"
	// BrewR: "206860292"
	// BrewL: "206889735"
	public string sensorId = "206847491";

	public float smoothing = 1f;
	public Vector3 target;

	public float maxX = 4.5f;
	public float maxZ = 9.5f;

    private RoomSensor roomSensor1 = new RoomSensor()
    {
        id = "b3ke6ba1frig00d8q7t0",
        temperature = 30f,
        pos = new Vector3(-4.5f, 1, -9.5f)
    };

    private RoomSensor roomSensor2 = new RoomSensor()
    {
        id = "b3ke66q1frig00d8q7sg",
        temperature = 5f,
        pos = new Vector3(4.5f, 1, -9.5f)
    };

    private RoomSensor roomSensor3 = new RoomSensor()
    {
        id = "b3ke50l3dl0000eaekh0",
        temperature = 18f,
        pos = new Vector3(4.5f, 1, 9.5f)
    };

    private RoomSensor roomSensor4 = new RoomSensor()
    {
        id = "b3ke4dd3dl0000eaekgg",
        temperature = 27f,
        pos = new Vector3(-4.5f, 1, 9.5f)
    };

    // 16c -> -9.5f

    // ((temp-16)/24*18-9.5f)

    // 40c -> 9.5f

    void Start()
    {
		explosion.Stop ();
		ice.Stop ();
		SetupRoomSensorLabels();
        StartCoroutine(pollBrewSensor());
        StartCoroutine(pollRoomSensor(roomSensor1));
        StartCoroutine(pollRoomSensor(roomSensor2));
        StartCoroutine(pollRoomSensor(roomSensor3));
        StartCoroutine(pollRoomSensor(roomSensor4));
        StartCoroutine(MovementCoroutine());
    }

    IEnumerator pollTargetSensor()
    {
        while (true)
        {
            StartCoroutine(api.GetSensors(targetSensorId, (temp) =>
            {
                targetTemperature = temp;
            }));
            yield return new WaitForSeconds(3f);
        }
    }

	void SetupRoomSensorLabels()
	{
		roomSensor1.tempLabel = roomTempNearRight;
		roomSensor1.tempLight = roomLightNearRight;
		roomSensor2.tempLabel = roomTempFarRight;
		roomSensor2.tempLight = roomLightFarRight;
		roomSensor3.tempLabel = roomTempFarLeft;
		roomSensor3.tempLight = roomLightFarLeft;
		roomSensor4.tempLabel = roomTempNearLeft;
		roomSensor4.tempLight = roomLightNearLeft;
	}

    IEnumerator MovementCoroutine ()
	{
        while (target == null)
        {
            yield return new WaitForSeconds(1f);
        }
                
        while (true)
        {
            var endTarget = pathFinder.AllocateEnd(sensorId, target);
            while (Vector3.Distance(transform.position, endTarget) < 0.02f)
            {
                yield return new WaitForSeconds(2f);
            }
            var nextTarget = pathFinder.GetNextGrid(sensorId, transform.position, endTarget);
            //print("ET: " + JsonUtility.ToJson(nextTarget));
            while (Vector3.Distance(transform.position, nextTarget) > 0.02f)
            {
                transform.position = Vector3.Lerp(transform.position, nextTarget, smoothing * Time.deltaTime);

                yield return null;
            }
        }

		print("MovementCoroutine is now finished.");
	}

    void ResponseHandler(float temperature)
    {
        target = GetTarget(temperature);
		temperatureLabel.text = (temperature + tempSuffix);
		if (temperature > maxTemperature) {
			explosion.Play();
		} else if (explosion.isEmitting) {
			explosion.Stop ();
		}
		if (temperature < minTemperature) {
			ice.Play();
		} else if (ice.isEmitting) {
			ice.Stop ();
		}
        //StartCoroutine(MovementCoroutine(target));
    }

    private IEnumerator pollBrewSensor()
    {
        while (true)
        {
            StartCoroutine(api.GetSensors(sensorId, ResponseHandler));
            yield return new WaitForSeconds(5f);
        }
    }


    private IEnumerator pollRoomSensor(RoomSensor sensor)
    {
        while (true)
        {
            StartCoroutine(api.GetSensors(sensor.id, (temp) =>
            {
                sensor.temperature = temp;
					sensor.tempLabel.text = (temp + tempSuffix);
            }));
            yield return new WaitForSeconds(3f);
        }
    }

    private Vector3 GetTarget(float inputTemperature)
    {
        var targetTemperature = CreateTargetRoomTemperature(inputTemperature);
        Vector3 one = GetPositionFromTemperature(roomSensor1, roomSensor3, targetTemperature);
        Vector3 two = GetPositionFromTemperature(roomSensor2, roomSensor4, targetTemperature);
        var distance = Vector3.Distance(one, two);
        Vector3 midPoint = Vector3.MoveTowards(one, two, distance / 2);
        return midPoint;
    }

    private float CreateTargetRoomTemperature(float inputTemperature)
    {
        print("TARGET: " + targetTemperature);
        if (inputTemperature > targetTemperature)
        {
            return inputTemperature - 2 < targetTemperature ? targetTemperature : inputTemperature - 2;
        } else
        {
            return inputTemperature + 2 > targetTemperature ? targetTemperature : inputTemperature + 2;
        }
    }

    private Vector3 GetPositionFromTemperature(RoomSensor s1, RoomSensor s2, float temperature)
    {
        var sensor1Pos = s1.pos;
        var sensor2Pos = s2.pos;
        var sensor1temp = s1.temperature;
        var sensor2temp = s2.temperature;

        var sensorDistance = Vector3.Distance(sensor1Pos, sensor2Pos);
        var distanceFromSensor1 = sensorDistance * (temperature - sensor1temp) / (sensor2temp - sensor1temp);

        var target = Vector3.MoveTowards(sensor1Pos, sensor2Pos, distanceFromSensor1);

        return target;

    }

    private Vector3 createmidPoint(Vector3 one, Vector3 two)
    {
        return Vector3.MoveTowards(one, two, Vector3.Distance(one, two));
    }

    public class RoomSensor
    {
        public string id;
        public float temperature;
        public Vector3 pos;
		public TextMesh tempLabel;
		public Light tempLight;
    }
}
