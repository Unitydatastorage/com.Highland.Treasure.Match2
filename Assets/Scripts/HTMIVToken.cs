using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace HTMIV
{
    public class HTMIVToken : MonoBehaviour
    {
        [SerializeField] private List<Sprite> HTMIVIcons;
        public Vector2Int HTMIVPOS { get; set; }
        private bool HTMIVIsDestroy;
        private int HTMIVidItem;

        public int HTMIVID
        {
            get => HTMIVidItem;
            set
            {
                HTMIVidItem = value;
                GetComponent<SpriteRenderer>().sprite = HTMIVIcons[HTMIVidItem];
            }
        }

        public void HTMIVDESTROY()
        {
            if (HTMIVIsDestroy)
                return;
            HTMIVIsDestroy = true;
            HTMIVImmDestroy();
        }

        private void HTMIVImmDestroy()
        {
            var HTMIVseq = DOTween.Sequence();
            HTMIVseq.Append(transform.DOScale(1.2f, 0.1f).SetEase(Ease.InOutBounce))
                .Append(transform.DOScale(0f, 0.15f).SetEase(Ease.InOutBounce))
                .OnComplete(() => { Destroy(gameObject); });
        }
    }
}