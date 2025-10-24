using UnityEngine;
using UnityEngine.SceneManagement;


namespace BlueStellar.Cor
{
    
    public class ButtonManager : MonoBehaviour
    {
        public ByteGameAdManager byteGameAdManager;
        public void StartLevel()
        {
            LevelController.Instance.LevelStart();
        }

        public void Continue()
        {
            UIManager.Instance.MoneyScreen(true);
        }

        public void RestartLevel()
        {
          

            if (Advertisements.Instance.IsInterstitialAvailable())
            {
                Advertisements.Instance.ShowInterstitial(InterstitialRestart);
            }
            else
            {
                MoneyWallet.Instance.MoneyPlus(50);
                SceneLoader(1);
            }

        }

        private void InterstitialRestart(string advertiser)
        {
            MoneyWallet.Instance.MoneyPlus(50);
            SceneLoader(1);
        }

        public void NextLevel()
        {
            if (Advertisements.Instance.IsInterstitialAvailable())
            {
                Advertisements.Instance.ShowInterstitial(InterstitialNextLevel);
            }
            else
            {
                MoneyWallet.Instance.MoneyPlus(100);
                ArenaSpawner.Instance.SetNext();
                LevelController.Instance.NextLevel();
                SceneLoader(1);
            }
        }

        private void InterstitialNextLevel(string advertiser)
        {
            MoneyWallet.Instance.MoneyPlus(100);
            ArenaSpawner.Instance.SetNext();
            LevelController.Instance.NextLevel();
            SceneLoader(1);
        }

        public void x2Reward()
        {// 激励广告
            byteGameAdManager.PlayRewardedAd("3tva6j9rsk163mmadg",
                        (isValid, duration) =>
                        {
                            //isValid广告是否播放完，正常游戏逻辑在以下部分
                            Debug.LogError(0);
                            if (isValid)
                            {
                                
                                Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
                                CompleteMethod(isValid);
                            }


                        },
                           (errCode, errMsg) =>
                           {
                               Debug.LogError(1);
                           });
           
        }

        private void CompleteMethod(bool completed)
        {
            
            if (completed == true)
            {
                MoneyWallet.Instance.MoneyPlus(200);
                ArenaSpawner.Instance.SetNext();
                LevelController.Instance.NextLevel();
                SceneLoader(1);
            }
            else
            {
#if UNITY_EDITOR
                MoneyWallet.Instance.MoneyPlus(200);
                ArenaSpawner.Instance.SetNext();
                LevelController.Instance.NextLevel();
                SceneLoader(1);
#endif
            }
        }
        private void SceneLoader(int indexScene)
        {
            SceneManager.LoadScene(indexScene);
        }
    }
}
