using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playagain()
    {
        // Na�te sc�nu s n�zvem "Game Over"
        SceneManager.LoadScene(0);
    }
}
