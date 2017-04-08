using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    /**
    private string sensor1Id = "b3ke6ba1frig00d8q7t0";
    private string sensor2Id = "b3ke66q1frig00d8q7sg";
    private string sensor3Id = "b3ke50l3dl0000eaekh0";
    private string sensor4Id = "b3ke4dd3dl0000eaekgg";

    private float sensor1Temp = 30f;
    private float sensor2Temp = 5f;
    private float sensor3Temp = 23f;
    private float sensor4Temp = 13f;

    private Vector3 sensor1Pos = new Vector3(-4.5f, 1, -9.5f);
    private Vector3 sensor2Pos = new Vector3(4.5f, 1, -9.5f);
    private Vector3 sensor3Pos = new Vector3(4.5f, 1, 9.5f);
    private Vector3 sensor4Pos = new Vector3(-4.5f, 1, 9.5f);
    **/
    private string brewSensor1Id = "206847491";
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

    private DisruptiveApi.ApiClient api = new DisruptiveApi.ApiClient();
    // Use this for initialization
    void Start()
    {
        StartCoroutine(pollBrewSensor());
        StartCoroutine(pollRoomSensor(roomSensor1));
        StartCoroutine(pollRoomSensor(roomSensor2));
        StartCoroutine(pollRoomSensor(roomSensor3));
        StartCoroutine(pollRoomSensor(roomSensor4));
    }


    void ResponseHandler(float temperature)
    {
        var target = GetTarget(temperature);
    }

    private IEnumerator pollBrewSensor()
    {
        while (true)
        {
            StartCoroutine(api.GetSensors(brewSensor1Id, ResponseHandler));
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
            }));
            yield return new WaitForSeconds(3f);
        }
    }

    private Vector3 GetTarget(float temperature)
    {        
        Vector3 one = GetPositionFromTemperature(roomSensor1, roomSensor3, temperature);
        Vector3 two = GetPositionFromTemperature(roomSensor2, roomSensor4, temperature);
        var distance = Vector3.Distance(one, two);
        Vector3 midPoint = Vector3.MoveTowards(one, two, distance/2);
        return midPoint;
    }

    private Vector3 GetPositionFromTemperature(RoomSensor s1, RoomSensor s2, float temperature)
    {
        var sensor1Pos = s1.pos;
        var sensor2Pos = s2.pos;
        var sensor1temp = s1.temperature;
        var sensor2temp = s2.temperature;

        var sensorDistance = Vector3.Distance(sensor1Pos, sensor2Pos);
        print("DTOT: " + sensorDistance);
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
    }
}
