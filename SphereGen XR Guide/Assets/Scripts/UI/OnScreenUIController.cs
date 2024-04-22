using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SphereGen.GuideXR
{
    public class OnScreenUIController : MonoBehaviour, IInstructionCardUI
    {
        [SerializeField]
        Button _forwardButton, _backButton, _resetButton, _toggleBigUIButton;
        bool _bigUIOn, _setup;
        IndividualAssetHandler _iah;
        StepAssetPlacer _assetPlacer;
        // Start is called before the first frame update
        void Start()
        {
            SetupUI(ScriptableObject.CreateInstance<InstructionCardSO>());
            _assetPlacer = GetComponent<StepAssetPlacer>();
        }
        public void SetupUI(InstructionCardSO instructionCardData)
        {
            _iah = FindObjectOfType<IndividualAssetHandler>();
            try
            {
                if(_forwardButton && _backButton)
                {
                    _forwardButton.onClick.AddListener(_iah.LoadNextStoryBoard);
                    _backButton.onClick.AddListener(_iah.LoadPreviousStoryboard);
                }
                else
                {
                    Debug.LogWarningFormat("Buttons were not setup for {0}, either the buttons are disabled or don't exist.", gameObject.name);
                }

            }
            catch (System.Exception e)
            {
                Debug.LogErrorFormat("Did not setup buttons for {0} due to an error.", gameObject.name);
            }
        }

        public void OnDestroy()
        {
            try
            {
                _forwardButton.onClick.RemoveListener(_iah.LoadNextStoryBoard);
                _backButton.onClick.RemoveListener(_iah.LoadPreviousStoryboard);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Issue trying to remove listeners on " + gameObject.name);
            }
        }

        public void OnDisable()
        {
            Debug.LogFormat("Disabled: {0}", gameObject.name);
        }

        public void TryResetAssets()
        {
            if(_assetPlacer)
            {
                _assetPlacer.ResetAssets();
            }
            else
            {
                Debug.LogWarningFormat("No asset placer assigned to {0}, so cannot reset.", gameObject.name);
            }
        }
    }
}
