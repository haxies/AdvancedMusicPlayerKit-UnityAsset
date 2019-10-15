using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Advanced Music Player Kit Unity Asset by Abd Majeed Abd Mumuni... 
Twitter: abdmajeedmumuni
Instagram: abdmajeedofficial
eMail: abdmajeedgeeky@gmail.com || abdmajeedgeeky@outlook.com
github: bitsandwits

Disclaimer: All Tracks used in this Asset Package, belongs to their respective owners, and are for demo purposes only...
Thanks for purchasing...
*/
public class main : MonoBehaviour
{
    public musicClass[] props;
    public listClass[] list;
    private int[] shuffled;
    public AudioSource audioSource;
    public Slider slider;
    public Slider volume;
    public GameObject Play;
    public GameObject Pause;
    public GameObject loopOn;
    public GameObject loopOff;
    public GameObject shuffleOn;
    public GameObject shuffleOff;
    public GameObject trackList;
    public Text Title;
    public Text albumName;
    public Text artistName;
    public Image Cover;
    public Text timeSeconds;
    public Text timeMinutes;
    public Text endMinute;
    public Text endSecond;
    public Text activeArtist;
    public Text activeTitle;
    public Image activeSprite;
    void Start()
    {
        CheckCurrentTrack();
        shuffled = new int[props.Length];
        volume.value = audioSource.volume;
    }

    void CheckCurrentTrack() //Gets last played track, on last player launch...
    {
        if (PlayerPrefs.HasKey("CurrentTrack"))
        {
            int current = PlayerPrefs.GetInt("CurrentTrack");
            Title.text = props[current].Title;
            albumName.text = props[current].albumName;
            artistName.text = props[current].artistName;
            Cover.sprite = props[current].Cover;
            audioSource.clip = props[current].Track;
        }
        else
        {
            PlayerPrefs.SetInt("CurrentTrack", 0);
            Title.text = props[0].Title;
            albumName.text = props[0].albumName;
            artistName.text = props[0].artistName;
            Cover.sprite = props[0].Cover;
            audioSource.clip = props[0].Track;
        }
    }
    public void PlayAudio() //UnPauses or Plays Track...
    {
        audioSource.Play();
    }

    public void PauseAudio() //Pauses Track...
    {
        audioSource.Pause();
    }

    public void LoopOn() //turns on Loop...
    {
        loopOn.SetActive(true);
        loopOff.SetActive(false);
    }

    public void LoopOff() //turns off Loop...
    {
        loopOff.SetActive(true);
        loopOn.SetActive(false);
    }

    public void ShuffleOn() //turns on Shuffle...
    {
        shuffleOn.SetActive(true);
        shuffleOff.SetActive(false);
        Shuffler();

    }

    public void ShuffleOff() //turn off Shuffle...
    {
        shuffleOff.SetActive(true);
        shuffleOn.SetActive(false);
        if (PlayerPrefs.HasKey("Shuffled"))
        {
            PlayerPrefs.DeleteKey("Shuffled");
        }
    }

    public void ChangeAudioTime() //slider moves simultaneously with track's progress...
    {
        audioSource.time = audioSource.clip.length * slider.value;

    }

    public void volumeControl()
    {
        audioSource.volume = volume.value;
    }
    void AudioPlaying() //shows track's current minute and second... 
    {
        slider.value = audioSource.time / audioSource.clip.length;
        float currentTime = audioSource.time;
        float minutes = Mathf.Floor(currentTime / 59);
        float seconds = Mathf.Floor(currentTime % 59);
        timeSeconds.text = seconds.ToString();
        timeMinutes.text = minutes.ToString();
        if (audioSource.isPlaying)
        {
            Pause.SetActive(true); //if track is playing, show pause button...
            Play.SetActive(false);
        }
        else
        {
            Play.SetActive(true); //if track is not playing, show play button...
            Pause.SetActive(false);
        }

    }
    void Update()
    {
        AudioPlaying();
        GetAudioLength();
        if (trackList.activeSelf == true)
        {
            TrackPlaying();
        }
    }

    void LateUpdate() //calls the JumpToNext function...
    {
        JumpToNext();
    }

    public void Next()  //if to change to next shuffled, or next track from original array...
    {
        if (shuffleOn.activeSelf == true)
        {
            NextShuffled();
        } else
        {
            NextAudio();
        }
    }

    public void Previous() //if to change to previous shuffled, or previous track from original array...
    {
        if (shuffleOn.activeSelf == true)
        {
            PrevShuffled();
        }
        else
        {
            PreviousAudio();
        }
    }

    void NextAudio() //change to next track...
    {
        int next = PlayerPrefs.GetInt("CurrentTrack");
        if (next >= 0 && next < props.Length - 1)
        {
            PlayerPrefs.SetInt("CurrentTrack", next + 1);
            int nextTrack = PlayerPrefs.GetInt("CurrentTrack");
            Title.text = props[nextTrack].Title;
            albumName.text = props[nextTrack].albumName;
            artistName.text = props[nextTrack].artistName;
            Cover.sprite = props[nextTrack].Cover;
            audioSource.clip = props[nextTrack].Track;
            audioSource.time = 0;
            audioSource.Play();
        }

    }

    void PreviousAudio() //change to previous track....
    {
        int previous = PlayerPrefs.GetInt("CurrentTrack");
        if (previous <= props.Length && previous > 0)
        {
            PlayerPrefs.SetInt("CurrentTrack", previous - 1);
            int prevTrack = PlayerPrefs.GetInt("CurrentTrack");
            Title.text = props[prevTrack].Title;
            albumName.text = props[prevTrack].albumName;
            artistName.text = props[prevTrack].artistName;
            Cover.sprite = props[prevTrack].Cover;
            audioSource.clip = props[prevTrack].Track;
            audioSource.time = 0;
            audioSource.Play();
        }
    }

    void GetAudioLength() //sets the end minute and end second of a track....
    {
        float audioLength = audioSource.clip.length;
        float endMin = Mathf.Floor(audioLength / 59);
        float endSec = Mathf.Floor(audioLength % 59);
        endMinute.text = endMin.ToString();
        endSecond.text = endSec.ToString();
    }

    void JumpToNext()  //if to loop, jump to next shuffled track or next track in the original array...
    {
        if (timeSeconds.text == endSecond.text && timeMinutes.text == endMinute.text && loopOn.activeSelf == true)
        {
            audioSource.time = 0;
            audioSource.Play();
        }
        else if (timeSeconds.text == endSecond.text && timeMinutes.text == endMinute.text && shuffleOn.activeSelf == true)
        {
            NextShuffled();
        }
        else if (timeSeconds.text == endSecond.text && timeMinutes.text == endMinute.text)
        {
            int i = PlayerPrefs.GetInt("CurrentTrack");
            if (i == props.Length - 1)
            {
                audioSource.Stop();
            } else
            {
                NextAudio();
            }
        }
    }

    void LoadList() //Loads the music track metadata from array into List Menu...
    {
        for (int i = 0; i <= props.Length - 1; i++)
        {
            list[i].artistName.text = i + 1 + ". " + props[i].artistName;
            list[i].Title.text = props[i].Title;
            list[i].button.SetActive(true);
        }
    }

    void TrackPlaying()  //makes the current playing track text, green, and makes the non-playing tracks, white...
    {
        int trackPlaying = PlayerPrefs.GetInt("CurrentTrack"); //previews the playing track below the List Menu...
        activeArtist.text = props[trackPlaying].artistName;
        activeTitle.text = props[trackPlaying].Title;
        activeSprite.sprite = props[trackPlaying].Cover;
        for (int c = 0; c <= list.Length - 1; c++)
        {
            if (trackPlaying != c)
            {
                list[c].artistName.color = Color.white;
                list[c].Title.color = Color.white;
            } else
            {
                list[c].artistName.color = Color.green;
                list[c].Title.color = Color.green;
            }
        }
    }

    public void OpenList() //opens Music List Menu...
    {
        trackList.SetActive(true);
        LoadList();
    }

    public void CloseList() //closes Music List Menu...
    {
        trackList.SetActive(false);
    }

    public void PlayTrack(int index)  //plays the selected music track in the List Menu...
    {
        if (index >= 0 && index <= props.Length - 1)
        {
            PlayerPrefs.SetInt("CurrentTrack", index);
            Title.text = props[index].Title;
            albumName.text = props[index].albumName;
            artistName.text = props[index].artistName;
            Cover.sprite = props[index].Cover;
            audioSource.clip = props[index].Track;
            audioSource.time = 0;
            audioSource.Play();
        }
    }

    void Shuffler() //shuffles the public array values into a second private array....
    {
        for (int i = 0; i <= props.Length - 1; i++)
        {
            shuffled[i] = i;
        }

        for (int j = 0; j <= props.Length - 1; j++)
        {
            int temp = shuffled[j];
            int x = Random.Range(j, shuffled.Length);
            shuffled[j] = shuffled[x];
            shuffled[x] = temp;
        }
    }



    void NextShuffled() //plays the next track by getting a value from the second private array i created...
    {
        if (PlayerPrefs.HasKey("Shuffled"))
        {
            int next = PlayerPrefs.GetInt("Shuffled");
            if (next >= 0 && next < props.Length - 1)
            {
                PlayerPrefs.SetInt("Shuffled", next + 1);
                int nextShuffled = PlayerPrefs.GetInt("Shuffled");
                Title.text = props[shuffled[nextShuffled]].Title;
                albumName.text = props[shuffled[nextShuffled]].albumName;
                artistName.text = props[shuffled[nextShuffled]].artistName;
                Cover.sprite = props[shuffled[nextShuffled]].Cover;
                audioSource.clip = props[shuffled[nextShuffled]].Track;
                audioSource.time = 0;
                audioSource.Play();

            }
        }
        else
        {
            PlayerPrefs.SetInt("Shuffled", 0);
            Title.text = props[shuffled[0]].Title;
            albumName.text = props[shuffled[0]].albumName;
            artistName.text = props[shuffled[0]].artistName;
            Cover.sprite = props[shuffled[0]].Cover;
            audioSource.clip = props[shuffled[0]].Track;
            audioSource.time = 0;
            audioSource.Play();
        }
    }

        void PrevShuffled()  ////plays the previous track by getting a value from the second private array i created...
    {
        if (PlayerPrefs.HasKey("Shuffled"))
        {
            int prev = PlayerPrefs.GetInt("Shuffled");
            if (prev > 0 && prev <= props.Length)
            {
                Title.text = props[shuffled[prev - 1]].Title;
                albumName.text = props[shuffled[prev - 1]].albumName;
                artistName.text = props[shuffled[prev - 1]].artistName;
                Cover.sprite = props[shuffled[prev - 1]].Cover;
                audioSource.clip = props[shuffled[prev - 1]].Track;
                audioSource.time = 0;
                audioSource.Play();
                PlayerPrefs.SetInt("Shuffled", prev - 1);
            }   
        }    
    }
    public void Reset()  //to reset the player, if needed....
    {
        PlayerPrefs.DeleteAll();
    }
}

