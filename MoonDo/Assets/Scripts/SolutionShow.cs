using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ResponseSolution
{
    public List<string> getSolutionResponses;
}

public class SolutionShow : MonoBehaviour
{
    public Text solutionText;
    int problemPaperId;

    public void SolutionPaperShow()
    {
        StartCoroutine(UnityWebRequestPost());
    }
    private void Start()
    {
        Debug.Log("ProblemPaperId : " + problemPaperId);
        /*
        problemPaperId = PlayerPrefs.GetInt("ProblemPaperId", -1);
        if(problemPaperId != -1)
        {
            Debug.Log("ProblemPaperId : " + problemPaperId);
        }
        else
        {
            Debug.Log("ProblemPaperId를 찾을 수 없음");
        }
        */
        SolutionPaperShow();
    }

    IEnumerator UnityWebRequestPost()
    {
        string url = "http://121.163.89.235:8080/problem/solution";
        if (problemPaperId != -1)
        {
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            CreateProblemResponse requestData = new CreateProblemResponse();
            requestData.problemPaperId = 97; // 저장된 값을 사용
            string jsonData = JsonUtility.ToJson(requestData);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.error == null)
            {
                var jsonResponse = request.downloadHandler.text;
                // ResponseObject responseData = JsonUtility.FromJson<ResponseObject>(jsonResponse);
                // Debug.Log("responseData: " + responseData);

                if (jsonResponse != null)
                {
                    solutionText.text = jsonResponse;
                }
                else
                {
                    Debug.Log("Invalid or missing response data");
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
        else
        {
            Debug.LogError("ProblemPaperId를 찾을 수 없음");
        }
    }
}
