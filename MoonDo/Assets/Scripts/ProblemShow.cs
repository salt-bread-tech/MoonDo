using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System;

[System.Serializable]
public class ResponseObject
{
    public string[] getProblemResponses;
}

public class ProblemShow : MonoBehaviour
{
    public Text problemText; // UI Text 요소에 대한 참조
    int problemPaperId;
    private object jsonArray;

    public void ProblemPaperShow()
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
        ProblemPaperShow();
    }

    IEnumerator UnityWebRequestPost()
    {
        string url = "http://localhost:8080/problem/all";
        Debug.Log("웹리퀘스트 시작");
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
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("JSON Response: " + jsonResponse);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    // 정규 표현식을 사용하여 "문제 X: "를 제거
                    jsonResponse = Regex.Replace(jsonResponse, @"(^\[|\]$|\\)", "");
                    jsonResponse = jsonResponse.Replace("?", "?\n");
                    jsonResponse = jsonResponse.Replace("문제", "\n문제");


                    ResponseObject responseData = new ResponseObject();
                    responseData.getProblemResponses = jsonResponse.Split(new char[] { '\"', ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                    if (responseData != null && responseData.getProblemResponses != null)
                    {
                        // 문자열 목록을 개행 문자로 연결하여 Text UI에 설정
                        string resultText = string.Join("\n", responseData.getProblemResponses);
                        problemText.text = resultText;
                    }
                    else
                    {
                        Debug.Log("Invalid or missing response data");
                    }
                }
                else
                {
                    Debug.Log("Empty response data");
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