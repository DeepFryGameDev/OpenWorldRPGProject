using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{
    public class FlightProcessing : MonoBehaviour
    {
        [ReadOnly] public bool active, flyButtonDown;
        float defaultGravityMultiplier, defaultJumpForce;
        SmartFPController.FirstPersonController fpController;

        // Start is called before the first frame update
        void Start()
        {
            fpController = GameObject.Find("Player").GetComponent<SmartFPController.FirstPersonController>();
            defaultGravityMultiplier = fpController.gravityMultiplier;
            defaultJumpForce = fpController.jumpForce;
        }

        // Update is called once per frame
        void Update()
        {
            if (active)
                Flight();
        }

        void Flight()
        {
            //fpController.gravityMultiplier = 0.25f;
            if (Input.GetButton("Jump"))
            {
                flyButtonDown = true;
            } else
            {
                flyButtonDown = false;
            }
        }
    }
}

