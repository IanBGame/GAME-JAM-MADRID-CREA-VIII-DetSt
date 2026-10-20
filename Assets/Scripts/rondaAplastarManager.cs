using UnityEngine;
using Random = UnityEngine.Random;

public class rondaAplastarManager : MonoBehaviour
{
    [SerializeField] private Transform[] vegetalSpawnPoints;

    [SerializeField] private GameObject vegetalPrefab;

    private int numeroVegetalMalo;



    private void RandomizarVegetal()
    {
        numeroVegetalMalo = Random.Range(1, 4);
    }

    private void SpawnVegetal()
    {
        for (int i = 0; vegetalSpawnPoints.Length > i; i++)
        {
            GameObject vegetalSpawneado = Instantiate(vegetalPrefab,vegetalSpawnPoints[i].position,vegetalSpawnPoints[i].rotation);
            VegetalController vegetalController = vegetalSpawneado.GetComponent<VegetalController>();
            if (i + 1 == numeroVegetalMalo)
            {
                vegetalController.isBad = true;
            }
            else
            {
                vegetalController.isBad = false;
            }
        }
    }
}
