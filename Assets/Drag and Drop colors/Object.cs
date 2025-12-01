using UnityEngine;

/// <summary>
/// Object
/// 
/// This script is attached to **colored prefabs only**.
/// It handles interactions when the object is dragged and dropped onto a 
/// matching target (the default prefab in the scene).
/// 
/// Features:
/// - Detects when it enters/leaves a valid drop target.
/// - Gives visual feedback by scaling the target up/down.
/// - On drop, copies materials from the dragged object to the target’s children.
/// - Destroys the dragged object after drop.
/// </summary>
public class Object : MonoBehaviour
{
    private GameObject correctTarget; // The object this prefab can be dropped onto

    /// <summary>
    /// Trigger enter detection.
    /// If the dragged prefab collides with a target of the same tag,
    /// save it as the correct target and scale it up slightly.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(gameObject.tag)) // Only accept if tags match
        {
            correctTarget = other.gameObject;
            correctTarget.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f); // Visual feedback
        }
    }

    /// <summary>
    /// Trigger exit detection.
    /// Resets the target back to normal scale and clears reference.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (correctTarget == other.gameObject)
        {
            correctTarget.transform.localScale = new Vector3(1, 1, 1); // Reset scale
            correctTarget = null; // Clear reference
        }
    }

    /// <summary>
    /// Checks if the drop is valid when the player releases the object.
    /// If valid:
    /// - Reset target scale.
    /// - Copy materials for Base and Icing child objects.
    /// - Destroy the dragged prefab.
    /// If invalid:
    /// - Just destroy the dragged prefab.
    /// </summary>
    public bool CheckDrop()
    {
        if (correctTarget != null)
        {
            // Reset scale
            correctTarget.transform.localScale = new Vector3(1, 1, 1);

            // Copy materials for both required child parts
            CopyChildMaterials(correctTarget, transform, "Base");
            CopyChildMaterials(correctTarget, transform, "icing");

            // Destroy dragged object
            Destroy(gameObject);
            return true;
        }
        else
        {
            // Wrong drop → just destroy
            Destroy(gameObject);
           return false;
        }
    }

    /// <summary>
    /// Copies materials from a specific child (by name) of the dragged object
    /// to the corresponding child of the target object.
    /// </summary>
    /// <param name="targetParent">Target object (default prefab in scene)</param>
    /// <param name="sourceParent">Dragged object (colored prefab)</param>
    /// <param name="childName">Child name to match (e.g., "Base", "icing")</param>
    private void CopyChildMaterials(GameObject targetParent, Transform sourceParent, string childName)
    {
        Transform targetChild = targetParent.transform.Find(childName);
        Transform sourceChild = sourceParent.Find(childName);

        if (targetChild != null && sourceChild != null)
        {
            MeshRenderer targetRenderer = targetChild.GetComponent<MeshRenderer>();
            MeshRenderer sourceRenderer = sourceChild.GetComponent<MeshRenderer>();

            if (targetRenderer != null && sourceRenderer != null)
            {
                // Use sharedMaterials so it applies the full material array
                targetRenderer.materials = sourceRenderer.sharedMaterials;
            }
        }
    }
}
