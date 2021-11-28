using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmbienceManager : MonoBehaviour
{
    Transform ambiencePointsTransform;

    List<AudioSource> sources_03;
    List<AudioSource> sources_09;
    List<AudioSource> sources_15;
    List<AudioSource> sources_21;

    public List<AudioClip> birdClips;
    public List<AudioClip> cicadaClips;
    public List<AudioClip> windClips;
    public List<AudioClip> lakeClips;
    public List<AudioClip> riverClips;

    public bool outputTime;
    public bool outputVolumeChange;
    public bool outputClipChange;

    private void Start()
    {
        ambiencePointsTransform = GameObject.Find("[AMBIENCEPOINTS]").transform;

        foreach (Transform ambiencePoint in ambiencePointsTransform)
        {
            BaseAmbiencePlayer bap = ambiencePoint.gameObject.GetComponent<BaseAmbiencePlayer>();

            //build clip lists
            if (bap.morningAmbience != BaseAmbiencePlayer.AmbienceTypes.NONE)
            {
                switch (bap.morningAmbience)
                {
                    case BaseAmbiencePlayer.AmbienceTypes.BIRDS:
                        bap.morningClips = birdClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.CICADAS:
                        bap.morningClips = cicadaClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.WINDY:
                        bap.morningClips = windClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.LAKE:
                        bap.morningClips = lakeClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.RIVER:
                        bap.morningClips = riverClips;
                        break;
                }
            }

            if (bap.noonAmbience != BaseAmbiencePlayer.AmbienceTypes.NONE)
            {
                switch (bap.noonAmbience)
                {
                    case BaseAmbiencePlayer.AmbienceTypes.BIRDS:
                        bap.noonClips = birdClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.CICADAS:
                        bap.noonClips = cicadaClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.WINDY:
                        bap.noonClips = windClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.LAKE:
                        bap.noonClips = lakeClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.RIVER:
                        bap.noonClips = riverClips;
                        break;
                }
            }

            if (bap.eveningAmbience != BaseAmbiencePlayer.AmbienceTypes.NONE)
            {
                switch (bap.eveningAmbience)
                {
                    case BaseAmbiencePlayer.AmbienceTypes.BIRDS:
                        bap.eveningClips = birdClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.CICADAS:
                        bap.eveningClips = cicadaClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.WINDY:
                        bap.eveningClips = windClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.LAKE:
                        bap.eveningClips = lakeClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.RIVER:
                        bap.eveningClips = riverClips;
                        break;
                }
            }

            if (bap.nightAmbience != BaseAmbiencePlayer.AmbienceTypes.NONE)
            {
                switch (bap.nightAmbience)
                {
                    case BaseAmbiencePlayer.AmbienceTypes.BIRDS:
                        bap.nightClips = birdClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.CICADAS:
                        bap.nightClips = cicadaClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.WINDY:
                        bap.nightClips = windClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.LAKE:
                        bap.nightClips = lakeClips;
                        break;
                    case BaseAmbiencePlayer.AmbienceTypes.RIVER:
                        bap.nightClips = riverClips;
                        break;
                }
            }

            //set the clips
            bap.SetClips(); 

            //play all clips
            ambiencePoint.GetChild(0).GetComponent<AudioSource>().Play();
            ambiencePoint.GetChild(1).GetComponent<AudioSource>().Play();
            ambiencePoint.GetChild(2).GetComponent<AudioSource>().Play();
            ambiencePoint.GetChild(3).GetComponent<AudioSource>().Play();

            bap.ready = true;
        }
    }


}
