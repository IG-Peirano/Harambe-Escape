using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int levelCount = 50;
    public Text banana;
    public Text distance;
    public Camera camera;
    public GameObject guiGameOver;
    public LevelGenerator levelGenerator = null;
    private int currentPoints = 0;
    private int currentDistance = 0;
    private bool canPlay = false;

    private static GameManager s_Instance; // access for other scripts, using all public methods and variables in this class
    public static GameManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            return s_Instance;
        }

    }

    private void Start()
    {
        for (int i = 0; i < levelCount; i++)
        {
            levelGenerator.RandomGenerator();
        }
    }

    public void UpdateBananaCount (int value)
    {
        Debug.Log("Player picked up another banana for " + value);

        currentPoints += value;

        banana.text = currentPoints.ToString();
    }

    public void UpdateDistanceCount()
    {
        Debug.Log("Player moved forward for one point");

        currentDistance += 1;

        distance.text = currentDistance.ToString();

        levelGenerator.RandomGenerator(); // generating one layer each time the player moves forward

    }

    public bool CanPlay()
    {
        return canPlay;
    }

    public void StartPlay()
    {
        canPlay = true;
    }

    public void GameOver()
    {
        camera.GetComponent<CameraShake>().Shake();
        camera.GetComponent<CameraFollow>().enabled = false;
        
        GuiGameOver();


    }

    void GuiGameOver()
    {
        Debug.Log("Game over :(");

        guiGameOver.SetActive(true);

    }

    public void PlayAgain()
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
