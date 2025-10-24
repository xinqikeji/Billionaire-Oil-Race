using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using TTSDK;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BlueStellar.Cor
{
    /// <summary>
    /// 游戏加载管理类，负责异步加载场景并显示加载进度
    /// </summary>
    public class LoadGame : MonoBehaviour
    {
        private AsyncOperation loadOperation;

        [SerializeField]
        private Slider progressBar;

        private float currentValue;
        private float targetValue;

        [SerializeField]
        [Range(0, 1)]
        private float progressAnimationMultiplier = 0.25f;

        private bool canLoad;

        /// <summary>
        /// 初始化加载过程，启动协程开始加载场景
        /// </summary>
        private void Start()
        {
            StartCoroutine(IE_Load());
        }

        /// <summary>
        /// 每帧更新加载进度条的显示
        /// 当加载完成时激活场景并停止更新
        /// </summary>
        private void Update()
        {
            if (canLoad)
            {
                // 计算目标进度值，Unity的加载进度最大为0.9，所以除以0.9得到实际进度
                targetValue = loadOperation.progress / 0.9f;
                // 使用MoveTowards平滑过渡当前值到目标值，实现进度条动画效果
                currentValue = Mathf.MoveTowards(currentValue, targetValue, progressAnimationMultiplier * Time.deltaTime);
                progressBar.value = currentValue;
                // 当进度接近完成时，激活场景并停止加载更新
                if (Mathf.Approximately(currentValue, 1f))
                {
                    loadOperation.allowSceneActivation = true;
                    progressBar.value = 1.2f;
                    canLoad = false;
                }
            }
        }

        /// <summary>
        /// 异步加载场景的协程函数
        /// 等待短暂延迟后开始加载场景，并初始化相关变量
        /// </summary>
        /// <returns>IEnumerator用于协程执行</returns>
        private IEnumerator IE_Load()
        {
            yield return new WaitForSeconds(0.15f);

            // 初始化进度条和相关变量
            progressBar.value = currentValue = targetValue = 0;

            // 开始异步加载场景1
            loadOperation = SceneManager.LoadSceneAsync(1);

            // 禁止自动激活场景，以便控制加载完成时机
            loadOperation.allowSceneActivation = false;
            canLoad = true;


            StartCoroutine("SendPostRequest");//加载激活回调


        }


        private string url = "https://analytics.oceanengine.com/api/v2/conversion";
        IEnumerator SendPostRequest()
        {
            TTSDK.LaunchOption launchOption = TT.GetLaunchOptionsSync();
            if (launchOption.Query != null && launchOption.Query.ContainsKey("clickid"))
            {
                Dictionary<string, object> postData = new Dictionary<string, object>
       {
           { "event_type", "active" },
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
            // 创建一个字典来存储POST请求的数据

        }
    }
}