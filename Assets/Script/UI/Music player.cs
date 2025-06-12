using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Musicplayer : NewUIBase
{
    [Header("————音乐播放器UI————")]
    public Button Pause_Button;
    public Button Play_Button;

    public TextMeshProUGUI Music_Name;//音乐名称

    public TextMeshProUGUI Music_NowTime;//音乐的已播放时间
    public TextMeshProUGUI Music_TotalTime;//音乐的总时间

    public Image Progress_Bar;
    [Header("————音乐列表/路径————")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField] private string folderPath = ""; // Resources 下的文件夹路径（可选）
    [Header("————数据————")]
    public int CurrentIndex = 0;
    void Start()
    {
        LoadAudioClips(folderPath);

        Play();
        #region 调试用方法
        // 打印加载结果（调试用）
        //foreach (var clip in audioClips)
        //{
        //    Debug.Log("Loaded AudioClip: " + clip.name);
        //}
        #endregion
    }
    #region 播放控制
    protected override void OnClick(string btnName, Button button)
    {
        switch (btnName)
        {
            case "Next_Button":
                Next();
                break;
            case "Last_Button":
                Last();
                break;
            case "Play_Button":
                Play();
                break;
            case "Pause_Button":
                Pause();
                break;
        }
    }
    public void Next()
    {
        if(CurrentIndex<audioClips.Count-1)
        {
            CurrentIndex++;
            AudioManager.GetInstance().PlayBKMusic(audioClips[CurrentIndex].name, InfoUpdate, true);
        }
        else
        {
            CurrentIndex = 0;
            AudioManager.GetInstance().PlayBKMusic(audioClips[CurrentIndex].name, InfoUpdate, true);
        }
    }
    public void Last()
    {
        if (CurrentIndex > 0)
        {
            CurrentIndex--;
            AudioManager.GetInstance().PlayBKMusic(audioClips[CurrentIndex].name, InfoUpdate, true);
        }
        else
        {
            CurrentIndex = audioClips.Count-1;
            AudioManager.GetInstance().PlayBKMusic(audioClips[CurrentIndex].name, InfoUpdate, true);
        }
    }
    public void Pause()
    {
        AudioManager.GetInstance().PauseBKMusic();
        Close_Open_Button(Pause_Button.GetComponent<CanvasGroup>(),true);
        Close_Open_Button(Play_Button.GetComponent<CanvasGroup>(),false);
    }
    public void Play()
    {
        AudioManager.GetInstance().PlayBKMusic(audioClips[CurrentIndex].name, InfoUpdate, true);
        Close_Open_Button(Pause_Button.GetComponent<CanvasGroup>(), false);
        Close_Open_Button(Play_Button.GetComponent<CanvasGroup>(), true);
    }
    public void Close_Open_Button(CanvasGroup canvasGroup, bool isClose = false)
    {
        if (isClose)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
    #endregion
    #region 信息更新
    /// <summary>
    /// 音乐播放器信息更新
    /// </summary>
    public void InfoUpdate()
    {
        BKMusicInfo_SignIn_Once();
        if(AudioManager.GetInstance().BKMusic_isPlaying)
        {
            MonoManager.GetInstance().AddUpdateLisener(BKMusicInfo_SignIn_NeedUpdate);
        }
        else
        {
            MonoManager.GetInstance().RemoveUpdateLisener(BKMusicInfo_SignIn_NeedUpdate);
        }
    }
    public void BKMusicInfo_SignIn_Once()
    {
        //音乐名称
        Music_Name.text = "正在播放：" + "\n" + audioClips[CurrentIndex].name;
        //音乐总长度
        Music_TotalTime.text = Minutes_Transformer(AudioManager.GetInstance().BKMusic_TotalDuration);
    }
    public void BKMusicInfo_SignIn_NeedUpdate()
    {
        //播放时间
        Music_NowTime.text = Minutes_Transformer(AudioManager.GetInstance().BKMusic_PlaybackTime);
        //进度条
        Progress_Bar.fillAmount = AudioManager.GetInstance().BKMusic_PlayBackPercentage;
    }
    /// <summary>
    /// 时间转换（秒——分）
    /// </summary>
    /// <param name="Seconds">秒</param>
    /// <returns></returns>
    public string Minutes_Transformer(float Seconds)
    {
        int totalSeconds = (int)Seconds;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return string.Format("{0}:{1:D2}", minutes, seconds);
    }
    #endregion
    #region 加载音频
    /// <summary>
    /// 从 Resources 文件夹加载所有 AudioClip 并加入列表
    /// </summary>
    /// <param name="path">子文件夹路径（相对于 Resources）</param>
    public void LoadAudioClips(string path)
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(path);

        if (clips.Length == 0)
        {
            Debug.LogWarning("未在路径下找到任何音频文件：" + path);
            return;
        }
        audioClips.Clear();
        audioClips.AddRange(clips);
    }

    /// <summary>
    /// 获取音频列表
    /// </summary>
    public List<AudioClip> GetAudioClips()
    {
        return audioClips;
    }
    #endregion
}
