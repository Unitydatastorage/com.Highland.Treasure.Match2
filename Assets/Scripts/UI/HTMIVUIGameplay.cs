using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HTMIV.UI
{
    public class HTMIVUIGameplay : MonoBehaviour
    {
        public void HTMIVWinGame(int HTMIVScore, int HTMIVGoal, float HTMIVTime)
        {
            HTMIVWinPanel.HTMIVReforce(true);
            HTMIVWinText.text = $"Result\nScore: {HTMIVScore}/{HTMIVGoal}\nTime: {HTMIVTime.HTMIVTimeToString()}s";
            
            var HTMIVCurLevel = PlayerPrefs.GetInt("HTMIVCurLvl", 0);
            var HTMIVPassed = PlayerPrefs.GetInt("HTMIVPassed", 0);

            if (HTMIVCurLevel == HTMIVPassed)
            {
                HTMIVPassed++;
                PlayerPrefs.SetInt("HTMIVPassed", HTMIVPassed);
            }
        }
        
        public void HTMIVLoseGame(int HTMIVScore, int HTMIVGoal, float HTMIVTime)
        {
            HTMIVLosePanel.HTMIVReforce(true);
            HTMIVLoseText.text = $"Result\nScore: {HTMIVScore}/{HTMIVGoal}\nTime: {HTMIVTime.HTMIVTimeToString()}s";
        }
        
        [SerializeField] private TMP_Text HTMIVWinText;
        [SerializeField] private TMP_Text HTMIVLoseText;
        [SerializeField] private Button HTMIVNextLvlBtn;
        [SerializeField] private Button HTMIVRetryLvlBtn;
        [SerializeField] private CanvasGroup HTMIVHudPanel;
        [SerializeField] private CanvasGroup HTMIVWinPanel;
        [SerializeField] private CanvasGroup HTMIVLosePanel;
        [SerializeField] private List<Button> HTMIVHomeBTN;
        
        private void Start()
        {
            foreach (var HTMIV in HTMIVHomeBTN)
                HTMIV.onClick.AddListener(() => SceneManager.LoadScene(1));

            HTMIVNextLvlBtn.onClick.AddListener(() =>
            {
                var HTMIVcur = PlayerPrefs.GetInt("HTMIVCurLvl", 0);

                HTMIVcur++;
                PlayerPrefs.SetInt("HTMIVCurLvl", HTMIVcur);

                SceneManager.LoadScene(2);
            });
            
            HTMIVRetryLvlBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(2);
            });
            
            HTMIVShowHud();
        }

        private void HTMIVShowHud()
        {
            HTMIVHudPanel.HTMIVReforce(true);
            HTMIVWinPanel.HTMIVReforce(false);
            HTMIVLosePanel.HTMIVReforce(false);
        }
    }
}