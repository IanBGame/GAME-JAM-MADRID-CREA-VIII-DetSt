using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] public GameObject[] keysList;
    private int indiceActual = 0; 
    private int[] secuenciaCorrecta; 

    void Start() //Genera una ronda de teclas con su secuencia
    {
        startRound(keysList);
    }

    public void startRound(GameObject[] keysList) //Funcion que genera secuencia y asigna teclas
    {
        indiceActual = 0; 
        secuenciaCorrecta = new int[keysList.Length];

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
            }
            temp++;
        }
        Debug.Log("Nueva ronda iniciada con IDs numéricos (D, F, J, K).");
    }

    void Update()
    {
    // Revisamos si el teclado actual está activo en este frame
    if (Keyboard.current != null)
    {
        // anyKey.wasPressedThisFrame detecta instantáneamente cualquier pulsación física
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            // Buscamos cuál fue exactamente la última tecla presionada
            foreach (var key in Keyboard.current.allKeys)
            {
                if (key.wasPressedThisFrame)
                {
                    string teclaPresionada = key.name.ToLower();
                    
                    // Ejecutamos tu lógica de conversión a ID directamente
                    ProcesarEntradaTeclado(teclaPresionada);
                    break; 
                }
            }
        }
    }
}

private void ProcesarEntradaTeclado(string teclaPresionada)
{
    int idPresionado = -1;

    if (teclaPresionada == "d") idPresionado = 0;
    else if (teclaPresionada == "f") idPresionado = 1;
    else if (teclaPresionada == "j") idPresionado = 2;
    else if (teclaPresionada == "k") idPresionado = 3;

    // Si pulsó otra tecla diferente a D,F,J,K, la ignoramos
    if (idPresionado == -1) return;

    Debug.Log($"Teclado físico detectó: {teclaPresionada} (ID: {idPresionado})");

    TeclasManager teclaEsperada = keysList[indiceActual].GetComponent<TeclasManager>();

    if (teclaEsperada != null)
    {
        if (idPresionado == teclaEsperada.id)
        {
            Debug.Log($"¡CORRECTO! Completaste el paso {indiceActual}");
            indiceActual++;

            if (indiceActual >= keysList.Length)
            {
                Debug.Log("¡GANASTE! Combinación completada.");
            }
        }
        else
        {
            Debug.LogWarning($"¡FALLO! Se esperaba ID {teclaEsperada.id}. Secuencia reiniciada.");
            indiceActual = 0;
        }
    }
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
