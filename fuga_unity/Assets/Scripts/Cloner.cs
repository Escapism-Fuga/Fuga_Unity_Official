using Treegen;
using UnityEngine;

public class CloneOnPress : MonoBehaviour
{
    public GameObject arbreOriginel;
    public TreegenTreeGenerator treegenTreeGenerator;

    // Variables pour sauvegarder les propri�t�s de TreeGenerator
    private float savedTreeNoiseForce;
    private float savedTrunkThickness;
    private Vector3 savedLeavesScale;

    private float resetTimer = 0f;
    private bool isResetPending = false;

    private int cloneCount = 0;  // Compteur pour savoir quel clone on cr�e

    // Position de l'arbre originel que tu veux restaurer apr�s r�initialisation
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
        // Cr�er un clone de l'arbre originel
        GameObject clone = Instantiate(arbreOriginel);

        // Calculer la position en fonction du nombre de clones
        float offset = cloneCount * -20f;  // D�caler chaque clone de 20 unit�s sur l'axe X (exemple n�gatif)
        clone.transform.position = new Vector3(offset, -2.70000005f, offset); // Utiliser l'offset sur l'axe X

        // Incr�menter le compteur de clones
        cloneCount++;
    }

    void ResetArbreOriginel()
    {
        if (treegenTreeGenerator != null)
        {
            // R�initialiser les propri�t�s de TreeGenerator avec les valeurs sauvegard�es
            treegenTreeGenerator.TreeNoiseForce = savedTreeNoiseForce;
            treegenTreeGenerator.TrunkThickness.keys[1].value = Mathf.Clamp(savedTrunkThickness, 0.1f, 2.0f);
            treegenTreeGenerator.LeavesScale = savedLeavesScale;

            // Reg�n�rer l'arbre avec les valeurs restaur�es
            treegenTreeGenerator.NewGen();
        }

        // R�initialiser la position de l'arbre originel
        arbreOriginel.transform.position = originalPosition;

        // Activer l'arbre originel si jamais il a �t� d�sactiv�
        arbreOriginel.SetActive(true);
    }
}
