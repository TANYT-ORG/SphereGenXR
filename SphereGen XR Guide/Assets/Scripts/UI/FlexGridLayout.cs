using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SGUI
{
    public class FlexGridLayout : LayoutGroup
    {

        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }
        public FitType fitType;
        public int rows, columns;
        public Vector2 spacing;
        public bool fitX, fitY;
        public Vector2 cellSize;

        //This is the default function that gets called by the base class for changes
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            //determine our elements
            float sqrRt = Mathf.Sqrt(transform.childCount);

            if (fitType == FitType.Width || fitType == FitType.Uniform)
            {
                columns = Mathf.CeilToInt(sqrRt);
                fitX = true;
            }
            if(fitType == FitType.Height || fitType == FitType.Uniform)
            {
                rows = Mathf.CeilToInt(sqrRt);                
                fitY = true;
            }

            //Adjust based on fit type
            if(fitType == FitType.Width || fitType == FitType.FixedColumns)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }
            if(fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            //Size of space we can use to layout elements
            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;
            //Position based on parent, adjusted based on spacing and padding
            float cellWidth = (parentWidth - padding.left - padding.right - spacing.x * (columns - 1)) / (float)columns;
            float cellHeight = (parentHeight - padding.top - padding.bottom - spacing.y * (rows - 1)) / (float)rows;

            //Setting the size based on space and # of elements
            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            if(cellSize.x == 0 || cellSize.y == 0)
            {
                Debug.LogFormat("Something wrong with your cell sizing logic.");
            }
            int columnCount = 0;
            int rowCount = 0;

            for(int i = 0; i< rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                var item = rectChildren[i];
                Vector2 itemPos = new Vector2((cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left, (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top);

                SetChildAlongAxis(item, 0, itemPos.x, cellSize.x);
                SetChildAlongAxis(item, 1, itemPos.y, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            
        }

        public override void SetLayoutHorizontal()
        {
            
        }

        public override void SetLayoutVertical()
        {
            
        }

        private new void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            CalculateLayoutInputHorizontal();
        }

        public void Awake()
        {
            //CalculateLayoutInputHorizontal();
        }
    }
}
