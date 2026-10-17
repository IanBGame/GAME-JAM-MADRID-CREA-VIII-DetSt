using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnRondaAplastarEnd()
    {

    }

    public void OnRondaPañueloEnd()
    {

    }

    public void OnRondaRuletaEnd()
    {

    }
}
