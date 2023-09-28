using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    public class Array2DInt : Array2D<int>
    {
        [SerializeField]
        CellRowInt[] cells = new CellRowInt[Consts.defaultGridSize];

        protected override CellRow<int> GetCellRow(int idx)
        {
            return cells[idx];
        }

        public static Array2DInt CreateArray2DInt(Vector2Int size)
        {
            Array2DInt array = new Array2DInt();
            array.gridSize = size;
            array.cells = new CellRowInt[size.y];

            for (int i = 0; i < array.cells.Length; ++i)
            {
                array.cells[i] = new CellRowInt(size.x);
            }

            Debug.Log($"Array size: {array.GridSize}");

            return array;
        }
    }
}
