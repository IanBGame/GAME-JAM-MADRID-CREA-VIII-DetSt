using UnityEngine;
using Random = UnityEngine.Random;

public class VegetalController : MonoBehaviour
{
    [SerializeField] private Sprite[] vegetableSkins;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private int vegetableType;
    public bool isGood;

    private rondaAplastarManager manager;

    public void Initialize(bool good)
    {
        isGood = good;

        int skinNumber;

        if(isGood)
        {
            skinNumber = Random.Range(0, 3);
        }
        else
        {
            skinNumber = Random.Range(3, 6);
        }

        spriteRenderer.sprite = vegetableSkins[skinNumber];
    }

  
}
