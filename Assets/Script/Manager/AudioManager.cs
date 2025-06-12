using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : BaseManager<AudioManager>
{
    private AudioSource BkMusic=null;//背景音乐
    private float BkMusicVolume=1f;//音量

    private GameObject SoundObj = null;//用于挂载音效的对象
    private List<AudioSource> soundList = new List<AudioSource>();//音效列表
    private float soundVolume=1f;

    public bool BKMusic_isPlaying;//背景音乐是否正在播放
    public string BKMusic_Name;
    public float BKMusic_PlaybackTime;//背景音乐已经播放的时间
    public float BKMusic_TotalDuration;//背景音乐总时长
    public float BKMusic_PlayBackPercentage;//背景音乐播放的百分比
    public AudioManager()
    {
        MonoManager.GetInstance().AddUpdateLisener(Update);
    }
    private void Update()
    {
        for(int i=soundList.Count-1; i>=0; i++)
        {
            // 增加 null 检查
            if (soundList[i] == null)
            {
                soundList.RemoveAt(i);
                continue;
            }
            if (!soundList[i].isPlaying)//如果音频播放完毕
            {
                GameObject.Destroy(soundList[i]);//销毁该音效
                soundList.RemoveAt(i);//从音效列表中移除
            }
        }
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name">音乐名称</param>
    /// <param name="action">播放时使用的方法</param>
    /// <param name="UpdateInfo">音乐的数据（长度/已播放时间等）</param>
    public void PlayBKMusic(string name,UnityAction action=null,bool UpdateInfo=false)
    {
        
        if (BkMusic == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = "BKMusic";
            BkMusic = gameObject.AddComponent<AudioSource>();
        }
        //异步加载背景音乐 加载完成后播放
        ResourceManager.GetInstance().LoadAsync<AudioClip>("Audio/BackGroundMusic/"+name,(Clip)=>
        {
            BkMusic.clip = Clip;//设置音效
            BkMusic.loop = true;//循环播放
            BkMusic.volume = BkMusicVolume;
            BkMusic.Play();
            if(UpdateInfo)
            {
                BKMusicInfo();
                MonoManager.GetInstance().AddUpdateLisener(BKMusicInfo);
            }
            action?.Invoke();
        });

    }
    /// <summary>
    /// 更新背景音乐信息
    /// </summary>
    public void BKMusicInfo()
    {
        BKMusic_isPlaying = BkMusic != null && BkMusic.isPlaying;
        BKMusic_Name = BkMusic.clip.name;
        BKMusic_PlaybackTime = BkMusic.time;
        BKMusic_TotalDuration = BkMusic.clip.length;
        BKMusic_PlayBackPercentage = BKMusic_PlaybackTime / BKMusic_TotalDuration;
        //Debug.Log(BKMusic_isPlaying + " " + BKMusic_Name + " " + BKMusic_PlaybackTime + " " + BKMusic_TotalDuration + " " + BKMusic_PlayBackPercentage);
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBKMusic()
    {
        if (BkMusic == null)
        {
            return;
        }
        BkMusic.Pause();
    }
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBKMusic()
    {
        if (BkMusic==null)
        {
            return;
        }
        BkMusic.Stop();
    }
    /// <summary>
    /// 改变背景音乐音量
    /// </summary>
    /// <param name="volumeValue">音量</param>
    public void ChangeBKValue(float volumeValue)
    {
        BkMusicVolume = volumeValue;
        if (BkMusic != null)
        {
            return;//return:立即退出当前方法
        }
        BkMusic.volume = volumeValue;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name,bool isLoop,UnityAction<AudioSource> callBack=null)
    {
        if(SoundObj==null)
        {
            SoundObj = new GameObject();//生成挂载音效的对象
            SoundObj.name = "Sound";
        }
        //异步加载背景音乐 加载完成后添加在音效列表中
        ResourceManager.GetInstance().LoadAsync<AudioClip>("Audio/Sound/" + name, (Clip) =>
        {
            AudioSource audio = SoundObj.AddComponent<AudioSource>();
            audio.clip = Clip;
            audio.loop = isLoop;
            audio.volume = BkMusicVolume;
            audio.Play();
            soundList.Add(audio);
            if(callBack!=null)
            {
                callBack(audio);
            }
        });
    }
    /// <summary>
    /// 改变音效声音大小
    /// </summary>
    /// <param name="volumeValue">音量</param>
    public void ChangeSoundValue(float volumeValue)
    {
        soundVolume = volumeValue;
        for(int i=0;i<soundList.Count;i++)
        {
            soundList[i].volume = volumeValue;
        }
    }
    /// <summary>
    /// 音效停止
    /// </summary>
    /// <param name="source">音效</param>
    public void StopSound(AudioSource source)
    {
        if(soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }
}
