using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisruptiveApi : MonoBehaviour {


    public class ApiClient
    {

        public IEnumerator GetSensors(string id, System.Action<float> resultCallback)
        {
            using (var webClient = new UnityWebRequest(string.Format("https://api.disruptive-technologies.com/v1/things/" + id)))
            {
                webClient.downloadHandler = new DownloadHandlerBuffer();

                webClient.SetRequestHeader("Accept", "application/json");
                webClient.SetRequestHeader("Authorization", "ApiKey d45a7c14c88f48f5937a8fc3254378ad");
                webClient.SetRequestHeader("Content-Type", "application/json");
                yield return webClient.Send();

                if (webClient.responseCode != 200) print("ERROR");
                var jsonText = webClient.downloadHandler.text;
                var subr = jsonText.Substring(jsonText.IndexOf("\"temperature") + "\"temperature:".Length + 1);
                subr = subr.Substring(0, subr.IndexOf(","));
                resultCallback(float.Parse(subr));
            }
        }

        public IEnumerator GetSensors(System.Action<List<Sensor>> resultCallback)
        {
            using (var webClient = new UnityWebRequest(string.Format("https://api.disruptive-technologies.com/v1/things")))
            {
                webClient.downloadHandler = new DownloadHandlerBuffer();

                webClient.SetRequestHeader("Accept", "application/json");
                webClient.SetRequestHeader("Authorization", "ApiKey d45a7c14c88f48f5937a8fc3254378ad");
                webClient.SetRequestHeader("Content-Type", "application/json");
                yield return webClient.Send();

                if (webClient.responseCode != 200) print("ERROR");
                var jsonText = webClient.downloadHandler.text;
                var sensorResponse = JsonUtility.FromJson<SensorsResponse>(jsonText);
                print(JsonUtility.ToJson(sensorResponse));
                List<Sensor> sensors = new List<Sensor>();
                foreach(Sensor sensor in sensorResponse.things)
                {
                    if (sensor.hasTemperature)
                    {
                        sensors.Add(sensor);
                    }
                }
                resultCallback(sensors);
            }
        }
    }
}
