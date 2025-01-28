using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    // Tableau de r�f�rences aux trois prefabs
    public GameObject[] treegenAssets;

    // Plage al�atoire pour la position du GameObject
    public Vector3 spawnRange = new Vector3(50f, 0f, 50f); // Plage ajust�e � 50

    // Position de base o� le GameObject peut appara�tre
    public Vector3 spawnPosition = new Vector3(0, 0, 0);

    void Start()
    {
        // Charger tous les prefabs du dossier "Resources/Prefabs/Arbres"
        treegenAssets = Resources.LoadAll<GameObject>("Prefabs/Arbres");

        // V�rifier si des assets ont �t� charg�s
        if (treegenAssets.Length == 0)
        {
            Debug.LogError("Aucun prefab trouv� dans le dossier 'Resources/Prefabs/Arbres'");
        }
    }

    void Update()
    {
        // V�rifie si la touche "T" est press�e
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Calcule une position al�atoire dans la plage sp�cifi�e
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnRange.x, spawnRange.x) + spawnPosition.x,
                spawnPosition.y, // La position Y reste fixe
                Random.Range(-spawnRange.z, spawnRange.z) + spawnPosition.z
            );

            // Affiche la position g�n�r�e dans la console
            Debug.Log("Position al�atoire g�n�r�e: " + randomPosition);

            // Cr�e une rotation al�atoire (par exemple sur l'axe Y)
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // V�rifie qu'il y a au moins un prefab dans le tableau
            if (treegenAssets.Length > 0)
            {
                // Choisir un prefab al�atoire
                GameObject selectedPrefab = treegenAssets[Random.Range(0, treegenAssets.Length)];

                // Instancier le prefab s�lectionn� � la position et rotation al�atoires
                Instantiate(selectedPrefab, randomPosition, randomRotation);
            }
            else
            {
                Debug.LogError("Aucun prefab assign� dans le tableau !");
            }
        }
    }
}
