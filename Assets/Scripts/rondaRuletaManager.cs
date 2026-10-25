using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static System.Collections.Specialized.BitVector32;
using Random = UnityEngine.Random;

public class rondaRuletaManager : MonoBehaviour
{
    [SerializeField] private GameObject objetoReglas;

    [SerializeField] private float minRouletteTime = 2f;
    [SerializeField] private float maxRouletteTime = 5f;

    [SerializeField] private float rouletteRotation = 180.0f;

    [SerializeField] private int numberOfSections = 8;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private int roundNumber = 5;
    private int currentRound = 0;
    private int section;

    private bool canSpin = false;
    private bool isSpinning;
    private bool isRouletteFinished;

    [SerializeField] GameObject roulette;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (objetoReglas != null)
        {
            StartCoroutine(RutinaQuitarReglas(5.0f));
        }
    }

    private void Update()
    {

        if (canSpin)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !isSpinning && !isRouletteFinished)
            {
                RotateRoulette();
            }
        }
        
        if(isSpinning)
        {
            roulette.transform.Rotate(0f, 0f, rouletteRotation * Time.deltaTime);
        }
    }

    private void RotateRoulette()
    {
        isSpinning = true;

        StartCoroutine(RouletteTimer());
    }

    private IEnumerator RouletteTimer()
    {
        float randomTime = Random.Range(minRouletteTime, maxRouletteTime);

        yield return new WaitForSeconds(randomTime);

        StopRoulette();
    }

    private void StopRoulette()
    {
        isSpinning = false;

        RouletteValueCheck();

        currentRound++;

        if (currentRound >= roundNumber)
        {
            isRouletteFinished = true;
            StartCoroutine(EndRound());
        }

        if (section == 0 || section == 2 || section == 4 || section == 6)
        {
            AnimacionAcierto();
        }
        else
        {
            AnimacionFallo();
        }

        StartCoroutine(ResetAnimation());

    }

    private void RouletteValueCheck()
    {
        float rotation = roulette.transform.eulerAngles.z;

        float sectionSize = 360f / numberOfSections;

        section = Mathf.FloorToInt(rotation / sectionSize);

        if (section == 0 || section == 2 || section == 4 || section == 6)
        {
            gameManager.AddScoreHigh();
        }
        else
        {
            gameManager.SubtractScoreBig();
        }

    }

    private void AnimacionAcierto()
    {
        playerAnimator.SetBool("IsHappy", true);
        playerAnimator.SetBool("IsSad", false);
    }

    private void AnimacionFallo()
    {
        playerAnimator.SetBool("IsHappy", false);
        playerAnimator.SetBool("IsSad", true);
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(1f);

        playerAnimator.SetBool("IsHappy", false);
        playerAnimator.SetBool("IsSad", false);
    }

    private IEnumerator EndRound()
    {
        yield return new WaitForSeconds(2f);

        gameManager.GetRuletaScore();
        gameManager.ResetTempScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private System.Collections.IEnumerator RutinaQuitarReglas(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        
        if (objetoReglas != null)
        {
            objetoReglas.SetActive(false);
        }
        canSpin = true;
    }

}
