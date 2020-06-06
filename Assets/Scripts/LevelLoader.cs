using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    int levelIndex;
    [SerializeField] float timeToDelay = 2f;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.L)) // FOR DEBUG AND TESTING
            {
                AdminLoadNextScene();
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.Quit();
        }

    }

    public void LoadNextScene()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex + 1);
    }

    public void RestartScene()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex);
    }

    public void AdminLoadNextScene()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = levelIndex + 1;
        if(nextLevelIndex == SceneManager.sceneCountInBuildSettings) // проверка на наличие следующей сцены в билде
        {
            nextLevelIndex = 0;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }
}
