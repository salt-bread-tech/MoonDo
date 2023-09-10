using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    // ȸ������ �� �α��� �� ��ȯ
    public void LoginToSignUp()
    {
        SceneManager.LoadSceneAsync("SignUp");
    }
    public void SignUpToLogin()
    {
        SceneManager.LoadSceneAsync("Login");
    }
    public void LoginToHome()
    {
        SceneManager.LoadSceneAsync("Home");
    }

    // ButtomNavigation
    public void HomeToProfile()
    {
        SceneManager.LoadScene("Profile");
    }
    public void BookmarkToProfile()
    {
        SceneManager.LoadScene("Profile");
    }
    public void ProblemMakingToProfile()
    {
        SceneManager.LoadScene("Profile");
    }

    public void ProfileToBookmark()
    {
        SceneManager.LoadScene("Bookmark");
    }
    public void HomeToBookmark()
    {
        SceneManager.LoadScene("Bookmark");
    }
    public void ProblemMakingToBookmark()
    {
        SceneManager.LoadScene("Bookmark");
    }

    public void ProfileToHome()
    {
        SceneManager.LoadScene("Home");
    }
    public void BookmarkToHome()
    {
        SceneManager.LoadScene("Home");
    }
    public void ProblemMakingToHome()
    {
        SceneManager.LoadScene("Home");
    }

    public void ProfileToProblemMaking()
    {
        SceneManager.LoadScene("ProblemMaking");
    }
    public void BookmarkToProblemMaking()
    {
        SceneManager.LoadScene("ProblemMaking");
    }
    public void HomeToProblemMaking()
    {
        SceneManager.LoadScene("ProblemMaking");
    }

    // ProblemMaking�� �������� ȭ�� ��ȯ
    public void ProblemMakingToProblemPaper()
    {
        SceneManager.LoadScene("ProblemPaper");
    }
    public void ProblemPaperToProblemMaking()
    {
        SceneManager.LoadScene("ProblemMaking");
    }
    public void ProblemPaperToAnswer()
    {
        SceneManager.LoadScene("Answer");
    }
    public void AnswerToProblemPaper()
    {
        SceneManager.LoadScene("ProblemPaper");
    }
    public void AnswerToHome()
    {
        SceneManager.LoadScene("Home");
    }
}
