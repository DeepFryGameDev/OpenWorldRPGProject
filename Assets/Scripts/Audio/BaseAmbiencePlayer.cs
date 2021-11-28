using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAmbiencePlayer : MonoBehaviour
{
    [ReadOnly] public bool ready = false;
    [ReadOnly] public AudioClip fadingIn, fadingOut;

    OneDaySystem OneDayController;
    int runIndex;

    public enum AmbienceTypes
    {
        NONE,
        BIRDS,
        CICADAS,
        WINDY,
        LAKE,
        RIVER
    }

    public AmbienceTypes morningAmbience;
    public AmbienceTypes noonAmbience;
    public AmbienceTypes eveningAmbience;
    public AmbienceTypes nightAmbience;

    [ReadOnly] public List<AudioClip> morningClips;
    [ReadOnly] public List<AudioClip> noonClips;
    [ReadOnly] public List<AudioClip> eveningClips;
    [ReadOnly] public List<AudioClip> nightClips;

    public bool outputTime;
    public bool outputVolumeChange;
    public bool outputClipChange;
    public float volumeOffset;

    AudioSource source_03;
    AudioSource source_09;
    AudioSource source_15;
    AudioSource source_21;

    bool muteAllOnce = false;

    void Start()
    {
        OneDayController = GameObject.Find("[WORLD]/OneDayController").GetComponent<OneDaySystem>();

        runIndex = 99; //force ambience to run at least once

        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    private void Update()
    {
        if (ready)
        {
            ProcessAmbience();
        }        
    }

    void ProcessAmbience()
    {
        int time = OneDayController.time;

        //Debug.Log(time);
        source_03 = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        source_09 = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        source_15 = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        source_21 = transform.GetChild(3).gameObject.GetComponent<AudioSource>();

        #region 12-230
        if (time >= 0 && time < 1800) //12am - 18 at 40%, 00 at 40%
        {
            if (runIndex != 0)
            {
                if (outputTime) Debug.Log("TIME -   12:00 AM");
                SetSourceVolume(source_21, 40);
                SetSourceVolume(source_03, 40);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 0;
        }
        else if (time >= 1800 && time < 3600)  //1230am - 18 35%, 00 45%
        {
            if (runIndex != 1)
            {
                if (outputTime) Debug.Log("TIME -  12:30 AM");
                SetSourceVolume(source_21, 35);
                SetSourceVolume(source_03, 45);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 1;
        }
        else if (time >= 3600 && time < 5400) //1am - 18 at 30%, 00 at 50%
        {
            if (runIndex != 2)
            {
                if (outputTime) Debug.Log("TIME -  01:00 AM");
                SetSourceVolume(source_21, 30);
                SetSourceVolume(source_03, 50);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 2;
        }
        else if (time >= 5400 && time < 7200) //130am - 18 25%, 00 55%
        {
            if (runIndex != 3)
            {
                if (outputTime) Debug.Log("TIME -  01:30 AM");
                SetSourceVolume(source_21, 25);
                SetSourceVolume(source_03, 55);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 3;
        }
        else if (time >= 7200 && time < 9000) //2am - 18 at 20%, 00 at 60%
        {
            if (runIndex != 4)
            {
                if (outputTime) Debug.Log("TIME -  02:00 AM");
                SetSourceVolume(source_21, 20);
                SetSourceVolume(source_03, 60);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 4;
        }
        else if (time >= 9000 && time < 10800) //230am - 18 15%, 00 65%
        {
            if (runIndex != 5)
            {
                if (outputTime) Debug.Log("TIME -  02:30 AM");
                SetSourceVolume(source_21, 15);
                SetSourceVolume(source_03, 65);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 5;
        }
        #endregion
        #region 3-530
        else if (time >= 10800 && time < 12600) //3am - 18 at 10%. 00 at 70%, 06 at 10%
        {
            if (runIndex != 6)
            {
                if (outputTime) Debug.Log("TIME -  03:00 AM");
                SetSourceVolume(source_21, 10);
                SetSourceVolume(source_03, 70);

                SetClip(source_09, morningClips);
                SetSourceVolume(source_09, 10);
                source_09.Play();

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 6;
        }
        else if (time >= 12600 && time < 14400) //330am - cut off 18. 00 65%, 06 15%
        {
            if (runIndex != 7)
            {
                if (outputTime) Debug.Log("TIME -  03:30 AM");
                SetSourceVolume(source_21, 00);
                source_21.Stop();

                SetSourceVolume(source_03, 65);
                SetSourceVolume(source_09, 15);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 7;
        }
        else if (time >= 14400 && time < 16200) //4am - 00 at 60%, 06 at 20%
        {
            if (runIndex != 8)
            {
                if (outputTime) Debug.Log("TIME -  04:00 AM");
                SetSourceVolume(source_03, 60);
                SetSourceVolume(source_09, 20);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 8;
        }
        else if (time >= 16200 && time < 18000) //430am - 00 55%, 06 25%
        {
            if (runIndex != 9)
            {
                if (outputTime) Debug.Log("TIME -  04:30 AM");
                SetSourceVolume(source_03, 55);
                SetSourceVolume(source_09, 25);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 9;
        }
        else if (time >= 18000 && time < 19800) //5am - 00 at 50%, 06 at 30%
        {
            if (runIndex != 10)
            {
                if (outputTime) Debug.Log("TIME -  05:00 AM");
                SetSourceVolume(source_03, 50);
                SetSourceVolume(source_09, 30);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 10;
        }
        else if (time >= 19800 && time < 21600) //530am - 00 45%, 06 35%
        {
            if (runIndex != 11)
            {
                if (outputTime) Debug.Log("TIME -  05:30 AM");
                SetSourceVolume(source_03, 45);
                SetSourceVolume(source_09, 35);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 11;
        }
        #endregion
        #region 6-830
        else if (time >= 21600 && time < 23400) //6am - 00 at 40%, 06 at 40%
        {
            if (runIndex != 12)
            {
                if (outputTime) Debug.Log("TIME -  06:00 AM");
                SetSourceVolume(source_03, 40);
                SetSourceVolume(source_09, 30);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 12;
        }
        else if (time >= 23400 && time < 25200) //630am - 00 35%, 06 45%
        {
            if (runIndex != 13)
            {
                if (outputTime) Debug.Log("TIME -  06:30 AM");
                SetSourceVolume(source_03, 35);
                SetSourceVolume(source_09, 45);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 13;
        }
        else if (time >= 25200 && time < 27000) //7am - 00 at 30%, 06 at 50%
        {
            if (runIndex != 14)
            {
                if (outputTime) Debug.Log("TIME -  07:00 AM");
                SetSourceVolume(source_03, 30);
                SetSourceVolume(source_09, 50);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 14;
        }
        else if (time >= 27000 && time < 28800) //730am - 00 25%, 06 55%
        {
            if (runIndex != 15)
            {
                if (outputTime) Debug.Log("TIME -  07:30 AM");
                SetSourceVolume(source_03, 25);
                SetSourceVolume(source_09, 55);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 15;
        }
        else if (time >= 28800 && time < 30600) //8am - 00 at 20%, 06 at 60%
        {
            if (runIndex != 16)
            {
                if (outputTime) Debug.Log("TIME -  08:00 AM");
                SetSourceVolume(source_03, 20);
                SetSourceVolume(source_09, 60);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 16;
        }
        else if (time >= 30600 && time < 32400) //830am - 00 15%, 06 65%
        {
            if (runIndex != 17)
            {
                if (outputTime) Debug.Log("TIME -  08:30 AM");
                SetSourceVolume(source_03, 15);
                SetSourceVolume(source_09, 65);

                fadingOut = source_03.clip;
                fadingIn = source_09.clip;
            }
            runIndex = 17;
        }
        #endregion
        #region 9-1130
        else if (time >= 32400 && time < 34200) //9am - 00 at 10%. 06 at 70%, 12 at 10%
        {
            if (runIndex != 18)
            {
                if (outputTime) Debug.Log("TIME -  09:00 AM");
                SetSourceVolume(source_03, 10);
                SetSourceVolume(source_09, 70);

                SetClip(source_15, noonClips);
                SetSourceVolume(source_15, 10);
                source_15.Play();

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 18;
        }
        else if (time >= 34200 && time < 36000) //930am - cut off 00. 06 65%, 12 15%
        {
            if (runIndex != 19)
            {
                if (outputTime) Debug.Log("TIME -  09:30 AM");
                SetSourceVolume(source_03, 00);
                source_03.Stop();

                SetSourceVolume(source_09, 65);
                SetSourceVolume(source_15, 15);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 19;
        }
        else if (time >= 36000 && time < 37800) //10am - 06 at 60%, 12 at 20%
        {
            if (runIndex != 20)
            {
                if (outputTime) Debug.Log("TIME -  10:00 AM");
                SetSourceVolume(source_09, 60);
                SetSourceVolume(source_15, 20);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 20;
        }
        else if (time >= 37800 && time < 39600) //1030am - 06 55%, 12 25%
        {
            if (runIndex != 21)
            {
                if (outputTime) Debug.Log("TIME -  10:30 AM");
                SetSourceVolume(source_09, 55);
                SetSourceVolume(source_15, 25);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 21;
        }
        else if (time >= 39600 && time < 41400) //11am - 06 at 50%, 12 at 30%
        {
            if (runIndex != 22)
            {
                if (outputTime) Debug.Log("TIME -  11:00 AM");
                SetSourceVolume(source_09, 50);
                SetSourceVolume(source_15, 30);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 22;
        }
        else if (time >= 41400 && time < 43200) //1130am - 06 45%, 12 35%
        {
            if (runIndex != 23)
            {
                if (outputTime) Debug.Log("TIME -  11:30 AM");
                SetSourceVolume(source_09, 45);
                SetSourceVolume(source_15, 35);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 23;
        }
        #endregion
        #region 12-230
        else if (time >= 43200 && time < 45000) //12pm - 06 at 40%, 12 at 40%
        {
            if (runIndex != 24)
            {
                if (outputTime) Debug.Log("TIME -  12:00 PM");
                SetSourceVolume(source_09, 40);
                SetSourceVolume(source_15, 40);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 24;
        }
        else if (time >= 45000 && time < 46800) //1230pm - 06 at 35%, 12 at 45%
        {
            if (runIndex != 25)
            {
                if (outputTime) Debug.Log("TIME -  12:30 PM");
                SetSourceVolume(source_09, 35);
                SetSourceVolume(source_15, 45);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 25;
        }
        else if (time >= 46800 && time < 48600) //1pm - 06 at 30%, 12 at 50%
        {
            if (runIndex != 26)
            {
                if (outputTime) Debug.Log("TIME -  01:00 PM");
                SetSourceVolume(source_09, 30);
                SetSourceVolume(source_15, 50);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 26;
        }
        else if (time >= 48600 && time < 50400) //130pm - 06 at 25%, 12 at 55%
        {
            if (runIndex != 27)
            {
                if (outputTime) Debug.Log("TIME -  01:30 PM");
                SetSourceVolume(source_09, 25);
                SetSourceVolume(source_15, 55);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 27;
        }
        else if (time >= 50400 && time < 52200) //2pm - 06 at 20%, 12 at 60%
        {
            if (runIndex != 28)
            {
                if (outputTime) Debug.Log("TIME -  02:00 PM");
                SetSourceVolume(source_09, 20);
                SetSourceVolume(source_15, 60);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 28;
        }
        else if (time >= 52200 && time < 54000) //230pm - 06 at 15%, 12 at 65%
        {
            if (runIndex != 29)
            {
                if (outputTime) Debug.Log("TIME -  02:30 PM");
                SetSourceVolume(source_09, 15);
                SetSourceVolume(source_15, 65);

                fadingOut = source_09.clip;
                fadingIn = source_15.clip;
            }
            runIndex = 29;
        }
        #endregion
        #region 3-530
        else if (time >= 54000 && time < 55800) //3pm - 06 at 10%, 12 at 70%, 18 at 10%
        {
            if (runIndex != 30)
            {
                if (outputTime) Debug.Log("TIME -  03:00 PM");
                SetSourceVolume(source_09, 10);
                SetSourceVolume(source_15, 70);

                SetClip(source_21, eveningClips);
                SetSourceVolume(source_21, 10);
                source_21.Play();

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 30;
        }
        else if (time >= 55800 && time < 57600) //330pm - cut off 06. 12 65%, 18 15%
        {
            if (runIndex != 31)
            {
                if (outputTime) Debug.Log("TIME -  03:30 PM");
                SetSourceVolume(source_09, 00);
                source_09.Stop();

                SetSourceVolume(source_15, 65);
                SetSourceVolume(source_21, 15);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 31;
        }
        else if (time >= 57600 && time < 59400) //4pm - 12 at 60%, 18 at 20%
        {
            if (runIndex != 32)
            {
                if (outputTime) Debug.Log("TIME -  04:00 PM");
                SetSourceVolume(source_15, 60);
                SetSourceVolume(source_21, 20);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 32;
        }
        else if (time >= 59400 && time < 61200) //430pm - 12 55%, 18 25%
        {
            if (runIndex != 33)
            {
                if (outputTime) Debug.Log("TIME -  04:30 PM");
                SetSourceVolume(source_15, 55);
                SetSourceVolume(source_21, 25);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 33;
        }
        else if (time >= 61200 && time < 63000) //5pm - 12 at 50%, 18 at 30%
        {
            if (runIndex != 34)
            {
                if (outputTime) Debug.Log("TIME -  05:00 PM");
                SetSourceVolume(source_15, 50);
                SetSourceVolume(source_21, 30);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 34;
        }
        else if (time >= 63000 && time < 64800) //530pm - 12 45%, 18 35%
        {
            if (runIndex != 35)
            {
                if (outputTime) Debug.Log("TIME -  05:30 PM");
                SetSourceVolume(source_15, 45);
                SetSourceVolume(source_21, 35);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 35;
        }
        #endregion
        #region 6-830
        else if (time >= 64800 && time < 66600) //6pm - 12 at 40%, 18 at 40%
        {
            if (runIndex != 36)
            {
                if (outputTime) Debug.Log("TIME -  06:00 PM");
                SetSourceVolume(source_15, 40);
                SetSourceVolume(source_21, 40);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 36;
        }
        else if (time >= 66600 && time < 68400) //630pm - 12 35%, 18 45%
        {
            if (runIndex != 37)
            {
                if (outputTime) Debug.Log("TIME -  06:30 PM");
                SetSourceVolume(source_15, 35);
                SetSourceVolume(source_21, 45);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 37;
        }
        else if (time >= 68400 && time < 70200) //7pm - 12 at 30%, 18 at 50%
        {
            if (runIndex != 38)
            {
                if (outputTime) Debug.Log("TIME -  07:00 PM");
                SetSourceVolume(source_15, 30);
                SetSourceVolume(source_21, 50);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 38;
        }
        else if (time >= 70200 && time < 72000) //730pm - 12 at 25%, 18 at 55%
        {
            if (runIndex != 39)
            {
                if (outputTime) Debug.Log("TIME -  07:30 PM");
                SetSourceVolume(source_15, 25);
                SetSourceVolume(source_21, 55);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 39;
        }
        else if (time >= 72000 && time < 73800) //8pm - 12 at 20%, 18 at 60%
        {
            if (runIndex != 40)
            {
                if (outputTime) Debug.Log("TIME -  08:00 PM");
                SetSourceVolume(source_15, 20);
                SetSourceVolume(source_21, 60);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 40;
        }
        else if (time >= 73800 && time < 75600) //830pm 12 at 15%, 18 at 65%
        {
            if (runIndex != 41)
            {
                if (outputTime) Debug.Log("TIME -  08:30 PM");
                SetSourceVolume(source_15, 15);
                SetSourceVolume(source_21, 65);

                fadingOut = source_15.clip;
                fadingIn = source_21.clip;
            }
            runIndex = 41;
        }
        #endregion
        #region 9-1130
        else if (time >= 75600 && time < 77400) //9pm - 12 at 10%, 18 at 70%, 00 at 10%
        {
            if (runIndex != 42)
            {
                if (outputTime) Debug.Log("TIME -  09:00 PM");
                SetSourceVolume(source_15, 10);
                SetSourceVolume(source_21, 70);

                SetClip(source_03, nightClips);
                SetSourceVolume(source_03, 10);
                source_03.Play();

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 42;
        }
        else if (time >= 77400 && time < 79200) //930pm cut off 12. 18 65%, 00 15%
        {
            if (runIndex != 43)
            {
                if (outputTime) Debug.Log("TIME -  09:30 PM");
                SetSourceVolume(source_15, 00);
                source_15.Stop();

                SetSourceVolume(source_21, 65);
                SetSourceVolume(source_03, 15);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 43;
        }
        else if (time >= 79200 && time < 81000) //10pm - 18 at 60%, 00 at 20%
        {
            if (runIndex != 44)
            {
                if (outputTime) Debug.Log("TIME -  10:00 PM");
                SetSourceVolume(source_21, 60);
                SetSourceVolume(source_03, 20);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 44;
        }
        else if (time >= 81000 && time < 82800) //1030pm - 18 55%, 60 25%
        {
            if (runIndex != 45)
            {
                if (outputTime) Debug.Log("TIME -  10:30 PM");
                SetSourceVolume(source_21, 55);
                SetSourceVolume(source_03, 25);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 45;
        }
        else if (time >= 82800 && time < 84600) //11pm - 18 at 50%, 00 at 30%
        {
            if (runIndex != 46)
            {
                if (outputTime) Debug.Log("TIME -  11:00 PM");
                SetSourceVolume(source_21, 50);
                SetSourceVolume(source_03, 30);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 46;
        }
        else if (time >= 84600 && time < 86400) //1130pm - 18 45%, 00 35%
        {
            if (runIndex != 47)
            {
                if (outputTime) Debug.Log("TIME -  11:30 PM");
                SetSourceVolume(source_21, 45);
                SetSourceVolume(source_03, 35);

                fadingOut = source_21.clip;
                fadingIn = source_03.clip;
            }
            runIndex = 47;
        }

        if (!muteAllOnce)
        {
            SetSourceVolume(source_03, 00);
            SetSourceVolume(source_09, 00);
            SetSourceVolume(source_15, 00);
            SetSourceVolume(source_21, 00);

            muteAllOnce = true;
        }
    }
        #endregion

    void SetSourceVolume(AudioSource source, int volume)
    {
        float vol = ((volume * .01f) + (volumeOffset * .01f));
        source.volume = vol;
        if (outputVolumeChange) Debug.Log(source.gameObject.name + " (" + source.clip.name + ") volume: " + (vol * 100) + "%");
    }

    public void SetVolumes()
    {
        SetSourceVolume(transform.GetChild(0).GetComponent<AudioSource>(), 00);
        SetSourceVolume(transform.GetChild(1).GetComponent<AudioSource>(), 00);
        SetSourceVolume(transform.GetChild(2).GetComponent<AudioSource>(), 00);
        SetSourceVolume(transform.GetChild(3).GetComponent<AudioSource>(), 00);
    }

    void SetClip(AudioSource source, List<AudioClip> clips)
    {
        source.clip = GetRandomClip(clips);
        if (outputClipChange) Debug.Log(source.gameObject.name + " source set to " + source.clip.name);
    }

    public void SetClips()
    {
        SetClip(transform.GetChild(0).GetComponent<AudioSource>(), morningClips);
        SetClip(transform.GetChild(1).GetComponent<AudioSource>(), noonClips);
        SetClip(transform.GetChild(2).GetComponent<AudioSource>(), eveningClips);
        SetClip(transform.GetChild(3).GetComponent<AudioSource>(), nightClips);     
    }

    AudioClip GetRandomClip(List<AudioClip> clips)
    {
        int rand = Random.Range(1, clips.Count);

        return clips[rand - 1];
    }

}
