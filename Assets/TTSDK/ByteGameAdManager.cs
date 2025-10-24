using UnityEngine;
using StarkSDKSpace;
using TTSDK;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using static TTSDK.TTAppLifeCycle;

using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking;




public class ByteGameAdManager : MonoBehaviour
{
    

    // 广告ID存储在List中，方便管理顺序
    public List<string> RewardedAdIds = new List<string>();
    //{
    //    "mi5ag76n0bg4506577", // LEVEL_VIDEO_ID
    //    "your_bonus_video_id_here", // BONUS_VIDEO_ID
    //    "your_special_video_id_here" // SPECIAL_VIDEO_ID
    //};

    // 插屏广告ID
    public const string INTERSTITIAL_ID = "2efdh3aelb10i0q2ie";
    // 横幅广告ID
    public const string BANNER_ID = "4f08ml2f1a81vsqvbg";
    public TTRewardedVideoAd tTRewardedVideoAd;
    // 广告实例存储
    private TTInterstitialAd _interstitialAd;
    private TTBannerAd _bannerAd;

    //// 激励广告实例存储（使用字典存储广告实例）
    //public Dictionary<string, TTRewardedVideoAd> _rewardedAds = new Dictionary<string, TTRewardedVideoAd>();
    public int count = 0;
    // 当前广告状态
    private bool _isRewardAdLoading;
    private bool _isInterstitialAdLoading = false;

    //public static ByteGameAdManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            //_instance = new GameObject("ByteGameAdManager").AddComponent<ByteGameAdManager>();
    //            //DontDestroyOnLoad(_instance.gameObject);
    //        }
    //        return _instance;
    //    }
    //}
    public void Update()
    {
    }


    private void Awake()
    {
        //if (_instance == null)
        //{
        //    _instance = this;
        //    //DontDestroyOnLoad(gameObject);
        //}

        TT.InitSDK((code, env) =>
        {
            Debug.Log($"SDK初始化完成，环境：{env.m_HostEnum}");

            // 预加载所有激励广告
            //foreach (var adId in RewardedAdIds)
            //{
            //    PreloadRewardAd(adId);
            //}

            // 预加载插屏广告
           // PreloadInterstitialAd();
        });
        TT.InitSDK((code, env) =>
        { });
        TT.CheckScene(TTSideBar.SceneEnum.SideBar, b =>
        {
            Debug.Log("check scene success，" + b);
            
        }, () =>
        {
            Debug.Log("check scene complete");
        }, (errCode, errMsg) =>
        {
            Debug.Log($"check scene error, errCode:{errCode}, errMsg:{errMsg}");
        });
        TestAppLifeCycle();
#if UNITY_EDITOR
        MockSetting.OpenAllMockModule();
#endif
    }
   
    private void OnShowCallback(Dictionary<string, object> data)
    {
        Debug.Log("OnShow callback triggered with data:");

        // 遍历字典并打印所有键值对
        foreach (var kvp in data)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
        }

        // 根据指定 key 获取指定数据
        if (data.ContainsKey("scene"))
        {
            Debug.Log("Scene: " + data["scene"]);
        }

        if (data.ContainsKey("launch_from"))
        {
            Debug.Log("Launch From: " + data["launch_from"]);
        }
        PlayerPrefs.SetString("Launch From", data["launch_from"].ToString());
        if (data.ContainsKey("location"))
        {
            Debug.Log("Location: " + data["location"]);
        }
        PlayerPrefs.SetString("location", data["location"].ToString());
        // 判断小游戏用户是否通过侧边栏入口场景启动
        if (data.ContainsKey("scene") && data["scene"] == "sidebar" &&
            data.ContainsKey("launch_from") && data["launch_from"] == "homepage" &&
            data.ContainsKey("location") && data["location"] == "homepage_expand")
        {
            Debug.Log("小游戏用户通过侧边栏入口场景启动");
            // 在这里添加你的逻辑
        }
    }
    public TTAppLifeCycle lifeCycle;
    public void TestAppLifeCycle()
    {
        Dictionary<string, object> param = new Dictionary<string, object>
        {
            { "scene", "sidebar" },
            { "launch_from", "homepage" },
            { "location", "sidebar_card" }
        };
        lifeCycle = TT.GetAppLifeCycle();
        lifeCycle.OnShow += OnShowCallback;
        
        Debug.Log(1111111);
        
        MethodInfo methodInfo = typeof(TTAppLifeCycle).GetMethod("OnShowCall", BindingFlags.NonPublic | BindingFlags.Instance);
        methodInfo?.Invoke(lifeCycle, new object[] { param });
        //TTSDK.LaunchOption launchOption = TT.GetLaunchOptionsSync();
        //Debug.Log("path :" + launchOption.Path);
        //Debug.Log("scene :" + launchOption.Scene);
        //Debug.Log("subScene :" + launchOption.SubScene);
        //Debug.Log("group_id :" + launchOption.GroupId);
        //Debug.Log("shareTicket :" + launchOption.ShareTicket);
        //Debug.Log("is_sticky :" + launchOption.IsSticky);
        //Debug.Log("query :");
        //if (launchOption.Query != null)
        //{
        //    foreach (KeyValuePair<string, string> kv in launchOption.Query)
        //        if (kv.Value != null)
        //            Debug.Log(kv.Key + ": " + kv.Value);
        //        else
        //            Debug.Log(kv.Key + ": " + "null ");
        //}

        //Debug.Log("refererInfo :");
        //if (launchOption.RefererInfo != null)
        //{
        //    foreach (KeyValuePair<string, string> kv in launchOption.RefererInfo)
        //        if (kv.Value != null)
        //            Debug.Log(kv.Key + ": " + kv.Value);
        //        else
        //            Debug.Log(kv.Key + ": " + "null ");
        //}

        //Debug.Log("extra :");
        //if (launchOption.Extra != null)
        //{
        //    foreach (KeyValuePair<string, string> kv in launchOption.Extra)
        //        if (kv.Value != null)
        //            Debug.Log(kv.Key + ": " + kv.Value);
        //        else
        //            Debug.Log(kv.Key + ": " + "null ");
        //}

    }

    private void OnAppShow(Dictionary<string, object> param)
    {
        Debug.LogError(11111111111); 
        Debug.Log(param.Keys);
    }
    public void OnDisable()
    {
    }
    #region 激励广告
    public void PlayRewardedAd(string adId,
    Action<bool, int> onClose = null,
    Action<int, string> onError = null)
    {
        tTRewardedVideoAd = TT.CreateRewardedVideoAd(adId,
            (isValid, duration) =>
            {
                onClose?.Invoke(isValid, duration);

            },
            (errCode, errMsg) =>
            {
                onError?.Invoke(errCode, errMsg);

            });
        var rewardedAd = tTRewardedVideoAd;
        if (rewardedAd != null)
        {
            rewardedAd.Show();
            rewardedAd.OnClose += (ended, count) =>
            {

                StartCoroutine("SendPostRequest");//加载激活回调
                //onClose?.Invoke(ended, count);
                Debug.Log($"激励视频关闭 ended: {ended}, count: {count}");
                tTRewardedVideoAd.Destroy();
                rewardedAd.Destroy();
            };

        }
        else
        {
            if (!_isRewardAdLoading)
            {
                rewardedAd.Load();
                _isRewardAdLoading = true;
            }
        }

    }

    private string url = "https://analytics.oceanengine.com/api/v2/conversion";//micro_game_ltv
    IEnumerator SendPostRequest()
    {
        TTSDK.LaunchOption launchOption = TT.GetLaunchOptionsSync();
        if (launchOption.Query != null && launchOption.Query.ContainsKey("clickid"))
        {
            Dictionary<string, object> postData = new Dictionary<string, object>
    {
        { "event_type", "micro_game_ltv" },
        { "context", new Dictionary<string, object>
            {
                { "ad", new Dictionary<string, object>
                    {
                        { "callback", launchOption.Query["clickid"]} // 替换为实际的clickid   
					}
                }
            }
        },
        { "timestamp", System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() } // 当前时间戳
    };
            // 将字典转换为JSON格式
            string json = JsonConvert.SerializeObject(postData);
            // 创建UnityWebRequest对象
            using (UnityWebRequest request = UnityWebRequest.Post(url, json))
            {
                // 设置请求头
                request.SetRequestHeader("Content-Type", "application/json");

                // 设置POST请求的body
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                // 发送请求
                yield return request.SendWebRequest();

                // 检查请求是否成功
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("sssError: " + request.error);
                }
                else
                {


                    Debug.Log("sssResponse: " + request.downloadHandler.text);
                }
            }
        }
    }


    // 播放指定ID的激励广告
    //public void PlayRewardedAd(string adId,
    //    Action<bool, int> onClose = null,
    //    Action<int, string> onError = null)
    //{
    //    if (!_rewardedAds.ContainsKey(adId))
    //    {

    //        CreateRewardedAd(adId, onClose, onError);
    //        return;
    //    }

    //    var rewardedAd = _rewardedAds[adId];
    //    if (rewardedAd != null )
    //    {
    //        rewardedAd.Show();
    //        rewardedAd.OnClose += (ended, count) =>
    //        {
    //            onClose?.Invoke(ended, count); 
    //            Debug.Log($"激励视频关闭 ended: {ended}, count: {count}");
    //        };
    //    }
    //    else
    //    {
    //        if (!_isRewardAdLoading)
    //        {
    //            rewardedAd.Load();
    //            _isRewardAdLoading = true;
    //        }
    //    }
    //}

    //// 预加载指定ID的激励广告
    //private void PreloadRewardAd(string adId)
    //{

    //    if (!_rewardedAds.ContainsKey(adId))
    //    {
    //        CreateRewardedAd(adId);
    //    }
    //    else
    //    {
    //        _rewardedAds[adId].Load();
    //    }
    //}

    //// 创建指定ID的激励广告
    //private void CreateRewardedAd(string adId,
    //    Action<bool, int> onClose = null,
    //    Action<int, string> onError = null)
    //{
    //    var newAd = TT.CreateRewardedVideoAd(adId,
    //        (isValid, duration) =>
    //        {
    //            onClose?.Invoke(isValid, duration);
    //            PreloadRewardAd(adId); // 自动重新加载
    //        },
    //        (errCode, errMsg) =>
    //        {
    //            onError?.Invoke(errCode, errMsg);
    //            _isRewardAdLoading = false;
    //        });

    //    newAd.OnLoad += () =>
    //    {
    //        Debug.Log($"激励广告（ID: {adId}）预加载完成");
    //        _isRewardAdLoading = false;
    //    };
    //    Debug.Log(_rewardedAds.Count);
    //    _rewardedAds.Add(adId, newAd);
    //    _rewardedAds[adId] = newAd; // 将新创建的广告实例存储到字典中

    //}
    #endregion

    #region 插屏广告
    public void PlayInterstitialAd(
        Action onClose = null,
        Action<int, string> onError = null)
    {
        if (_interstitialAd == null)
        {
            CreateInterstitialAd(onClose, onError);
            return;
        }

        if (_interstitialAd.IsLoaded())
        {
            _interstitialAd.Show();
        }
        else
        {
            if (!_isInterstitialAdLoading)
            {
                _interstitialAd.Load();
                _isInterstitialAdLoading = true;
            }
        }
    }

    private void PreloadInterstitialAd()
    {
        if (_interstitialAd == null)
        {
            CreateInterstitialAd();
        }
        else
        {
            _interstitialAd.Load();
        }
    }

    private void CreateInterstitialAd(
        Action onClose = null,
        Action<int, string> onError = null)
    {
        _interstitialAd = TT.CreateInterstitialAd(new CreateInterstitialAdParam
        {
            InterstitialAdId = INTERSTITIAL_ID
        });

        _interstitialAd.OnLoad += () =>
        {
            Debug.Log("插屏广告预加载完成");
            _isInterstitialAdLoading = false;
        };

        _interstitialAd.OnClose += () =>
        {
            onClose?.Invoke();
            PreloadInterstitialAd();
        };

        _interstitialAd.OnError += (errCode, errMsg) =>
        {
            onError?.Invoke(errCode, errMsg);
            _isInterstitialAdLoading = false;
        };
    }
    #endregion

    #region 横幅广告
    public void ShowBanner(
        Action onShow = null,
        Action<int, string> onError = null)
    {
        if (_bannerAd == null)
        {
            CreateBannerAd(onShow, onError);
        }

        if (_bannerAd != null)
        {
            _bannerAd.Show();
        }
    }

    public void HideBanner()
    {
        if (_bannerAd != null)
        {
            _bannerAd.Hide();
        }
    }

    private void CreateBannerAd(
        Action onShow = null,
        Action<int, string> onError = null)
    {
        var style = new TTBannerStyle();
        style.top = 0;
        style.left = 0;
        style.width = 100;
        _bannerAd = TT.CreateBannerAd(new CreateBannerAdParam
        {
            BannerAdId = BANNER_ID,
            Style = style,
            AdIntervals = 1
        });

        _bannerAd.OnLoad += () =>
        {
            Debug.Log("横幅广告加载完成");
            onShow?.Invoke();
        };

        _bannerAd.OnError += (errCode, errMsg) =>
        {
            onError?.Invoke(errCode, errMsg);
        };
        _bannerAd.Show();
    } 
    #endregion
}
// 激励广告
//byteGameAdManager.PlayRewardedAd("mi5ag76n0bg4506577",
//            (isValid, duration) =>
//            {
//                //isValid广告是否播放完，正常游戏逻辑在以下部分
//                Debug.LogError(0);
//                if (!isValid)
//                {
//                    if (selectedMode == sceneName)
//                    {
//                        SceneManager.LoadScene(sceneName);

//                    }


//                }


//            },
//            (errCode, errMsg) =>
//            {
//                Debug.LogError(1);
//            });

//// 插屏广告
//ByteGameAdManager.Instance.PlayInterstitialAd(
//    () => { ResumeGame()},
//    (errCode, errMsg) => { HandleError()});

//// 横幅广告
//ByteGameAdManager.Instance.ShowBanner();
//ByteGameAdManager.Instance.HideBanner();