using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GuardarPuntuacionFinall : MonoBehaviour
{
    [SerializeField] private TMP_InputField nombreJugadorInput;

    private LocalLeaderboard leaderboard;
    private GameManager gameManager;

    void Start()
    {
        leaderboard = FindFirstObjectByType<LocalLeaderboard>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void GuardarPuntuacion()
    {
        string nombre = nombreJugadorInput.text.Trim();

        if(string.IsNullOrEmpty(nombre))
        {
            return;
        }

        if(leaderboard == null || gameManager == null)
        {
            return;
        }

        int puntos = gameManager.GetScore();

        leaderboard.AddEntry(nombre, puntos);

        SceneManager.LoadScene(0);
    }
}
