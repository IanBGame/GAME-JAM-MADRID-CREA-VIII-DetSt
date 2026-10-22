using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class rondaAplastarManager : MonoBehaviour
{
    [SerializeField] private Transform[] vegetalSpawnPoints;

    [SerializeField] private GameObject vegetalPrefab;
    [SerializeField] private GameObject playerPrefab;

    private int numeroVegetalMalo;

    private List<GameObject> vegetalesSpawneados = new List<GameObject>();

    private bool rondaOver;

    void Start()
    {
        SpawnVegetal();
    }

    private void RandomizarVegetal()
    {
        numeroVegetalMalo = Random.Range(1, 4);
    }

    private void SpawnVegetal()
    {
        rondaOver = false;

        RandomizarVegetal();


        for (int i = 0; vegetalSpawnPoints.Length > i; i++)
        {
            GameObject vegetalSpawneado = Instantiate(vegetalPrefab,vegetalSpawnPoints[i].position,vegetalSpawnPoints[i].rotation);
            VegetalController vegetalController = vegetalSpawneado.GetComponent<VegetalController>();

            vegetalController.Initialize(i == numeroVegetalMalo, this);

            vegetalesSpawneados.Add(vegetalSpawneado);
        }
    }

    public void VegetalSeleccionado(bool isBad)
    {
        if(rondaOver)
        {
            return;
        }

        rondaOver = true;

        if(isBad)
        {
            //RELLENAR PUNTOS
        }
        else
        {
            //RELLENAR PUNTOS
        }

        StartCoroutine(SiguienteRonda());
    }

    private void DestruirVegetales()
    {
        foreach(GameObject vegetal in vegetalesSpawneados)
        {
            Destroy(vegetal);
        }

        vegetalesSpawneados.Clear();
    }

    private IEnumerator SiguienteRonda()
    {
        yield return new WaitForSeconds(1f);

        DestruirVegetales();
        SpawnVegetal();
    }
}
