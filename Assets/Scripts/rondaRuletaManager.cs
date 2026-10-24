using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class rondaRuletaManager : MonoBehaviour
{
    [SerializeField] private float rouletteTime = 3f;

    [SerializeField] private float rouletteRotation = 180.0f;

    [SerializeField] private int numberOfSections = 8;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private int roundNumber = 5;
    private int currentRound = 0;

    private bool isSpinning;
    private bool isRouletteFinished;

    [SerializeField] GameObject roulette;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !isSpinning && !isRouletteFinished)
        {
            RotateRoulette();
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
        yield return new WaitForSeconds(rouletteTime);

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

    }

    private void RouletteValueCheck()
    {
        float rotation = roulette.transform.eulerAngles.z;

        float sectionSize = 360f / numberOfSections;

        int section = Mathf.FloorToInt(rotation / sectionSize);

        if (section == 0 || section == 2 || section == 4 || section == 6)
        {
            gameManager.AddScoreHigh();
            AnimacionAcierto();
        }
        else
        {
            gameManager.SubtractScoreBig();
            AnimacionFallo();
        }

        StartCoroutine(ResetAnimation());
    }

    private void AnimacionAcierto()
    {
        playerAnimator.SetBool("isHappy", true);
        playerAnimator.SetBool("isSad", false);
    }

    private void AnimacionFallo()
    {
        playerAnimator.SetBool("isHappy", false);
        playerAnimator.SetBool("isSad", true);
    }

    private IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(1f);

        playerAnimator.SetBool("isHappy", false);
        playerAnimator.SetBool("isSad", false);
    }

    private IEnumerator EndRound()
    {
        yield return new WaitForSeconds(2f);

        gameManager.GetRuletaScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
