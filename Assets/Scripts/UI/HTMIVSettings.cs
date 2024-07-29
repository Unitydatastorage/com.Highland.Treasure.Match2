using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace HTMIV.UI
{
    public class HTMIVSettings : MonoBehaviour
    {
        [SerializeField] private AudioSource HTMIVSFXSource;
        [SerializeField] private AudioClip HTMIVCLICKClip;
        [SerializeField] private CanvasGroup HTMIVContent;
        [SerializeField] private Button HTMIVOpenSettings;
        [SerializeField] private Button HTMIVCloseSettings;
        [SerializeField] private AudioSource HTMIVBackgroundMusic;
        [SerializeField] private Slider HTMIVMusicSlider;
        [SerializeField] private Slider HTMIVSFXSlider;
        public void HTMIVCLICK() => HTMIVSFXSource.HTMIVPitching(HTMIVCLICKClip);

        private void Start()
        {
            Application.targetFrameRate = 120;

            HTMIVHide();
            HTMIVOpenSettings.onClick.AddListener(HTMIVShow);
            HTMIVCloseSettings.onClick.AddListener(HTMIVHide);

            var htmivSfx = PlayerPrefs.GetFloat("HTMIVSFX", 1f);
            HTMIVSFXSource.volume = htmivSfx;
            HTMIVSFXSlider.value = htmivSfx;

            var htmivBg = PlayerPrefs.GetFloat("HTMIVBG", 1f);
            HTMIVBackgroundMusic.volume = htmivBg;
            HTMIVMusicSlider.value = htmivBg;

            HTMIVMusicSlider.onValueChanged.AddListener(HTMIVChangeBgMsc);
            HTMIVSFXSlider.onValueChanged.AddListener(HTMIVChangeSfx);

            return;

            void HTMIVChangeBgMsc(float htmivDef)
            {
                PlayerPrefs.SetFloat("HTMIVBG", htmivDef);
                HTMIVBackgroundMusic.volume = htmivDef;
            }

            void HTMIVChangeSfx(float htmivDef)
            {
                PlayerPrefs.SetFloat("HTMIVSFX", htmivDef);
                HTMIVSFXSource.volume = htmivDef;
            }

            void HTMIVHide()
            {
                DOTween.timeScale = 1f;
                HTMIVHelpers.HTMIVStop = false;
                HTMIVContent.HTMIVReforce(false);
            }

            void HTMIVShow()
            {
                HTMIVSFXSource.Stop();
                DOTween.timeScale = 0f;
                HTMIVHelpers.HTMIVStop = true;
                HTMIVContent.HTMIVReforce(true);
            }
        }
    }
}