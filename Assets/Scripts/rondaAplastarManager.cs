using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class rondaAplastarManager : MonoBehaviour
{
    [SerializeField] private GameObject objetoReglas;
    [SerializeField] private Transform[] vegetalSpawnPoints;

    [SerializeField] private GameObject vegetalPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private int roundNumber = 10;
    private int currentRound = 1;

    private int numeroVegetalBueno;

    private List<GameObject> vegetalesSpawneados = new List<GameObject>();

    private bool rondaOver;

    [SerializeField] private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (objetoReglas != null)
        {
            StartCoroutine(RutinaQuitarReglas(5.0f));
        }
    }

    void Update()
    {
        if(rondaOver || Keyboard.current == null)
        {
            return;
        }

        if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            playerAnimator.SetBool("leftPressed", true);
            playerAnimator.SetBool("middlePressed", false);
            playerAnimator.SetBool("rightPressed", false);
            VegetalSeleccionado(0);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            playerAnimator.SetBool("leftPressed", false);
            playerAnimator.SetBool("middlePressed", true);
            playerAnimator.SetBool("rightPressed", false);
            VegetalSeleccionado(1);
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            playerAnimator.SetBool("leftPressed", false);
            playerAnimator.SetBool("middlePressed", false);
            playerAnimator.SetBool("rightPressed", true);
            VegetalSeleccionado(2);
        }
    }

    private void RandomizarVegetal()
    {
        numeroVegetalBueno = Random.Range(0, vegetalSpawnPoints.Length);
    }

    private void SpawnVegetal()
    {
        rondaOver = false;

        RandomizarVegetal();


        for (int i = 0; vegetalSpawnPoints.Length > i; i++) 
        {
            GameObject vegetalSpawneado = Instantiate(vegetalPrefab,vegetalSpawnPoints[i].position,vegetalSpawnPoints[i].rotation);
            VegetalController vegetalController = vegetalSpawneado.GetComponent<VegetalController>();

            vegetalController.Initialize(i == numeroVegetalBueno);

            vegetalesSpawneados.Add(vegetalSpawneado);
        }
    }

    public void VegetalSeleccionado(int indice)
    {
        if(rondaOver)
        {
            return;
        }

        if (indice < 0 || indice >= vegetalSpawnPoints.Length)
        {
            return;
        }

        rondaOver = true;

        if (indice == numeroVegetalBueno)
        {
            if (gameManager != null)
            {
                gameManager.AddScoreMedium();
            }
                
        }
        else
        {
            if (gameManager != null)
            {
                gameManager.SubtractScoreBig();
            }
                
        }
        
        StartCoroutine(SiguienteRonda());
    }

    private void DestruirVegetales()
    {
        foreach(GameObject vegetal in vegetalesSpawneados)
        {
            vegetal.SetActive(false);
            Destroy(vegetal);
        }

        vegetalesSpawneados.Clear();
    }

    private IEnumerator SiguienteRonda()
    {
        yield return new WaitForSeconds(0.7f);

        playerAnimator.SetBool("leftPressed", false);
        playerAnimator.SetBool("middlePressed", false);
        playerAnimator.SetBool("rightPressed", false);
        DestruirVegetales();

        if (currentRound >= roundNumber)
        {
            gameManager.GetAplastarScore();
            gameManager.ResetTempScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            currentRound++;
            SpawnVegetal();
        }
    }

    private System.Collections.IEnumerator RutinaQuitarReglas(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        
        if (objetoReglas != null)
        {
            objetoReglas.SetActive(false);
        }
        SpawnVegetal();
    }

}
