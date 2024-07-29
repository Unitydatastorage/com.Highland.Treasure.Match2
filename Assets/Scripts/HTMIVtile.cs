using System;
using UnityEngine;

namespace HTMIV
{
    public class HTMIVtile : MonoBehaviour
    {
        public Vector2Int HTMIVPOSITION { get; set; }

        public HTMIVToken ImpImpTokenItem { get; set; }
        
        public event Action<HTMIVtile> HTMIVONTileDown;
        public event Action<HTMIVtile> HTMIVONTileEnter;
        public event Action HTMIVONTileUP;

        #region HTMIV_Events

        private void OnMouseUp()
        {
            HTMIVONTileUP?.Invoke();
        }
        
        private void OnMouseDown()
        {
            if (HTMIVHelpers.HTMIVIgnoreInput)
                return;
            
            HTMIVONTileDown?.Invoke(this);
        }

        private void OnMouseEnter()
        {
            HTMIVONTileEnter?.Invoke(this);
        }

        #endregion
    }
}