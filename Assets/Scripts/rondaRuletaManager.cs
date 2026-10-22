using UnityEngine;
using TMPro;

public class rondaRuletaManager : MonoBehaviour
{
    [Header("¡¡¡EL VALOR SE REDUCE POR 50 CADA SEGUNDO!!!")]

    [SerializeField] private int rouletteTimer;

    [SerializeField] private float rouletteRotation = 180.0f;

    [SerializeField] private int numberOfSections = 8;

    [SerializeField] private TextMeshProUGUI buttonText;

    private bool isSpinning;

    GameObject roulette;

    private void FixedUpdate()
    {
        if(isSpinning == true)
        {
            roulette.transform.Rotate(0.0f, 0.0f, rouletteRotation * Time.deltaTime);
        }
    }

    private void RotateRoulette()
    {
        isSpinning = true;
        buttonText.text = "PARAR";
    }

    private void StopRoulette()
    {
        //Alomejor hacer que no pare repentinamente y tarde un poco?
        isSpinning = false;

        float stopRotationvalue = roulette.transform.eulerAngles.z;

        RouletteValueCheck();
        buttonText.text = "GIRAR";
    }

    private void RouletteValueCheck()
    {
        float rotation = roulette.transform.eulerAngles.z;

        float sectionSize = 360f / numberOfSections;

        int section = Mathf.FloorToInt(rotation /45f);
    }

    public void RouletteButtonHit()
    {
        if(isSpinning)
        {
            StopRoulette();
        }
        else
        {
            RotateRoulette();
        }
    }

}
