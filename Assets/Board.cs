using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform topLeftPoint;
    public Transform bottomRightPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            int column = GetColumnWithMousePos(Input.mousePosition);
            Debug.Log(column);
        }
    }

    int GetColumnWithMousePos(Vector3 mousePos) 
    {
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));  
        
        var topLeft = topLeftPoint.position;
        var bottomRight = bottomRightPoint.position;
        if(pos.x < topLeft.x || pos.y > topLeft.y || pos.x > bottomRight.x || pos.y < bottomRight.y){
            return -1;
        }
        return Mathf.Min(6, Mathf.FloorToInt(7 * (pos.x - topLeft.x) / (bottomRight.x - topLeft.x)));
    }
}
