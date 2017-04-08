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
            using (var webClient = new UnityWebRequest(string.Format("https://api.disruptive-technologies.com/v1/things/" + id))) {
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
/**
        public IEnumerator Put<T>(string path, string json, System.Action<ApiResponse<T>> resultCallback, System.Action<ApiResponse> errorCallback = null, string authKey = null)
        {

            using (var webClient = new UnityWebRequest(string.Format("http://{0}{1}", Host, path)))
            {

                webClient.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json.ToString()));
                webClient.downloadHandler = new DownloadHandlerBuffer();
                webClient.method = UnityWebRequest.kHttpVerbPUT;

                if (!string.IsNullOrEmpty(authKey))
                    webClient.SetRequestHeader("authenticationkey", authKey);

                webClient.SetRequestHeader("Accept", "application/json");
                webClient.SetRequestHeader("Content-Type", "application/json");

                yield return webClient.Send();

                var jsonText = webClient.downloadHandler.text;
                if (Debug.isDebugBuild)
                {
                    Debug.Log("PUT result: " + jsonText);
                    Debug.Log("PUT responseCode: " + webClient.responseCode);
                }

                if (webClient.responseCode != 200)
                {
                    if (errorCallback != null)
                    {
                        var response = new ApiResponse();
                        response.ResponseCode = (int)webClient.responseCode;
                        response.ResponseMessage = jsonText;
                        Debug.LogError(path + "\n" + JsonUtility.ToJson(response));
                        errorCallback(response);
                    }
                }
                else
                {
                    if (resultCallback != null)
                    {
                        var response = new ApiResponse<T>();
                        response.ResponseCode = (int)webClient.responseCode;
                        if (!string.IsNullOrEmpty(jsonText))
                        {
                            response.Data = JsonUtility.FromJson<T>(jsonText);
                        }
                        resultCallback(response);
                    }
                }
            }
        }**/
    }
}
