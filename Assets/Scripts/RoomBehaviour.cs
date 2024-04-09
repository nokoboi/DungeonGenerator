using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] wall; //0 - top; 1 - down; 2 - left; 3 - right;
    [SerializeField] private GameObject[] torchs;
    [SerializeField] private GameObject[] randomObjectPrefabs; // Prefabs de los objetos aleatorios
    [SerializeField] private GameObject spawnPoint, spawnPoint2; // Objeto vacío para instanciar objetos aleatorios
    bool connected;

    public void UpdateSala(bool[] status)
    {
        connected = false;

        for (int i = 0; i < status.Length; i++)
        {
            wall[i].SetActive(!status[i]); // Desactivamos los muros de las entradas
            torchs[i].SetActive(status[i]);
            
            // Activamos las puertas de las entradas
            if (status[i] )
            {
                connected = true;
            }


        }

        if(!connected)
        {
            Destroy(gameObject);
        }

        // Generar un número aleatorio entre 0 y 3 para determinar cuántos objetos instanciar
        int numRandomObjectsToInstantiate = Random.Range(0, 2);

        // Instanciar objetos aleatorios
        for (int i = 0; i < numRandomObjectsToInstantiate; i++)
        {
            GameObject randomObjectPrefab = randomObjectPrefabs[Random.Range(0, randomObjectPrefabs.Length)];
            GameObject spawnedObject = Instantiate(randomObjectPrefab, spawnPoint.transform);
        }
    }

}
