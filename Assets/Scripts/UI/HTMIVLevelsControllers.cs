using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HTMIV.UI
{
    public class HTMIVLevelsControllers : MonoBehaviour
    {
        [SerializeField] private List<Button> HTMIVLevelsBtns;

        private void Start()
        {
            var HTMIVPassed = PlayerPrefs.GetInt("HTMIVPassed", 0);

            for (var i = 0; i < HTMIVLevelsBtns.Count; i++)
            {
                var HTMIVindex = i;
                var htmivCurBtn = HTMIVLevelsBtns[i];
                htmivCurBtn.GetComponentInChildren<TMP_Text>().text = $"Lvl {HTMIVindex + 1}";
                var HTMIVUnlocked = i <= HTMIVPassed;
                htmivCurBtn.interactable = HTMIVUnlocked;

                htmivCurBtn.onClick.AddListener(() =>
                {
                    if (HTMIVindex % 5 == 0)
                        PlayerPrefs.SetString("HTMIVCurLvlIsSup", "HTMIVTrue");
                    else
                        PlayerPrefs.SetString("HTMIVCurLvlIsSup", "HTMIVFalse");
                    
                    PlayerPrefs.SetInt("HTMIVCurLvl", HTMIVindex);
                    SceneManager.LoadScene(2);
                });
            }
        }
    }
}