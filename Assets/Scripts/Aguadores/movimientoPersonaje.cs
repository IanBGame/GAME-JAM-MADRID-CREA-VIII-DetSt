using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    private Transform destinoActual;
    private float velocidad = 5f;
    private bool enMovimiento = false;
    private bool esParaRetirarse = false;

    void Update()
    {
        if (enMovimiento && destinoActual != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinoActual.position, velocidad * Time.deltaTime);

            if (Vector3.Distance(transform.position, destinoActual.position) < 0.1f)
            {
                enMovimiento = false;
                
                if (esParaRetirarse)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void IrA(Transform nuevoDestino)
    {
        destinoActual = nuevoDestino;
        enMovimiento = true;
        esParaRetirarse = false;
    }

    public void Retirarse(Transform puntoSalida)
    {
        destinoActual = puntoSalida;
        enMovimiento = true;
        esParaRetirarse = true;
    }
}