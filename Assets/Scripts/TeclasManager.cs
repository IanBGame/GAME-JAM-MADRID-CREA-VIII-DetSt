using UnityEngine;
using UnityEngine.InputSystem;

public class TeclasManager : MonoBehaviour
{
    public int id;
    public int orderId;
    public CanvasManager canvasManager;

    public int randomizeKey() //Randomiza la tecla
    {
        int nuevoId = Random.Range(0, 4); 
        return nuevoId; 
    }

    // Al empezar, asigna el CanvasManager a cada tecla para poder recibir acciones
    void Start()
    {
        canvasManager = Object.FindAnyObjectByType<CanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

