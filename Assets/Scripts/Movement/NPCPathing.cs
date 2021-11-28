using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DeepFry
{
    public class NPCPathing : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Transform player;
        [HideInInspector] public BaseCombatAI bcai;
        [HideInInspector] public LayerMask whatIsGround, whatIsPlayer;

        protected Vector3 walkpoint;

        protected bool walkPointSet, waitTimeSet, routeSet, haltWaitingToResume;
        [ReadOnly] public bool walking, waiting, paused, halted;
        protected Vector3 pointBase;

        public bool canStopDuringPath;
        protected float haltDistance = 10, haltWaitBeforeResuming = 5;

        protected enum PathingPhases
        {
            PREROUTESET,
            SETTINGROUTE,
            ROUTESET,
            ISWALKING,
            ENDOFROUTE
        }

        protected PathingPhases pathingPhase;

        private void Awake()
        {
            player = GameObject.Find("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            whatIsGround = LayerMask.GetMask("WhatIsGround");
            whatIsPlayer = LayerMask.GetMask("WhatIsPlayer");
            bcai = gameObject.GetComponent<BaseCombatAI>();
        }

        public void LookAtWalkPoint()
        {
            // Determine which direction to rotate towards
            Vector3 targetDirection = walkpoint - transform.position;

            // The step size is equal to speed times frame time.
            int speed = 1; //change this
            float singleStep = speed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            //Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        /// <summary>
        /// Changes state of animation
        /// </summary>
        /// <param name="index">0 - Idle, 1 - Walking</param>
        public void ChangeAnimationState(int index)
        {
            Animator anim = gameObject.GetComponent<Animator>();

            switch (index)
            {
                case 0:
                    anim.SetBool("isWalking", false); //idle
                    break;
                case 1:
                    anim.SetBool("isWalking", true); //walking
                    break;
            }
        }
    }

}
