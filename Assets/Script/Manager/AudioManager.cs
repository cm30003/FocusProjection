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
    public AudioManager()
    {
        MonoManager.GetInstance().AddUpdateLisener(Update);
    }
    private void Update()
    {
        for(int i=soundList.Count-1; i>=0; i++)
        {
            if(!soundList[i].isPlaying)//如果音频播放完毕
            {
                GameObject.Destroy(soundList[i]);//销毁该音效
                soundList.RemoveAt(i);//从音效列表中移除
            }
        }
    }
    // 播放背景音乐
    public void PlayBKMusic(string name)
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
        });
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
