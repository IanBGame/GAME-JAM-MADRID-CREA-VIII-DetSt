using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;
using Debug = UnityEngine.Debug;

public class CanvasManager : MonoBehaviour
{
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

    void Start() 
    {
        startRound(keysList);
    }

    public void startRound(GameObject[] keysList) 
    {
        indiceActual = 0; 
        secuenciaCorrecta = new int[keysList.Length];

        tiempoRestante = duracionRonda;
        rondaActiva = true;

        // Lista de IDs únicos disponibles (0 = D; 1 = F; 2 = J; 3 = K)
        List<int> idsDisponibles = new List<int> { 0, 1, 2, 3 };
        int temp = 0;

        foreach (GameObject key in keysList)
        {
            TeclasManager keyScript = key.GetComponent<TeclasManager>();

            if (keyScript != null)
            {
                // Si hay más teclas en el array que IDs en la lista, volvemos a llenarla
                if (idsDisponibles.Count == 0)
                {
                    idsDisponibles.AddRange(new int[] { 0, 1, 2, 3 });
                }

                // Selección aleatoria sin repetición consecutiva inmediata
                int indiceAleatorio = Random.Range(0, idsDisponibles.Count);
                keyScript.id = idsDisponibles[indiceAleatorio];
                idsDisponibles.RemoveAt(indiceAleatorio);

                // Guardamos el ID en el orden correcto que el jugador debe presionar
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
        if (rondaActiva)
        {
            tiempoRestante -= Time.deltaTime;

            // Actualizar el texto en pantalla (redondeando hacia arriba)
            if (textoTemporizador != null)
            {
                textoTemporizador.text = Mathf.Ceil(tiempoRestante).ToString() + "s";
            }

            if (tiempoRestante <= 0)
            {
                TiempoAgotado();
            }
        }

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
        Debug.LogWarning("¡TIEMPO AGOTADO! Se acabó el tiempo de la ronda.");
        
        // Aquí puedes decidir qué pasa cuando se acaba el tiempo:
        // Opción A: Reiniciar la misma ronda automáticamente
        // IniciarNuevaRonda();

        // Opción B: Detener el juego o invocar un menú de Game Over
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

        Debug.Log($"Teclado físico detectó: {teclaPresionada} (ID: {idPresionado})");

        TeclasManager teclaEsperada = keysList[indiceActual].GetComponent<TeclasManager>();

        if (teclaEsperada != null)
        {
            if (idPresionado == teclaEsperada.id)
            {
                Debug.Log($"¡CORRECTO! Completaste el paso {indiceActual}");
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
                Debug.LogWarning($"¡FALLO! Se esperaba ID {teclaEsperada.id}. Secuencia reiniciada.");
                teclaEsperada.state = 2; 
                StartCoroutine(RutinaCastigoFallo(0.5f));
            }
        }
    }

    private System.Collections.IEnumerator RutinaVictoria(float tiempoEspera)
    {
        rondaActiva = false; 
        yield return new WaitForSeconds(tiempoEspera); 
        startRound(keysList); 
    }

    private System.Collections.IEnumerator RutinaCastigoFallo(float tiempoCastigo)
    {
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
}


/*Para usar el InputAction, cambia update por este codigo e implementar la accion tecla presionada:

public void OnPresionarTecla(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        // 1. Detectamos el nombre de la tecla física presionada
        string teclaPresionada = context.control.name.ToLower();
        int idPresionado = -1; // -1 significa que presionó una tecla no válida para el juego

        // 2. Traducimos la tecla física a tu sistema de IDs
        if (teclaPresionada == "d") idPresionado = 0;
        else if (teclaPresionada == "f") idPresionado = 1;
        else if (teclaPresionada == "j") idPresionado = 2;
        else if (teclaPresionada == "k") idPresionado = 3;

        // Si el jugador presionó cualquier otra tecla que no sea D, F, J o K, la ignoramos por completo
        if (idPresionado == -1) return;

        // 3. Obtenemos el script de la tecla que toca presionar en este paso de la lista
        TeclasManager teclaEsperada = keysList[indiceActual].GetComponent<TeclasManager>();

        if (teclaEsperada != null)
        {
            // 4. Comparamos el ID de la tecla física contra el ID guardado en el objeto de pantalla
            if (idPresionado == teclaEsperada.id)
            {
                Debug.Log($"¡CORRECTO! Presionaste la tecla física asociada al ID {idPresionado}.");
                indiceActual++; // Avanzamos en la combinación

                if (indiceActual >= keysList.Length)
                {
                    Debug.Log("¡GANASTE! Has completado toda la secuencia perfectamente.");
                    // Aquí puedes llamar a tu lógica de victoria o cambiar de nivel
                }
            }
            else
            {
                // REINICIO AUTOMÁTICO SI FALLA
                Debug.LogWarning($"¡FALLO! Presionaste el ID {idPresionado} pero se esperaba el ID {teclaEsperada.id}. Secuencia reiniciada.");
                indiceActual = 0;
                
                // Descomenta la siguiente línea si quieres que la combinación cambie totalmente al equivocarse:
                // startRound(keysList);
            }
        }
    }
}
*/
