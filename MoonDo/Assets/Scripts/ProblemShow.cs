using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class ProblemInfoResponse
{
    public string problem;
}

public class ProblemShow : MonoBehaviour
{
    public TMPro.TextMeshProUGUI outputText;

    private int problemPaperId;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetProblemsFromServer());
    }

    IEnumerator GetProblemsFromServer()
    {
        string serverURL = "http://121.163.89.235:8080/problem/all?problemPaperId" + problemPaperId;
        /*
        UnityWebRequest www = new UnityWebRequest(serverURL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        */
        UnityWebRequest www = UnityWebRequest.Get(serverURL);
        yield return www.SendWebRequest();

        if (www.error == null)
        {
            string jsonResponse = www.downloadHandler.text;
            var responseData = JsonUtility.FromJson<ProblemInfoResponse>(jsonResponse);
            Debug.Log(www.downloadHandler.text);

            outputText.text = responseData.problem;
        }
        else
        {
            Debug.LogError("Error: " + www.error);
        }
    }
}
