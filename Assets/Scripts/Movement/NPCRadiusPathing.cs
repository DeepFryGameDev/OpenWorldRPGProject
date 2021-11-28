using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DeepFry
{
    public class NPCRadiusPathing : NPCPathing
    {
        public float walkPointRange = 25;
        public float minTimeBetweenMovement = 1, maxTimeBetweenMovement = 3;
        float waitTime;
        bool forGizmos;

        // Start is called before the first frame update
        void Start()
        {
            //establish pointbase
            pointBase = gameObject.transform.position;

            //start idle animation
            ChangeAnimationState(0);

            forGizmos = true; //for debugging purposes
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
            if (!waitTimeSet)
            {
                SetWaitTime();
                waitTimeSet = true;
            }

            //set route
            Debug.Log(gameObject.name + " - RadiusPathing - Setting route");
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

                LookAtWalkPoint(); //look forward (currently looking at walk point.. needs to be updated later)

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
            Debug.Log(gameObject.name + " - RadiusPathing - Reached end of walkpoint");

            waitTime = 0.0f;

            ChangeAnimationState(0);

            walkPointSet = false;
            waitTimeSet = false;
            routeSet = false;
            waiting = false;
            walking = false;

            pathingPhase = PathingPhases.SETTINGROUTE;
        }

        private IEnumerator WaitForTime()
        {
            waiting = true;
            //wait for set time
            yield return new WaitForSeconds(waitTime);
            pathingPhase = PathingPhases.ISWALKING;
        }

        void SetWaitTime()
        {
            waitTime = GameObject.Find("GameManager").GetComponent<GenerateRandom>().GenerateRandomNumber(minTimeBetweenMovement, maxTimeBetweenMovement);
        }

        private void SearchWalkPoint()
        {
            //Calculate random point in range
            float randomZ = GameObject.Find("GameManager").GetComponent<GenerateRandom>().GenerateRandomNumber(-walkPointRange, walkPointRange);
            float randomX = GameObject.Find("GameManager").GetComponent<GenerateRandom>().GenerateRandomNumber(-walkPointRange, walkPointRange);

            //walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            walkpoint = new Vector3(pointBase.x + randomX, pointBase.y, pointBase.z + randomZ);

            Debug.Log(gameObject.name + " - RadiusPathing - Checking walkpoint: " + walkpoint);

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
                SearchWalkPoint();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            if (!forGizmos)
            {
                Gizmos.DrawWireSphere(transform.position, walkPointRange);
            }
            else
            {
                Gizmos.DrawWireSphere(pointBase, walkPointRange);
            }
        }
    }
}

/*
private void Patrolling()
{
    if (!walkPointSet) SearchWalkPoint();

    if (walkPointSet)
        agent.SetDestination(walkpoint);

    LookAtWalkPoint();

    Vector3 distanceToWalkPoint = transform.position - walkpoint;

    //Walkpoint reached
    if (distanceToWalkPoint.magnitude < 1f)
        walkPointSet = false;
}*/