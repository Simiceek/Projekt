using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Manager : MonoBehaviour
{
    public static Manager instance; // Singleton instance

    public TextMeshProUGUI penizeText;  // Odkaz na UI Text pro zobrazen� pen�z
    private int penize = 0;   // Po�et pen�z
    private bool pauser = true;

    /*void Awake()
    {
        // Singleton - zajist�, �e existuje pouze jedna instance GameManageru
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Zachov� GameManager mezi sc�nami
        }
        else
        {
            Destroy(gameObject);
        }
    }*/

    // Metoda pro p�id�n� pen�z
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
