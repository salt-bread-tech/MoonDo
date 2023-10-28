using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System;

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
        string url = "http://localhost:8080/problem/solution";
        if (problemPaperId != -1)
        {
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            CreateProblemResponse requestData = new CreateProblemResponse();
            requestData.problemPaperId = PlayerPrefs.GetInt("ProblemPaperId", -1); ; // 저장된 값을 사용
            string jsonData = JsonUtility.ToJson(requestData);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.error == null)
            {
                var jsonResponse = request.downloadHandler.text;
                Debug.Log("JSON Response: " + jsonResponse);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    // jsonResponse = Regex.Replace(jsonResponse, @"(^\[|\]$|\\)", "");
                    jsonResponse = jsonResponse.Replace("\\", "");
                    jsonResponse = jsonResponse.Replace("[", "");
                    jsonResponse = jsonResponse.Replace("]", "");
                    jsonResponse = jsonResponse.Replace("\"answer\":", "");
                    jsonResponse = jsonResponse.Replace("\"explanation\":", "");
                    jsonResponse = jsonResponse.Replace("}\n", "}\n\n");
                    jsonResponse = jsonResponse.Replace("{", "");
                    jsonResponse = jsonResponse.Replace("}", "");
                    jsonResponse = jsonResponse.Replace("정답", "\n\n정답");
                    jsonResponse = jsonResponse.Replace("정답 1", "정답 1");
                    jsonResponse = jsonResponse.Replace("풀이", "\n풀이");

                    ResponseObject responseData = new ResponseObject();
                    responseData.getProblemResponses = jsonResponse.Split(new char[] { '\"', ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                    if (responseData != null && responseData.getProblemResponses != null)
                    {
                        // 문자열 목록을 개행 문자로 연결하여 Text UI에 설정
                        string resultText = string.Join("\n", responseData.getProblemResponses);
                        solutionText.text = resultText;
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
}
