using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Manager : MonoBehaviour
{
    public static Manager instance; // Singleton instance

    public TextMeshProUGUI penizeText;  // Odkaz na UI Text pro zobrazení penìz
    private int penize = 0;   // Poèet penìz
    private bool pauser = true;

    /*void Awake()
    {
        // Singleton - zajistí, že existuje pouze jedna instance GameManageru
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Zachová GameManager mezi scénami
        }
        else
        {
            Destroy(gameObject);
        }
    }*/

    // Metoda pro pøidání penìz
    public void AddMoney(int amount)
    {
        penize += amount;
    }

    // Aktualizace UI textu
    void Update()
    {
        if (penizeText != null)
        {
            penizeText.text = penize.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauser == true)
            {
                Time.timeScale = 0;
                pauser = false;
            }
            else if (pauser == false)
            {
                Time.timeScale = 1;
                pauser = true;
            }
        }
    }
}
