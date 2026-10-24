using UnityEngine;
using TMPro;

public class puntosRondaScript : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] TMP_Text textoPunto;

    [SerializeField] bool isAplastarText;
    [SerializeField] bool isPanueloText;
    [SerializeField] bool isRuletaText;
    [SerializeField] bool isAguaText;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if(isAplastarText)
        {
            textoPunto.text = gameManager.GetAplastarScore().ToString();
        }
        else if(isPanueloText)
        {
            textoPunto.text = gameManager.GetPanueloScore().ToString();
        }
        else if(isRuletaText)
        {
            textoPunto.text = gameManager.GetRuletaScore().ToString();
        }
        else if(isAguaText)
        {
            textoPunto.text = gameManager.GetAguadorScore().ToString();
        }
    }
}
