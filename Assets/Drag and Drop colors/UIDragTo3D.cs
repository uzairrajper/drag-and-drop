using UnityEngine;

/// <summary>
/// UIDragTo3D
/// 
/// This script allows dragging a 3D prefab into the scene by clicking 
/// on a UI Image. The object follows the mouse while dragging, and when released, 
/// it checks if it was dropped correctly using the attached Object script.
/// 
/// Attach this script to a **UI Image** in the Canvas.
/// </summary>
public class UIDragTo3D : MonoBehaviour
{
    [Header("Prefab Settings")]
    [Tooltip("The 3D object prefab to spawn when dragging from this UI element.")]
    public GameObject objectPrefab;   // Prefab assigned in Inspector

    private GameObject spawnedObject; // Stores reference to the instantiated prefab
    private Camera mainCam;           // Reference to the main scene camera
    private bool isDragging = false;  // Flag to track dragging state

    /// <summary>
    /// Unity Start method.
    /// Gets reference to the main camera.
    /// </summary>
    void Start()
    {
        mainCam = Camera.main;
    }

    /// <summary>
    /// Unity Update method.
    /// Handles the drag & drop process by checking mouse states each frame.
    /// </summary>
    void Update()
    {
        OnMousePressed();   // Handles start of drag
        MouseDrag();        // Updates object position while dragging
        OnMouseReleased();  // Handles releasing the object
    }

    /// <summary>
    /// Converts mouse screen position to world position in front of the camera.
    /// Adjust the z value to control the distance from the camera.
    /// </summary>
    private Vector3 GetWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;

        // Example: If camera z = -10 and desired depth = 3 → distance = -7
        mousePos.z = Mathf.Abs(mousePos.z - 3);

        // Convert from screen space to world space
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
        return worldPos;
    }

    /// <summary>
    /// Checks if the mouse is currently over this UI element.
    /// Used to ensure dragging only happens when clicking on the correct image.
    /// </summary>
    private bool IsMouseOverUI()
    {
        RectTransform rect = GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition);
    }

    /// <summary>
    /// Handles mouse press event.
    /// Spawns the prefab at the mouse position if clicked on the UI image.
    /// </summary>
    private void OnMousePressed()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            if (IsMouseOverUI()) // Only allow spawning if clicked over this UI element
            {
                spawnedObject = Instantiate(objectPrefab);       // Create prefab in scene
                spawnedObject.transform.position = GetWorldPosition(); // Place at mouse world pos
                isDragging = true;                               // Start drag
            }
        }
    }

    /// <summary>
    /// Handles dragging logic.
    /// Updates the spawned object's position to follow the mouse.
    /// </summary>
    private void MouseDrag()
    {
        if (isDragging && spawnedObject != null && Input.GetMouseButton(0))
        {
            spawnedObject.transform.position = GetWorldPosition();
        }
    }

    /// <summary>
    /// Handles mouse release event.
    /// Calls the Object script on the spawned prefab to validate the drop.
    /// Deactivates this UI element so it can only be used once.
    /// </summary>
    private void OnMouseReleased()
    {
         bool isMatched = false;
        if (isDragging && Input.GetMouseButtonUp(0)) // Left mouse released
        {
            if (spawnedObject != null)
            {
                // Check drop logic inside the spawned prefab
                Object dragScript = spawnedObject.GetComponent<Object>();
             isMatched =    dragScript.CheckDrop();
            }

            if (isMatched) {
                gameObject.SetActive(false); // Disable UI image after one use
                spawnedObject = null;        // Clear reference
            }
            isDragging = false;
        }
    }
}
