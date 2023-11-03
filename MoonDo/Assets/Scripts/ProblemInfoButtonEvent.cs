using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ProblemInfoButtonEvent : MonoBehaviour
{

    int problemPaperId;

    public void GoToProblemPaperScene()
    {
        if (problemPaperId != -1)
        {
            PlayerPrefs.GetInt("ProblemPaperId", problemPaperId);
            SceneManager.LoadScene("ProblemPaper");
        }
        else
        {
            Debug.LogError("ProblemPaperId를 저장할 수 없습니다.");
        }
    }
}
