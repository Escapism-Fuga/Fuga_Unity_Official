using Treegen;
using UnityEngine;

public class CloneOnPress : MonoBehaviour
{
    public GameObject arbreOriginel;
    public TreegenTreeGenerator treegenTreeGenerator;

    // Variables pour sauvegarder les propriétés de TreeGenerator
    private float savedTreeNoiseForce;
    private float savedTrunkThickness;
    private Vector3 savedLeavesScale;

    private float resetTimer = 0f;
    private bool isResetPending = false;

    private int cloneCount = 0;  // Compteur pour savoir quel clone on crée

    // Position de l'arbre originel que tu veux restaurer après réinitialisation
    private Vector3 originalPosition = new Vector3(-0.368030012f, -2.70000005f, 0.588739991f);

    void Start()
    {
        if (arbreOriginel != null)
        {
            treegenTreeGenerator = arbreOriginel.GetComponent<TreegenTreeGenerator>();

            if (treegenTreeGenerator != null)
            {
                savedTreeNoiseForce = treegenTreeGenerator.TreeNoiseForce;
                savedTrunkThickness = treegenTreeGenerator.TrunkThickness.keys[1].value;
                savedLeavesScale = treegenTreeGenerator.LeavesScale;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CloneAndMove();
            isResetPending = true;
            resetTimer = 3f;
        }

        if (isResetPending)
        {
            resetTimer -= Time.deltaTime;

            if (resetTimer <= 0f)
            {
                ResetArbreOriginel();
                isResetPending = false;
            }
        }
    }

    void CloneAndMove()
    {
        // Créer un clone de l'arbre originel
        GameObject clone = Instantiate(arbreOriginel);

        // Calculer la position en fonction du nombre de clones
        float offset = cloneCount * -20f;  // Décaler chaque clone de 20 unités sur l'axe X (exemple négatif)
        clone.transform.position = new Vector3(offset, -2.70000005f, offset); // Utiliser l'offset sur l'axe X

        // Incrémenter le compteur de clones
        cloneCount++;
    }

    void ResetArbreOriginel()
    {
        if (treegenTreeGenerator != null)
        {
            // Réinitialiser les propriétés de TreeGenerator avec les valeurs sauvegardées
            treegenTreeGenerator.TreeNoiseForce = savedTreeNoiseForce;
            treegenTreeGenerator.TrunkThickness.keys[1].value = Mathf.Clamp(savedTrunkThickness, 0.1f, 2.0f);
            treegenTreeGenerator.LeavesScale = savedLeavesScale;

            // Regénérer l'arbre avec les valeurs restaurées
            treegenTreeGenerator.NewGen();
        }

        // Réinitialiser la position de l'arbre originel
        arbreOriginel.transform.position = originalPosition;

        // Activer l'arbre originel si jamais il a été désactivé
        arbreOriginel.SetActive(true);
    }
}
