using UnityEngine;
using Random = UnityEngine.Random;

public class VegetalController : MonoBehaviour
{
    [SerializeField] private Sprite[] vegetableSkins;

    [SerializeField] private SpriteRenderer visualContainer;

    private int vegetableType;
    public bool isBad;

    private void Awake()
    {
        vegetableType = Random.Range(1, 4);
    
        if(vegetableType == 1)
        {
            //SpriteRenderer.Sprite = vegetableSkins[vegetableType];
            CheckSkin();
        }
        else if(vegetableType == 2)
        {
            //SpriteRenderer.Sprite = vegetableSkins[vegetableType];
            CheckSkin();
        }
        else if(vegetableType == 3)
        {
            //SpriteRenderer.Sprite = vegetableSkins[vegetableType];
            CheckSkin();
        }
    }

    public void CheckSkin()
    {
        if(!isBad)
        {
            //SpriteRenderer.Sprite = vegetableSkins[vegetableType + 3];
        }
        else
        {
            //SpriteRenderer.Sprite = vegetableSkins[vegetableType + 3];
        }
    }
}
