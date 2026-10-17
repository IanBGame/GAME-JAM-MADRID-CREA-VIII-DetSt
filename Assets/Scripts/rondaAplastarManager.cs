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
        
    }
}
