using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioManager : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float dialogueVolume;
    [Range(0.0f, 1.0f)]
    public float ambianceVolume;
    [Range(0.0f, 1.0f)]
    public float BGMVolume;

    public AudioClip confirmSE;
    public AudioClip backSE;
    public AudioClip cantActionSE;

    public AudioClip equipSE; //not the correct equip SE but ok for now
    public AudioClip openMenuSE;

    public AudioClip purchaseSE;
    
    #region Singleton
    public static AudioManager instance; //call instance to get the single active inventory for the game

    private void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    public void PlaySE(AudioClip audio)
    {
        GameObject.Find("AudioManager/BGS").GetComponent<AudioSource>().PlayOneShot(audio);
    }

    private void Update()
    {

    }

}
