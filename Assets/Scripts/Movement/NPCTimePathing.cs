using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DeepFry
{
    public class NPCTimePathing : NPCPathing
    {
        [Tooltip("12AM - 0\n1AM - 3600\n2AM - 7200\n3AM - 10800\n4AM - 14400\n5AM - 18000\n6AM - 21600\n7AM - 25200\n8AM - 28800\n9AM - 32400\n10AM - 36000\n11AM - 39600\n" +
        "12PM - 43200\n1PM - 46800\n2PM - 50400\n3PM - 54000\n4PM - 57600\n5PM - 61200\n6PM - 64800\n7PM - 68400\n8PM - 72000\n9PM - 75600\n10PM - 79200\n11PM - 82800\n-=-=-=-=-=-=-=-=-=-=-=-=-=-\n900 per 15 minute interval")]
        [ReadOnly] public int time;
        OneDaySystem OneDayController;

        [ReadOnly] public GameObject destination;
        bool runOnce;

        private string runUponArrival;
        private BaseScriptedEvent bse;

        private float waitTime;

        public List<BaseTimePathPoint> timePathPoints = new List<BaseTimePathPoint>();

        // Start is called before the first frame update
        void Start()
        {
            OneDayController = GameObject.Find("[WORLD]/OneDayController").GetComponent<OneDaySystem>();

            //establish pointbase
            pointBase = gameObject.transform.position;

            //start idle animation
            ChangeAnimationState(0);

            //Establish scripted events
            bse = gameObject.GetComponent(typeof(BaseScriptedEvent)) as BaseScriptedEvent;
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
                            CheckTime();
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
            SearchWalkPoint();

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

        private IEnumerator WaitForTime()
        {
            waiting = true;
            //wait for set time
            yield return new WaitForSeconds(waitTime);
            pathingPhase = PathingPhases.ISWALKING;
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

        void EndOfRoute()
        {
            if (runUponArrival.Length > 0)
            {
                Debug.Log("Run upon arrival: " + runUponArrival);
                bse.Invoke(runUponArrival, 0);
            }

            Debug.Log(gameObject.name + " - TimePathing - Reached end of walkpoint");

            waitTime = 0.0f;

            ChangeAnimationState(0);

            walkPointSet = false;
            waitTimeSet = false;
            routeSet = false;
            waiting = false;
            walking = false;

            pathingPhase = PathingPhases.PREROUTESET;
        }

        private void SearchWalkPoint()
        {
            walkpoint = destination.transform.position;

            Debug.Log(gameObject.name + " - TimePathing - Checking walkpoint: " + walkpoint);

            if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
            {
                Debug.Log(gameObject.name + " - TimePathing - Route set");
                routeSet = true;
            } else if (Physics.Raycast(walkpoint, transform.up, 2f, whatIsGround))
            {
                Debug.Log(gameObject.name + " - TimePathing - Route set");
                routeSet = true;
            } else
            {
                Debug.Log("TimePathing - " + gameObject.name + " - unable to locate walkpoint: " + walkpoint);
            }
        }

        void CheckTime()
        {
            time = OneDayController.time;

            foreach (BaseTimePathPoint btpp in timePathPoints)
            {
                if (btpp.Time - time <= 35 && btpp.Time - time > 0 && !runOnce)
                {
                    runOnce = true;
                    Debug.Log("Time triggered on NPCTimePathing: " + time + " on unit " + gameObject.name);

                    if (btpp.runBeforeDeparture.Length > 0)
                    {
                        Debug.Log("Run before departure: " + btpp.runBeforeDeparture);
                        bse.Invoke(btpp.runBeforeDeparture, 0);
                    }

                    destination = btpp.pathPoint.pathPoint;

                    if (btpp.runUponArrival.Length > 0)
                    {
                        runUponArrival = btpp.runUponArrival;
                    } else
                    {
                        runUponArrival = "";
                    }

                    waitTime = GameObject.Find("GameManager").GetComponent<GenerateRandom>().GenerateRandomNumber(btpp.pathPoint.minSecondsToWait, btpp.pathPoint.maxSecondsToWait);

                    pathingPhase = PathingPhases.SETTINGROUTE;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            foreach (BaseTimePathPoint btpp in timePathPoints)
            {
                if (btpp.pathPoint != null)
                    Gizmos.DrawWireCube(btpp.pathPoint.pathPoint.transform.position, new Vector3(3, 3, 3));
            }
        }
    }
}

