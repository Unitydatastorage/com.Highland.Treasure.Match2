using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HTMIV.UI
{
    public class HTMIVStartHUD : MonoBehaviour
    {
        [SerializeField] private Button HTMIVOpenLevel;
        [SerializeField] private CanvasGroup HTMIVPolicyPanel;
        [SerializeField] private Button HTMIVCloseApplication;
        [SerializeField] private Button HTMIVCloseWelcome;
        [SerializeField] private CanvasGroup HTMIVMenuPanel;
        [SerializeField] private CanvasGroup HTMIVPolicyPanel1;
        [SerializeField] private Button HTMIVPolicyPanel1Btn;
        [SerializeField] private Button HTMIVPolicyPanel1BtnBack;
        [SerializeField] private CanvasGroup HTMIVLevelPanel;
        [SerializeField] private CanvasGroup HTMIVWelcomePanel;
        [SerializeField] private Button HTMIVPolicyBTN;
        [SerializeField] private List<Button> HTMIVCloseAll;

        private void Awake()
        {
            foreach (var HTMIVClocseBtn in this.HTMIVCloseAll)
                HTMIVClocseBtn.onClick.AddListener(HTMIVCloseAll);

            HTMIVPolicyBTN.onClick.AddListener(() => HTMIVPolicyPanel.HTMIVReforce(true));
            HTMIVOpenLevel.onClick.AddListener(() => HTMIVLevelPanel.HTMIVReforce(true));

            HTMIVCloseApplication.onClick.AddListener(Application.Quit);

            HTMIVCloseAll();

            if (!PlayerPrefs.HasKey("HTMIV"))
            {
                HTMIVWelcomePanel.HTMIVReforce(true);
                HTMIVMenuPanel.HTMIVReforce(false);
            }

            HTMIVPolicyPanel1Btn.onClick.AddListener(() => HTMIVPolicyPanel1.HTMIVReforce(true));
            HTMIVPolicyPanel1BtnBack.onClick.AddListener(() =>
            {
                HTMIVPolicyPanel1.HTMIVReforce(false);
                HTMIVWelcomePanel.HTMIVReforce(true);
            });
            HTMIVCloseWelcome.onClick.AddListener(() =>
            {
                HTMIVCloseAll();
                PlayerPrefs.SetString("HTMIV", "HTMIVAccess");
            });

            return;

            void HTMIVCloseAll()
            {
                HTMIVMenuPanel.HTMIVReforce(true);
                HTMIVWelcomePanel.HTMIVReforce(false);
                HTMIVPolicyPanel.HTMIVReforce(false);
                HTMIVLevelPanel.HTMIVReforce(false);
                HTMIVPolicyPanel1.HTMIVReforce(false);
            }
        }
    }
}