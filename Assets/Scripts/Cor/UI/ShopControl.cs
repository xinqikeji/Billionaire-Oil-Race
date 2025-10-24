using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BlueStellar.Cor;

public class ShopControl : MonoBehaviour
{
    public List<shopItems> hats;
    public SetHat _sethat;
    public static ShopControl Instance;

    public ByteGameAdManager byteGameAdManager2;
    private void Awake()
    {
        Instance = this;
    }
    public void BackMenu()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SetHats()
    {
        _sethat.SetCurrentHat();
    }

    public void OnBuyButton()
    {
        if (MoneyWallet.Instance.ammountMoney >= 300)
        {
            if (hats.Count != 0)
            {
                int rn = UnityEngine.Random.Range(0, hats.Count);
                hats[rn].Buy();
                MoneyWallet.Instance.MoneyMinus(300);
            }
        }
    }

    public void OnAdsButton()
    {
        //Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
        // 激励广告
        byteGameAdManager2.PlayRewardedAd("p1e46h9q6mi8i6hqa8",
                    (isValid, duration) =>
                    {
                        //isValid广告是否播放完，正常游戏逻辑在以下部分
                        Debug.LogError(0);
                        if (isValid)
                        {                 
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
            MoneyWallet.Instance.MoneyPlus(150);
        }
        else
        {
#if UNITY_EDITOR
            MoneyWallet.Instance.MoneyPlus(150);
#endif
        }
    }
}
