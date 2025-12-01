using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIItem : MonoBehaviour, IBeginDragHandler,
  IDragHandler
 
{
    [SerializeField] private GameObject objectPrefab;
    GameObject spawnedObject;
    
 

    public void OnBeginDrag(PointerEventData data)
    {
   
        if (IsMouseOverUI()) 
        {
            spawnedObject = Instantiate(objectPrefab);       
            UpdateObjectPosition(); 
                                  
        }

    }
    public void OnDrag(PointerEventData data)
    {
        UpdateObjectPosition();
    }
   
    private void UpdateObjectPosition()
    {
        if (spawnedObject == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Ground")) // only move on ground
            {
                Vector3 spawnPos = hit.point;
                float objectHeight = spawnedObject.GetComponent<Renderer>().bounds.size.y;
                spawnPos.y += objectHeight / 2f;

                spawnedObject.transform.position = spawnPos;
                break; // stop after first ground hit
            }
        }
    }

    private bool IsMouseOverUI()
    {
        RectTransform rect = GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition);
    }
}
