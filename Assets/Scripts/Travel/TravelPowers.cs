using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{
    public class TravelPowers : MonoBehaviour
    {
        [ReadOnly] public int fitnessLevel, flightLevel, speedLevel, jumpLevel;
        GameObject player;

        public enum TravelModes
        {
            IDLE,
            FLIGHT,
            SPEED,
            JUMP
        }

        public TravelModes travelMode;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("Player");
        }

        // Update is called once per frame
        void Update()
        {
            switch (travelMode)
            {
                case TravelModes.IDLE:
                    //player.GetComponent<SmartFPController.FirstPersonController>().enabled = true;
                    //player.GetComponent<SmartFPController.FirstPersonFlight>().enabled = false;

                    break;
                case TravelModes.FLIGHT:
                    //player.GetComponent<SmartFPController.FirstPersonController>().enabled = false;
                    //player.GetComponent<SmartFPController.FirstPersonFlight>().enabled = true;

                    break;
                case TravelModes.SPEED:

                    break;

                case TravelModes.JUMP:

                    break;
            }
        }

        public void FlightEnabled(bool enabled)
        {
            Debug.Log("FlightEnabled: " + enabled);
            if (enabled)
            {
                player.GetComponent<SmartFPController.FirstPersonController>().enabled = false;
                player.GetComponent<SmartFPController.FirstPersonFlight>().enabled = true;
            } else
            {
                player.GetComponent<SmartFPController.FirstPersonController>().enabled = true;
                player.GetComponent<SmartFPController.FirstPersonFlight>().enabled = false;
            }            
        }
    }

}