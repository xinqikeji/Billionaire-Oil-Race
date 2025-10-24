using System.Collections;
using System.Collections.Generic;
using StarkSDKSpace;
using UnityEngine;
using UnityEngine.UI;
using TTSDK;
using TTSDK.UNBridgeLib.LitJson;

public class SDKTest : MonoBehaviour
{
    public Image tip;
   
    public Button startButton;
    public Button stopButton;
    public Button shareBtn;


    public void OPenpop(string pop)
    {
        tip.gameObject.SetActive(true);
        tip.GetComponentInChildren<Text>().text = pop;
       
        Invoke("closepop", 1f);
    }
    void closepop()
    {
        tip.gameObject.SetActive(false);
    }

    [SerializeField] private bool m_IsRecordAudio = true;
    [SerializeField] private int m_MaxRecordTime = 300; // 最大录制时长（秒）

    private TTGameRecorder m_TTGameRecorder;

    void Start()
    {
        tip.gameObject.SetActive(false);
        startButton.onClick.AddListener(OnStartButtonTapped);
        stopButton.onClick.AddListener(OnStopButtonTapped);       // 初始化SDK录屏组件
        m_TTGameRecorder = TT.GetGameRecorder();
        shareBtn.onClick.AddListener(share);
        // 初始化按钮状态
        SetUIState(m_TTGameRecorder.GetEnabled());
    }

    public void share()
    {
        if(videopath==null)
        {
            OPenpop("录屏路径为空");return;
        }
        if(lupintime<3)
        {
            OPenpop("小游戏录屏时间小于 3 "); return;
        }
        JsonData shareJson = new JsonData();
        shareJson["channel"] = "video";
        shareJson["extra"] = new JsonData();

        shareJson["extra"]["videoPath"] = videopath ; ;//录屏分享的话，路径是 OnRecordComplete 拿到的路径
     JsonData videoTopics = new JsonData();
        videoTopics.SetJsonType(JsonType.Array);


        shareJson["extra"]["hashtag_list"] = videoTopics;
        TT.ShareAppMessage(shareJson, (data) =>
        {
            Debug.Log($"ShareAppMessage success: {data}");
            OPenpop("分享成功");
        },
        (errMsg) =>
        {
            Debug.Log($"ShareAppMessage failed: {errMsg}");
            OPenpop(errMsg);
        },
        () =>
        {
            Debug.Log($"ShareAppMessage cancel");
            OPenpop("分享取消");
        });

    }
    // 开始录屏按钮点击
    public void OnStartButtonTapped()
    {
        if (m_TTGameRecorder.GetVideoRecordState() != TTGameRecorder.VideoRecordState.RECORD_STARTED)
        {
            m_TTGameRecorder.Start(
                m_IsRecordAudio,
                m_MaxRecordTime,
                OnRecordStart,
                OnRecordError,
                OnRecordTimeout
            );
        }
        else
        {
            Debug.LogWarning("录屏已经开始，请勿重复点击");
        }
    }

    // 停止录屏按钮点击
    public void OnStopButtonTapped()
    {
        m_TTGameRecorder.Stop(OnRecordComplete, OnRecordError);
    }

    // 录屏开始回调
    private void OnRecordStart()
    {
        OPenpop("录屏开始");
        SetUIState(false);
    }
    public string videopath;
    // 录屏完成回调
    private void OnRecordComplete(string videoPath)
    {
        videopath = videoPath; lupintime = m_TTGameRecorder.GetRecordDuration();
        OPenpop($"录屏完成: 时长: {m_TTGameRecorder.GetRecordDuration() }秒");
        SetUIState(true);
    }
    public int lupintime;
    // 录屏超时回调
    private void OnRecordTimeout(string videoPath)
    {
        videopath = videoPath; lupintime = m_TTGameRecorder.GetRecordDuration();
        OPenpop($"录屏超时: {videoPath}\n已录制时长: {m_TTGameRecorder.GetRecordDuration()}秒");
        SetUIState(true);
    }

    // 错误处理（合并重复方法）
    private void OnRecordError(int errCode, string errMsg)
    {
        OPenpop($"录屏错误: [{errCode}] {errMsg}");
        SetUIState(true);
    }

    // 统一控制UI状态
    private void SetUIState(bool isRecording)
    {
        // 确保在主线程更新UI
        
            startButton.gameObject.SetActive(isRecording);
            stopButton.gameObject.SetActive(!isRecording);

            // 保持按钮交互性
            startButton.interactable = isRecording;
            stopButton.interactable = !isRecording;
        
    }
}