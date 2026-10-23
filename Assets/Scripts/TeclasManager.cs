using UnityEngine;
using UnityEngine.InputSystem;

public class TeclasManager : MonoBehaviour
{
    public int id;
    public int orderId;
    public CanvasManager canvasManager;

    [SerializeField] public int state = 0;

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite spriteD;
    public Sprite spriteF;
    public Sprite spriteJ;
    public Sprite spriteK;

    public Sprite spriteDcorrecto;
    public Sprite spriteFcorrecto;
    public Sprite spriteJcorrecto;
    public Sprite spriteKcorrecto;

    public Sprite spriteDincorrecto;
    public Sprite spriteFincorrecto;
    public Sprite spriteJincorrecto;
    public Sprite spriteKincorrecto;

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
        ActualizarSpriteSegunEstado();
    }

    private void ActualizarSpriteSegunEstado()
    {
        if (spriteRenderer == null) return;

        Sprite spriteAAsignar = null;

        switch (id)
        {
            case 0: // D
                if (state == 1) spriteAAsignar = spriteDcorrecto;
                else if (state == 2) spriteAAsignar = spriteDincorrecto;
                else spriteAAsignar = spriteD;
                break;

            case 1: // F
                if (state == 1) spriteAAsignar = spriteFcorrecto;
                else if (state == 2) spriteAAsignar = spriteFincorrecto;
                else spriteAAsignar = spriteF;
                break;

            case 2: // J
                if (state == 1) spriteAAsignar = spriteJcorrecto;
                else if (state == 2) spriteAAsignar = spriteJincorrecto;
                else spriteAAsignar = spriteJ;
                break;

            case 3: // K
                if (state == 1) spriteAAsignar = spriteKcorrecto;
                else if (state == 2) spriteAAsignar = spriteKincorrecto;
                else spriteAAsignar = spriteK;
                break;
        }

        if (spriteAAsignar != null)
        {
            spriteRenderer.sprite = spriteAAsignar;
        }
    }

}

