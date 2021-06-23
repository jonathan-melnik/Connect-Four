using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _topLeftPoint;
    [SerializeField] private Transform _bottomRightPoint;
    [SerializeField] private Disc _discPrefab;
    [SerializeField] private Transform _discsContainer;

    private List<List<Disc>> _columns = new List<List<Disc>>();
    private bool _redPlays = true;

    private const float COLUMN_WIDTH = 0.9f;
    private const float ROW_HEIGHT = 0.8f;
    private const int COLUMNS = 7;
    private const int ROWS = 6;
    
    private void Start()
    {
        for(int i=0; i<COLUMNS; i++){
            _columns.Add(new List<Disc>());
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            int col = GetColumnWithMousePos(Input.mousePosition);
            if(col != -1) {
                AddDiscAtColumn(col);
            }
        }
    }

    private void AddDiscAtColumn(int col) {
        if(_columns[col].Count == 6) {
            return;
        }
        var disc = Instantiate(_discPrefab);
        int row = _columns[col].Count;
        disc.transform.position = GetBoardPosition(row, col);
        disc.transform.SetParent(_discsContainer, false);
        disc.Initialize(isRed:_redPlays);        
        _columns[col].Add(disc);

        _redPlays = !_redPlays;
    }

    private int GetColumnWithMousePos(Vector3 mousePos) 
    {
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));  
        
        var topLeft = _topLeftPoint.position;
        var bottomRight = _bottomRightPoint.position;
        if(pos.x < topLeft.x || pos.y > topLeft.y || pos.x > bottomRight.x || pos.y < bottomRight.y){
            return -1;
        }
        return Mathf.Min(6, Mathf.FloorToInt(7 * (pos.x - topLeft.x) / (bottomRight.x - topLeft.x)));
    }

    private Vector3 GetBoardPosition(int row, int col) {
        float x = COLUMN_WIDTH * (col - COLUMNS/2);
        float y = _bottomRightPoint.position.y + ROW_HEIGHT * (row + 0.5f);
        return new Vector3(x, y, 0);
    }
}
