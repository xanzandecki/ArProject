using UnityEngine;

public class PlaceFurniture : MonoBehaviour
{
    public GameObject furniturePrefab;
    private GameObject placedFurniture;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            
            if (Physics.Raycast(ray, out hit))
            {
                if (placedFurniture == null)
                {
                    placedFurniture = Instantiate(furniturePrefab, hit.point, Quaternion.identity);
                }
                else
                {
                    placedFurniture.transform.position = hit.point;
                }
            }
        }
    }
}
