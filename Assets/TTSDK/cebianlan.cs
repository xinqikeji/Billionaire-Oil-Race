using System;
using System.Collections;
using System.Collections.Generic;
using BlueStellar.Cor;
using TTSDK;
using TTSDK.UNBridgeLib.LitJson;
using UnityEngine;
using UnityEngine.UI;

public class cebianlan : MonoBehaviour
{
    public Button guanbibtn;
    public Button opencebianlan;
    public Button linqujiangli;
    public Button showbtn;
    public Text lingqujieshu;
    private const string PREFS_KEY_LAST_CLAIMED = "LastClaimedTime";
    public void Awake()
    {
        if (CanClaimReward())
        {
            PlayerPrefs.SetString("IsCebianlan", "True");
        }
        showbtn.onClick.AddListener(() => { this.gameObject.SetActive(true); });

        lingqujieshu.gameObject.SetActive(false);
        guanbibtn.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
        opencebianlan.onClick.AddListener(() =>
        {
            TTosidebar();
        });
        linqujiangli.onClick.AddListener(() =>
        {
            //侧边栏奖励
            MoneyWallet.Instance.MoneyPlus(100);
            ArenaSpawner.Instance.SetNext();

            opencebianlan.gameObject.SetActive(false);
            linqujiangli.gameObject.SetActive(false);
            lingqujieshu.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            this.showbtn.gameObject.SetActive(false);
            // 更新最后领取时间
            PlayerPrefs.SetString(PREFS_KEY_LAST_CLAIMED, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            PlayerPrefs.SetString("IsCebianlan", "false");
            PlayerPrefs.Save();

        });
        TTSidebarinit();
    }
    private bool CanClaimReward()
    {
        // 获取当前时间
        DateTime now = DateTime.Now;

        // 获取上次领取的时间戳
        if (!PlayerPrefs.HasKey(PREFS_KEY_LAST_CLAIMED))
        {
            // 如果没有时间戳，表示从未领取过奖励
            return true;
        }

        // 解析时间戳
        DateTime lastClaimed = DateTime.Parse(PlayerPrefs.GetString(PREFS_KEY_LAST_CLAIMED));
        Debug.LogError(now.Day);
        Debug.LogError(lastClaimed.Day);
        // 计算时间差
        //TimeSpan timeSpan = now - lastClaimed;
        if (now.Day != lastClaimed.Day)
        {
            return true;
        }
        return false;
        // 如果超过24小时，允许再次领取奖励
        // return timeSpan.TotalHours >= 24;
    }
    public void TTSidebarinit()
    {
        TT.InitSDK((code, env) =>
        { });

        if (PlayerPrefs.GetString("IsCebianlan") == "True")
        {
            showbtn.gameObject.SetActive(true);
        }
        else
        {
            showbtn.gameObject.SetActive(false);
        }
        if ((PlayerPrefs.GetString("Launch From") == "homepage") && (PlayerPrefs.GetString("location") == "homepage_expand"))
        {
            opencebianlan.gameObject.SetActive(false);

            if (CanClaimReward())
            {
                linqujiangli.gameObject.SetActive(true);
                lingqujieshu.gameObject.SetActive(false);
            }
            else
            {
                lingqujieshu.gameObject.SetActive(true);
                linqujiangli.gameObject.SetActive(false);
            }
        }
        else
        {
            opencebianlan.gameObject.SetActive(true);
            linqujiangli.gameObject.SetActive(false);
        }

    }

    public void TTosidebar()
    {
        var data = new JsonData
        {
            ["scene"] = "sidebar",
            //["activityId"] = "cacheActivityId",
        };
        TT.NavigateToScene(data, () =>
        {
            Debug.Log("navigate to scene success");
        }, () =>
        {
            opencebianlan.gameObject.SetActive(false);


            linqujiangli.gameObject.SetActive(true);

            Debug.Log("navigate to scene complete");
        }, (errCode, errMsg) =>
        {
            Debug.Log($"navigate to scene error, errCode:{errCode}, errMsg:{errMsg}");
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
