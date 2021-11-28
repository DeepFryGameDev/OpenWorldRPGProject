using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DeepFry
{
    public class NPCPointPathing : NPCPathing
    {
        public enum pathTypes
        {
            RANDOM,
            SEQUENTIAL
        }
        public pathTypes pathType;

        public List<BasePathPoint> pathPoints = new List<BasePathPoint>();

        int pointIndex;
        float waitTime;

        [ReadOnly] public GameObject destination;

        // Start is called before the first frame update
        void Start()
        {
            //establish pointbase
            pointBase = gameObject.transform.position;

            //start idle animation
            ChangeAnimationState(0);

            pointIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            switch (bcai.currentState)
            {
                case BaseCombatAI.currentStates.IDLE:
                    switch (pathingPhase)
                    {
                        case PathingPhases.PREROUTESET:
                            pathingPhase = PathingPhases.SETTINGROUTE;
                            break;

                        case PathingPhases.SETTINGROUTE:
                            SetRoute();
                            break;

                        case PathingPhases.ROUTESET:
                            WaitBeforePath();
                            break;

                        case PathingPhases.ISWALKING:
                            Walk();
                            break;

                        case PathingPhases.ENDOFROUTE:
                            EndOfRoute();
                            break;
                    }

                    break;
                case BaseCombatAI.currentStates.INCOMBAT:

                    break;
            }
        }

        void SetRoute()
        {
            BasePathPoint bpp = pathPoints[pointIndex];

            if (!waitTimeSet)
            {
                SetWaitTime(bpp);
                waitTimeSet = true;
            }

            //set route
            Debug.Log(gameObject.name + " - PointPathing - Setting route");
            destination = bpp.pathPoint;
            SearchWalkPoint(bpp);

            if (routeSet)
            {
                pathingPhase = PathingPhases.ROUTESET;
            }
        }

        void WaitBeforePath()
        {
            if (!waiting)
            {
                StartCoroutine(WaitForTime());
            }
        }

        void Walk()
        {
            if (!paused)
            {
                if (!walking)
                {
                    walking = true;
                    if (!gameObject.GetComponent<Animator>().GetBool("isWalking"))
                        ChangeAnimationState(1);

                    agent.SetDestination(walkpoint);
                }

                if (canStopDuringPath && Vector3.Distance(transform.position, player.position) <= haltDistance)
                {
                    Halt();
                }

                if (halted && Vector3.Distance(transform.position, player.position) > haltDistance)
                {
                    if (!haltWaitingToResume)
                    {
                        StartCoroutine(WaitToResumeAfterHalt());
                    }
                }

                if (!halted)
                {
                    //look forward (currently looking at walk point.. needs to be updated later)
                    LookAtWalkPoint();

                    //walk towards walkpoint
                    Vector3 distanceToWalkPoint = transform.position - walkpoint;

                    //Walkpoint reached
                    if (distanceToWalkPoint.magnitude < 1f)
                    {
                        pathingPhase = PathingPhases.ENDOFROUTE;
                    }
                }
            }            
        }

        IEnumerator WaitToResumeAfterHalt()
        {
            haltWaitingToResume = true;

            Debug.Log("Waiting " + haltWaitBeforeResuming + " seconds.");
            yield return new WaitForSeconds(haltWaitBeforeResuming);
            
            if (Vector3.Distance(transform.position, player.position) <= haltDistance)
            {
                Debug.Log("Still within halt distance - restarting wait");
                StartCoroutine(WaitToResumeAfterHalt());
            } else
            {
                Debug.Log("Can continue with path. continuing");
                HaltContinue();
            }
        }

        void HaltContinue()
        {
            halted = false;
            haltWaitingToResume = false;
            ChangeAnimationState(1);
            agent.isStopped = false;
        }

        void Halt()
        {
            halted = true;
            haltWaitingToResume = false;
            ChangeAnimationState(0);
            agent.isStopped = true;
            //look at player
            // Determine which direction to rotate towards
            Vector3 targetDirection = player.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = 2.0f * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            //Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        void EndOfRoute()
        {
            Debug.Log(gameObject.name + " - PointPathing - Reached end of walkpoint");

            waitTime = 0.0f;

            ChangeAnimationState(0);

            walkPointSet = false;
            waitTimeSet = false;
            routeSet = false;
            waiting = false;
            walking = false;

            //choose next point
            if (pathType == pathTypes.RANDOM)
            {

            }
            else if (pathType == pathTypes.SEQUENTIAL)
            {
                pointIndex++;
            }

            pathingPhase = PathingPhases.SETTINGROUTE;
        }

        private IEnumerator WaitForTime()
        {
            waiting = true;
            //wait for set time
            yield return new WaitForSeconds(waitTime);
            pathingPhase = PathingPhases.ISWALKING;
        }

        void SetWaitTime(BasePathPoint bpp)
        {
            waitTime = GameObject.Find("GameManager").GetComponent<GenerateRandom>().GenerateRandomNumber(bpp.minSecondsToWait, bpp.maxSecondsToWait);
        }

        private void SearchWalkPoint(BasePathPoint bpp)
        {
            walkpoint = bpp.pathPoint.transform.position;

            Debug.Log(gameObject.name + " - PointPathing - Checking walkpoint: " + walkpoint);

            if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
            {
                Debug.Log(gameObject.name + " - PointPathing - Route set");
                routeSet = true;
            }
            else if (Physics.Raycast(walkpoint, transform.up, 2f, whatIsGround))
            {
                Debug.Log(gameObject.name + " - PointPathing - Route set");
                routeSet = true;
            }
            else
            {
                Debug.Log("PointPathing - " + gameObject.name + " - unable to locate walkpoint: " + walkpoint);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            
            foreach (BasePathPoint pathPoint in pathPoints)
            {
                if (pathPoint.pathPoint != null)
                    Gizmos.DrawWireCube(pathPoint.pathPoint.transform.position, new Vector3(3, 3, 3));
            }
        }
    }
}