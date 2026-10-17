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
        //for(int i = 0; vegetalSpawnPoints.GetSize() > i; i++)
        //{
        //    GameObject vegetalSpawneado = Instantiate(vegetalPrefab, spawnPoint.position);
        //    if(i + 1 == numeroVegetalMalo)
        //    {
        //        vegetalSpawneado.GetComponent<VegetalController>.isBad = true;
        //    }
        //    else
        //    {
        //        vegetalSpawneado.GetComponent<VegetalController>.isBad = false;
        //    }
        //}
    }
}
