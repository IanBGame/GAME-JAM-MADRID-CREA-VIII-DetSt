using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [Header("Interfaz del Minijuego")]
    [SerializeField] private GameObject burbuja;
    [SerializeField] private GameObject objetoReglas;

    [Header("Sprites Ian")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite ianNormal;
    [SerializeField] private Sprite ianSirve;

    [Header("Configuración de Fila y Spawner")]
    [SerializeField] private List<GameObject> prefabsPersonajes; 
    [SerializeField] private Transform puntoSpawnAtras; 
    [SerializeField] private Transform[] puntosFila;    
    [SerializeField] private Transform puntoSalida;
    
    private bool esperandoQueLlegueAlPunto = false;
    private List<GameObject> personajesEnFila = new List<GameObject>();
    private bool personajeActivoParaFrente = false;

    [SerializeField] public GameObject[] keysList;
    private int indiceActual = 0; 
    private int[] secuenciaCorrecta; 
    private bool estaCastigado = false;

    [Header("Interfaz (UI)")]
    [SerializeField] private TextMeshProUGUI textoTemporizador;

    [Header("Configuración de Tiempo")]
    [SerializeField] private float duracionRonda = 30f; 
    private float tiempoRestante;
    private bool rondaActiva = false;
    private bool juegoIniciado = false;

    private GameManager gameManager;

    void Start() 
    {
        gameManager = FindFirstObjectByType<GameManager>();

        tiempoRestante = duracionRonda;
        rondaActiva = false;
        juegoIniciado = false;
        Ocultar();

        if (textoTemporizador != null) textoTemporizador.text = duracionRonda.ToString() + "s";
        
        LlenarFilaInicial();
        
        if (objetoReglas != null)
        {
            StartCoroutine(RutinaQuitarReglas(5.0f));
        }
    }

    void Mostrar() { if (burbuja != null) burbuja.SetActive(true); }
    void Ocultar() { if (burbuja != null) burbuja.SetActive(false); }

    void LlenarFilaInicial()
    {
        for (int i = 0; i < puntosFila.Length; i++)
        {
            SpawnearPersonajeEnFila();
        }
    }

    void SpawnearPersonajeEnFila()
    {
        if (prefabsPersonajes.Count == 0) return;

        int indexAleatorio = Random.Range(0, prefabsPersonajes.Count);
        GameObject nuevoObjeto = Instantiate(prefabsPersonajes[indexAleatorio], puntoSpawnAtras.position, puntoSpawnAtras.rotation);
        
        Vector3 posicionActual = nuevoObjeto.transform.position;
        posicionActual.z = 0f;
        nuevoObjeto.transform.position = posicionActual;
        
        personajesEnFila.Insert(0, nuevoObjeto);
        ActualizarPosicionesFila();
        esperandoQueLlegueAlPunto = true;
    }

    void ActualizarPosicionesFila()
    {
        for (int i = 0; i < personajesEnFila.Count; i++)
        {
            if (i < puntosFila.Length)
            {
                MovimientoPersonaje mov = personajesEnFila[i].GetComponent<MovimientoPersonaje>();
                if (mov != null)
                {
                    mov.IrA(puntosFila[i]);
                }
            }
        }
    }

    void VerificarLlegadaAlFrente()
    {
        if (!juegoIniciado || rondaActiva) return;

        personajeActivoParaFrente = true;
        Mostrar();
        startRound(keysList);
        Debug.Log("El personaje del frente ha llegado al punto. ¡Inicia minijuego!");
    }

    public void startRound(GameObject[] keysList) 
    {
        if (spriteRenderer != null) { spriteRenderer.sprite = ianNormal; }
        indiceActual = 0; 
        secuenciaCorrecta = new int[keysList.Length];

        rondaActiva = true;

        List<int> idsDisponibles = new List<int> { 0, 1, 2, 3 };
        int temp = 0;

        foreach (GameObject key in keysList)
        {
            TeclasManager keyScript = key.GetComponent<TeclasManager>();

            if (keyScript != null)
            {
                if (idsDisponibles.Count == 0)
                {
                    idsDisponibles.AddRange(new int[] { 0, 1, 2, 3 });
                }

                int indiceAleatorio = Random.Range(0, idsDisponibles.Count);
                keyScript.id = idsDisponibles[indiceAleatorio];
                idsDisponibles.RemoveAt(indiceAleatorio);

                secuenciaCorrecta[temp] = keyScript.id;
                keyScript.orderId = temp;
                keyScript.state = 0;
            }
            temp++;
        }
        Debug.Log("Nueva ronda iniciada con IDs numéricos (D, F, J, K), tienes 30 segundos.");
    }

    void Update()
    {
        // 1. Detectar UNA SOLA VEZ la desaparición del objeto de reglas
        if (!juegoIniciado)
        {
            if (objetoReglas == null || !objetoReglas.activeInHierarchy)
            {
                juegoIniciado = true;
                Debug.Log("El objeto de reglas ha desaparecido. ¡Comienza el juego oficial!");

                // Si un personaje ya llegó al frente mientras leías las reglas, arranca la ronda ahora
                int indiceFrente = puntosFila.Length - 1;
                if (personajesEnFila.Count > indiceFrente)
                {
                    GameObject personajeAlFrente = personajesEnFila[indiceFrente];
                    Transform puntoDestinoFinal = puntosFila[indiceFrente];
                    if (personajeAlFrente != null && puntoDestinoFinal != null)
                    {
                        float distancia = Vector3.Distance(personajeAlFrente.transform.position, puntoDestinoFinal.position);
                        if (distancia < 0.5f)
                        {
                            esperandoQueLlegueAlPunto = false;
                            VerificarLlegadaAlFrente();
                        }
                    }
                }
            }
            else
            {
                // Si aún están las reglas en pantalla, no procesamos la lógica de la partida
                return;
            }
        }

        // 2. Control del temporizador de la ronda
        if (rondaActiva)
        {
            tiempoRestante -= Time.deltaTime;

            if (textoTemporizador != null)
            {
                textoTemporizador.text = Mathf.Ceil(tiempoRestante).ToString() + "s";
            }

            if (tiempoRestante <= 0)
            {
                TiempoAgotado();
            }
        }

        // 3. Comprobar llegada del personaje al frente de la fila
        if (esperandoQueLlegueAlPunto && personajesEnFila.Count > 0)
        {
            int indiceFrente = puntosFila.Length - 1;

            if (personajesEnFila.Count > indiceFrente)
            {
                GameObject personajeAlFrente = personajesEnFila[indiceFrente]; 
                Transform puntoDestinoFinal = puntosFila[indiceFrente]; 

                if (personajeAlFrente != null && puntoDestinoFinal != null)
                {
                    float distancia = Vector3.Distance(personajeAlFrente.transform.position, puntoDestinoFinal.position);
                    
                    if (distancia < 0.5f)
                    {
                        esperandoQueLlegueAlPunto = false;
                        VerificarLlegadaAlFrente();
                    }
                }
            }
        }

        // 4. Procesamiento del teclado
        if (Keyboard.current != null)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                foreach (var key in Keyboard.current.allKeys)
                {
                    if (key.wasPressedThisFrame)
                    {
                        string teclaPresionada = key.name.ToLower();
                        ProcesarEntradaTeclado(teclaPresionada);
                        break; 
                    }
                }
            }
        }
    }

    private void TiempoAgotado()
    {   
        rondaActiva = false;
        personajeActivoParaFrente = false;
        Debug.LogWarning("¡TIEMPO AGOTADO! Se acabó la ronda.");
        Ocultar();
    }

    private void ProcesarEntradaTeclado(string teclaPresionada)
    {
        int idPresionado = -1;

        if (estaCastigado || !rondaActiva) return;

        if (teclaPresionada == "d") idPresionado = 0;
        else if (teclaPresionada == "f") idPresionado = 1;
        else if (teclaPresionada == "j") idPresionado = 2;
        else if (teclaPresionada == "k") idPresionado = 3;

        if (idPresionado == -1) return;

        TeclasManager teclaEsperada = keysList[indiceActual].GetComponent<TeclasManager>();

        if (teclaEsperada != null)
        {
            if (idPresionado == teclaEsperada.id)
            {
                teclaEsperada.state = 1; 
                indiceActual++;

                if (indiceActual >= keysList.Length)
                {
                    Debug.Log("¡GANASTE! Combinación completada.");
                    StartCoroutine(RutinaVictoria(0.5f));
                }
            }
            else
            {
                teclaEsperada.state = 2; 
                StartCoroutine(RutinaCastigoFallo(0.5f));
            }
        }
    }

    private System.Collections.IEnumerator RutinaVictoria(float tiempoEspera)
    {
        if (spriteRenderer != null) { spriteRenderer.sprite = ianSirve; }

        gameManager.AddScoreMedium();
        rondaActiva = false;
        personajeActivoParaFrente = false; 
        Ocultar();
        yield return new WaitForSeconds(tiempoEspera); 

        int indiceFrente = puntosFila.Length - 1;

        if (personajesEnFila.Count > indiceFrente)
        {
            GameObject personajeQueSeVa = personajesEnFila[indiceFrente]; 
            personajesEnFila.RemoveAt(indiceFrente); 

            MovimientoPersonaje scriptMov = personajeQueSeVa.GetComponent<MovimientoPersonaje>();
            if (scriptMov != null)
            {
                scriptMov.Retirarse(puntoSalida);
            }
        }

        SpawnearPersonajeEnFila();
    }

    private System.Collections.IEnumerator RutinaCastigoFallo(float tiempoCastigo)
    {
        gameManager.SubtractScoreBig();
        estaCastigado = true;
        yield return new WaitForSeconds(tiempoCastigo);
        indiceActual = 0;

        foreach (GameObject obj in keysList)
        {
            TeclasManager tm = obj.GetComponent<TeclasManager>();
            if (tm != null)
            {
                tm.state = 0;
            }
        }      

        estaCastigado = false;
        Debug.Log("Fin del castigo. ¡A jugar de nuevo!");
    }

    private System.Collections.IEnumerator RutinaQuitarReglas(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        foreach (GameObject obj in keysList)
        {
            TeclasManager tm = obj.GetComponent<TeclasManager>();
            if (tm != null)
            {
                tm.state = 0;
            }
        }      
        
        if (objetoReglas != null)
        {
            objetoReglas.SetActive(false);
        }
    }
}
