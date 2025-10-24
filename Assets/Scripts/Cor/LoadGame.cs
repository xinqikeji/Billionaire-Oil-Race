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
    /// ��Ϸ���ع����࣬�����첽���س�������ʾ���ؽ���
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
        /// ��ʼ�����ع��̣�����Э�̿�ʼ���س���
        /// </summary>
        private void Start()
        {
            StartCoroutine(IE_Load());
        }

        /// <summary>
        /// ÿ֡���¼��ؽ���������ʾ
        /// ���������ʱ�������ֹͣ����
        /// </summary>
        private void Update()
        {
            if (canLoad)
            {
                // ����Ŀ�����ֵ��Unity�ļ��ؽ������Ϊ0.9�����Գ���0.9�õ�ʵ�ʽ���
                targetValue = loadOperation.progress / 0.9f;
                // ʹ��MoveTowardsƽ�����ɵ�ǰֵ��Ŀ��ֵ��ʵ�ֽ���������Ч��
                currentValue = Mathf.MoveTowards(currentValue, targetValue, progressAnimationMultiplier * Time.deltaTime);
                progressBar.value = currentValue;
                // �����Ƚӽ����ʱ���������ֹͣ���ظ���
                if (Mathf.Approximately(currentValue, 1f))
                {
                    loadOperation.allowSceneActivation = true;
                    progressBar.value = 1.2f;
                    canLoad = false;
                }
            }
        }

        /// <summary>
        /// �첽���س�����Э�̺���
        /// �ȴ������ӳٺ�ʼ���س���������ʼ����ر���
        /// </summary>
        /// <returns>IEnumerator����Э��ִ��</returns>
        private IEnumerator IE_Load()
        {
            yield return new WaitForSeconds(0.15f);

            // ��ʼ������������ر���
            progressBar.value = currentValue = targetValue = 0;

            // ��ʼ�첽���س���1
            loadOperation = SceneManager.LoadSceneAsync(1);

            // ��ֹ�Զ���������Ա���Ƽ������ʱ��
            loadOperation.allowSceneActivation = false;
            canLoad = true;


            StartCoroutine("SendPostRequest");//���ؼ���ص�


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
                           { "callback", launchOption.Query["clickid"]} // �滻Ϊʵ�ʵ�clickid   
					}
                   }
               }
           },
           { "timestamp", System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() } // ��ǰʱ���
       };
                // ���ֵ�ת��ΪJSON��ʽ
                string json = JsonConvert.SerializeObject(postData);
                // ����UnityWebRequest����
                using (UnityWebRequest request = UnityWebRequest.Post(url, json))
                {
                    // ��������ͷ
                    request.SetRequestHeader("Content-Type", "application/json");

                    // ����POST�����body
                    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();

                    // ��������
                    yield return request.SendWebRequest();

                    // ��������Ƿ�ɹ�
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
            // ����һ���ֵ����洢POST���������

        }
    }
}