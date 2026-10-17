using UnityEngine;

public class rondaRuletaManager : MonoBehaviour
{
    [Header("¡¡¡EL VALOR SE REDUCE POR 50 CADA SEGUNDO!!!")]

    [SerializeField] private int rouletteTimer;

    [SerializeField] private float rouletteRotation = 35.0f;

    private bool isSpinning;

    GameObject roulette;

    private void FixedUpdate()
    {
        if(isSpinning == true)
        {
            roulette.transform.Rotate(rouletteRotation, 0.0f, 0.0f, Space.Self);
        }
    }

    private void RotateRoulette()
    {
        isSpinning = true;
    }

    private void StopRoulette()
    {
        //Alomejor hacer que no pare repentinamente y tarde un poco?
        isSpinning = false;
    }

    private void RouletteValueCheck()
    {
        /*
         *PONER IF CON LA ROTACION DE CADA COSA Y APLICAR PUNTOS RESPECTIVAMENTE. 
         *
         */
    }


}
