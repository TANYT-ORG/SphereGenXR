using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SphereGen.GuideXR

{
    public class SceneAssetBundleHandler : MonoBehaviour
    {
        /// <summary>
        /// The URL used to fetch the AssetBundle on <see cref="Start"/>.
        /// </summary>
        [Tooltip("The URL used to fetch the AssetBundle on Start.")]
        public string AssetBundleUrl;

        [SerializeField]
        private Image LoaderIcon;
        

        [SerializeField]
        public MessageBoxHandler messageBoxHandler;

        // Number of attempts before we show the user a retry button.
        private const int InitialAttemptCount = 3;
        private AssetBundle _bundle;
        private int _assetBundleRetrievalAttemptCount;
        
        private bool _downloading;


        // Start is called before the first frame update
        void Start()
        {
            AttemptAssetBundleDownload(InitialAttemptCount);
        }

        /// <summary>
        /// Attempts to retry the AssetBundle download one time.
        /// </summary>
        public void ButtonEventRetryDownload()
        {
            AttemptAssetBundleDownload(1);
        }
        /// <summary>
        /// Attempts to download the AssetBundle available at AssetBundleUrl.
        /// If it fails numberOfAttempts times, then it will display a retry button.
        /// </summary>
        private void AttemptAssetBundleDownload(int numberOfAttempts)
        {
            if (_downloading)
            {
                Debug.Log("Download attempt ignored because a download is already in progress.");
                return;
            }

            HideRetryButton();
           
            StartCoroutine(AttemptAssetBundleDownloadsCo(numberOfAttempts));
        }
        private IEnumerator AttemptAssetBundleDownloadsCo(int numberOfAttempts)
        {
            _downloading = true;

            for (var i = 0; i < numberOfAttempts; i++)
            {
                _assetBundleRetrievalAttemptCount++;
                Debug.LogFormat("Attempt #{0} at downloading AssetBundle...", _assetBundleRetrievalAttemptCount);

                yield return GetAssetBundle(AssetBundleUrl);

                if (_bundle != null)
                {
                    break;
                }

                yield return new WaitForSeconds(0.5f);
            }

            if (_bundle == null)
            {
                ShowRetryButton();
                messageBoxHandler.SetMessage("Error: 101: Failed to download AssetBundle, Please Check the internet Connection","", "Retry", ButtonEventRetryDownload);
                _downloading = false;
                yield break;
            }

            Debug.Log("Downloaded Successfully");
           SceneManager.LoadSceneAsync(_bundle.GetAllScenePaths()[0]);

             yield return null;
            _downloading = false;
        }
        private IEnumerator GetAssetBundle(string assetBundleUrl)
        {
            UnityWebRequest webRequest;
             var downloadOperation = StartAssetBundleDownload(assetBundleUrl, out webRequest);
           
            
            var isDone = false;
            while (!isDone)
            {
                if (downloadOperation.isDone)
                {
                    isDone = true;
                }
                else
                {
                  
                   if(LoaderIcon.fillAmount>=1)
                    {
                        LoaderIcon.fillAmount = 0f;

                    }
                    LoaderIcon.fillAmount += 0.01f;
                }

                yield return null;
            }
           if (IsNetworkError(webRequest))
            {
               // _maxLoadingBarProgress = LoadingBar.Progress;
                Debug.LogFormat("Failed to download AssetBundle: {0}", webRequest.error);
                messageBoxHandler.SetMessage("Error: 102: Failed to download AssetBundle, Please Check the internet Connection",webRequest.error,"Retry",ButtonEventRetryDownload);
            }
            else
            {
                _bundle = DownloadHandlerAssetBundle.GetContent(webRequest);
            }
        }

        private void ShowRetryButton()
        {
            LoaderIcon.gameObject.SetActive(false);
           
        }
        private static bool IsNetworkError(UnityWebRequest request)
        {
#if UNITY_2020_2_OR_NEWER
            return request.result == UnityWebRequest.Result.ConnectionError ||
                   request.result == UnityWebRequest.Result.ProtocolError;
#else
            return request.isHttpError || request.isNetworkError;
#endif
        }

        private static AsyncOperation StartAssetBundleDownload(string assetBundleUrl, out UnityWebRequest webRequest)
        {
#if UNITY_2018_1_OR_NEWER
            webRequest = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl);
#else
            webRequest = UnityWebRequest.GetAssetBundle(assetBundleUrl);
#endif
            return webRequest.SendWebRequest();
        }

        private void HideRetryButton()
        {
            LoaderIcon.gameObject.SetActive(true);
          
        }
    }
}
