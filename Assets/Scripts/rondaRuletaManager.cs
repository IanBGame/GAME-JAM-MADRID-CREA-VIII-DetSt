using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class rondaRuletaManager : MonoBehaviour
{
    [SerializeField] private float minTimer = 3f;
    [SerializeField] private float maxTimer = 5f;

    [SerializeField] private float rouletteRotation = 180.0f;

    [SerializeField] private int numberOfSections = 8;

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
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !isSpinning && !rouletteFinished)
        {
            RotateRoulette();
        }

        if(isSpinning)
        {
            roulete.transform.Rotate(0f, 0f, rouletteRotation * Time.deltaTime);
        }
    }

    private void RotateRoulette()
    {
        isSpinning = true;

        StartCoroutine(RouletteTimer());
    }

    private IEnumerator RouletteTimer()
    {
        yield return new WaitForSeconds(randomTime);

        StopRoulette();
    }

    private void StopRoulette()
    {
        isSpinning = false;
        isRouletteFinished = false;

        float stopRotationvalue = roulette.transform.eulerAngles.z;

        RouletteValueCheck();
    }

    private void RouletteValueCheck()
    {
        float rotation = roulette.transform.eulerAngles.z;

        float sectionSize = 360f / numberOfSections;

        int section = Mathf.FloorToInt(rotation / sectionSize);

        if (section == 0 || section == 2 || section == 4 || section == 6)
        {
            gameManager.AddScoreMedium();
        }
        else
        {
            gameManager.SubtractScore();
        }
    }

}
