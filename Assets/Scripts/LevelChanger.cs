using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        LevelStateController.level = level;
        SceneManager.LoadScene(2);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
