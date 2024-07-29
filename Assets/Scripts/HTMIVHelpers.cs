using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HTMIV
{
    public static class HTMIVHelpers
    {
        public static void HTMIVShake(this HTMIVToken HTMIVToken)
            => HTMIVToken.transform.DOShakeScale(0.1f, Vector3.up).SetEase(Ease.Linear);

        public static (float, float, float, float) HTMIVGetRand() => (
            Random.Range(0.2f, 0.7f), Random.Range(0.2f, 0.7f),
            Random.Range(0.2f, 0.4f), Random.Range(0.2f, 0.7f)
        );
        
        public static bool HTMIVStop { get; set; }
        public static bool HTMIVIgnoreInput { get; set; }

        public static Tween HTMIVMovement(this HTMIVToken HTMIVtoken)
            => HTMIVtoken.transform.DOLocalMove(new Vector3(HTMIVtoken.HTMIVPOS.x, HTMIVtoken.HTMIVPOS.y, 0), 0.1f)
                .SetEase(Ease.Linear);

        public static void HTMIVReforce(this CanvasGroup HTMIV, bool HTMIVVal)
        {
            HTMIV.interactable = HTMIVVal;
            HTMIV.blocksRaycasts = HTMIVVal;
            HTMIV.alpha = HTMIVVal ? 1 : 0;
        }

        public static string HTMIVTimeToString(this float HTMIVtime)
        {
            var HTMIVval = Mathf.CeilToInt(HTMIVtime);
            return HTMIVval.ToString();
        }
        
        public static void HTMIVPitching(this AudioSource HTMIVSoundsource, AudioClip HTMIVclip, bool HTMIVval = false)
        {
            if (!HTMIVval)
            {
                HTMIVSoundsource.pitch = 1f;
                HTMIVSoundsource.PlayOneShot(HTMIVclip);
                return;
            }

            HTMIVSoundsource.pitch = Random.Range(0.9f, 1.1f);
            HTMIVSoundsource.PlayOneShot(HTMIVclip);
        }
    }

    public class HTMIVInfo
    {
        public HTMIVtile HTMIVEmpty;
        public HTMIVtile HTMIVMovableGeTile;
    }
}