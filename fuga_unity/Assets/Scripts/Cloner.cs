using Treegen;
using UnityEngine;

public class CloneOnPress : MonoBehaviour
{
    public GameObject arbreOriginel;
    public float distance;

    public TreegenTreeGenerator treegenTreeGenerator;

    // Variables pour sauvegarder les propri�t�s de TreeGenerator
    private float savedTreeNoiseForce;
    private float savedTrunkThickness;
    private Vector3 savedLeavesScale;

    private Vector3 initialPosition; // Pour stocker la position initiale de l'arbre
    private Quaternion initialRotation; // Pour stocker la rotation initiale de l'arbre

    private float resetTimer = 0f; // Temps qui s'�coule avant de r�initialiser
    private bool isResetPending = false;

    void Start()
    {
        // Assurer que tu as bien la r�f�rence � TreeGenerator
        if (arbreOriginel != null)
        {
            treegenTreeGenerator = arbreOriginel.GetComponent<TreegenTreeGenerator>();

            // Sauvegarder les valeurs initiales de TreeGenerator
            if (treegenTreeGenerator != null)
            {
                savedTreeNoiseForce = treegenTreeGenerator.TreeNoiseForce;
                savedTrunkThickness = treegenTreeGenerator.TrunkThickness.keys[1].value;
                savedLeavesScale = treegenTreeGenerator.LeavesScale;
            }

            initialPosition = arbreOriginel.transform.position;
            initialRotation = arbreOriginel.transform.rotation;
        }
    }

    void Update()
    {
        // V�rifie si la touche "T" est press�e pour cloner
        if (Input.GetKeyDown(KeyCode.T))
        {
            CloneAndMove();

            isResetPending = true;
            resetTimer = 3f; // Timer de 3 secondes
        }

        // Si le reset est en attente, diminuer le timer
        if (isResetPending)
        {
            resetTimer -= Time.deltaTime; // Diminue le timer de l'�coulement du temps

            // Si le timer est �coul�, r�initialiser l'arbre
            if (resetTimer <= 0f)
            {
                ResetArbreOriginel();
                isResetPending = false; // R�initialiser l'indicateur
            }
        }

        
    }

    void CloneAndMove()
    {
        // Cr�er un clone de l'arbre originel
        GameObject clone = Instantiate(arbreOriginel);

        // D�placer le clone de 500 m�tres (500 unit�s) sur l'axe Z
        clone.transform.position = arbreOriginel.transform.position + new Vector3(distance, 0, distance);
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

            arbreOriginel.SetActive(false);
        }
    }
}
