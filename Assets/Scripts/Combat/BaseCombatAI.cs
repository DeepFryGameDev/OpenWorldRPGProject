using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DeepFry
{
    public class BaseCombatAI : MonoBehaviour
    {
        NavMeshAgent agent;
        Transform player;

        public bool friendly;
        public int enemyID;
        public float moveSpeed = 1;
        [ReadOnly] public BaseEnemy enemy;

        public enum currentStates
        {
            IDLE,
            INCOMBAT
        }
        public currentStates currentState;

        //Attacking
        public float aa_timeBetweenAttacks;
        bool alreadyAttacked;
        public GameObject aa_projectile;
        public GameObject aa_shooterObj;

        //States
        public float sightRange, attackRange;
        [ReadOnly] public bool playerInSightRange, playerInAttackRange;

        LayerMask whatIsGround, whatIsPlayer;

        [HideInInspector] public bool isDead;
        bool timerStarted;
        [ReadOnly] public float respawnSeconds;
        [ReadOnly] public int encounterID;
        [HideInInspector] public SpawnZone spawnZone;

        private void Awake()
        {
            player = GameObject.Find("Player").transform;
            agent = GetComponent<NavMeshAgent>();

            whatIsGround = LayerMask.GetMask("WhatIsGround");
            whatIsPlayer = LayerMask.GetMask("WhatIsPlayer");

            if (!friendly)
                GenerateInventory();

        }

        private void Start()
        {
            SetEnemy();
        }


        private void Update()
        {
            if (!isDead)
            {
                switch (currentState)
                {
                    case currentStates.IDLE:

                        break;
                    case currentStates.INCOMBAT:
                        //Check for sight and attack range
                        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

                        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                        if (playerInSightRange && playerInAttackRange) AttackPlayer();
                        break;
                }                
            }
            else
            {
                if (gameObject.CompareTag("Enemy"))
                    ProcessRespawn();
            }
        }

        public void SetEnemy()
        {
            enemy = EnemyDB.instance.GetEnemy(enemyID).NewEnemy();
            Debug.Log("setting enemy to ID " + enemyID);
        }


        void GenerateInventory()
        {
            Debug.Log("-----Generating inventory for " + gameObject.name + "-----");

            int tempID = 0;
            int count = 0;

            foreach (BaseNPCInventoryItem bnii in EnemyDB.instance.GetEnemy(enemyID).items)
            {
                count++;
                if (!bnii.onlyOnQuest)
                {
                    string itemName;

                    if (!bnii.isEquip)
                    {
                        itemName = ItemDB.instance.GetItem(bnii.ID).item.name;
                    }
                    else
                    {
                        itemName = EquipmentDB.instance.GetEquip(bnii.ID).equipment.name;
                    }


                    tempID = bnii.ID;

                    float rand = GameObject.Find("GameManager").GetComponent<GenerateRandom>().GenerateRandomNumber(1, 100);

                    Debug.Log(count + ") " + "Loot roll for " + itemName + ": " + rand + " / Chance to drop: " + bnii.chance);

                    if (rand <= bnii.chance) //item dropped, add to inventory
                    {
                        Debug.Log(count + ") " + itemName + " - SUCCESS");

                        bool inInventory = false;

                        //check if item is already in inventory.  if so, increase quantity of item
                        foreach (BaseNPCItem bni in gameObject.GetComponent<NPCInventory>().inventory)
                        {
                            if (bni.itemID == bnii.ID && bni.isEquip == bnii.isEquip)
                            {
                                //Debug.Log(count + ") " + itemName + " already in inventory.  Increasing quantity of " + bni.quantity + " to " + (bni.quantity + 1));

                                bni.quantity += 1;
                                inInventory = true;
                                break;
                            }
                        }

                        //if not found, add just 1
                        if (!inInventory)
                        {
                            //Debug.Log(count + ") " + itemName + " not found in inventory.  Adding 1 of them");

                            gameObject.GetComponent<NPCInventory>().inventory.Add(GetBaseNPCItem(bnii.ID, bnii.isEquip, 1));
                        }
                    }
                    else
                    {
                        Debug.Log(count + ") " + itemName + " - FAIL");
                    }
                }
            }
        }

        void ProcessRespawn()
        {
            if (!timerStarted)
            {
                timerStarted = true;
                StartCoroutine(Respawn());
            }
        }

        IEnumerator Respawn()
        {
            Debug.Log("Respawning after " + respawnSeconds + " seconds..");
            yield return new WaitForSeconds(respawnSeconds);

            //Spawn another enemy
            BaseEnemy enemy = EnemyDB.instance.GetEnemy(enemyID).NewEnemy();
            spawnZone.SpawnEnemy(enemy, spawnZone.RandomPointInBounds(), encounterID);

            //Destroy object
            Destroy(gameObject);
        }

        BaseNPCItem GetBaseNPCItem(int itemID, bool isEquip, int quantity)
        {
            return new BaseNPCItem()
            {
                itemID = itemID,
                isEquip = isEquip,
                quantity = quantity
            };
        }

        private void AttackPlayer()
        {
            //Make sure enemy doesn't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                //Enemy behavior here
                //for tutorial:
                Rigidbody rb = Instantiate(aa_projectile, aa_shooterObj.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * 8f, ForceMode.Impulse);


                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), aa_timeBetweenAttacks);
            }
        }

        //For attack
        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        //For receiving damage
        public void TakeDamage(int damage)
        {
            enemy.curHP -= damage;

            Debug.Log(enemy.name + " HP: " + enemy.curHP + "/" + enemy.maxHP);

            if (enemy.curHP <= 0)
            {
                DeathEvents();

                Debug.Log("You killed the " + enemy.name);

                //scripts to run upon death
            }
        }

        private void DeathEvents()
        {
            //check if any items dropped require quest and add to inventory if possible
            GenerateQuestItems();
            GenerateQuestKills();

            isDead = true;

            gameObject.GetComponent<Animator>().SetBool("isDead", true);

            NPCPathing pathing = gameObject.GetComponent(typeof(NPCPathing)) as NPCPathing;
            if (pathing != null)
                pathing.agent.isStopped = true;
        }

        private void GenerateQuestKills()
        {
            Debug.Log("-----Checking for quest kills for " + gameObject.name + "-----");
            foreach (BaseQuest quest in QuestManager.instance.activeQuests)
            {
                if (quest.killReqs.Count > 0) //quest has kill requirements
                {
                    foreach (BaseQuestKillRequirement bqkr in quest.killReqs)
                    {
                        if (bqkr.enemyID == enemyID) //quest kill requirements contain this enemy
                        {
                            if (bqkr.targetsKilled < bqkr.quantity)
                            {
                                bqkr.targetsKilled++; //increase targets killed
                            }

                            if (bqkr.targetsKilled == bqkr.quantity)
                            {
                                SetDialogueSystemVariable("quest_" + quest.ID.ToString() + "_finished", true); //quest is completed
                            }
                        }
                    }
                }
            }
        }

        private void GenerateQuestItems()
        {
            Debug.Log("-----Checking for quest items for " + gameObject.name + "-----");

            int tempID = 0;
            int count = 0;

            foreach (BaseNPCInventoryItem bnii in EnemyDB.instance.GetEnemy(enemyID).items)
            {
                if (bnii.onlyOnQuest && IsQuestActive(bnii.questID))
                {
                    count++;
                    string itemName;

                    if (!bnii.isEquip)
                    {
                        itemName = ItemDB.instance.GetItem(bnii.ID).item.name;
                    }
                    else
                    {
                        itemName = EquipmentDB.instance.GetEquip(bnii.ID).equipment.name;
                    }

                    Debug.Log(count + ") " + itemName + " drops while on quest " + QuestDB.instance.quests[bnii.questID].quest.name + ".  Rolling..");


                    tempID = bnii.ID;

                    float rand = GameObject.Find("GameManager").GetComponent<GenerateRandom>().GenerateRandomNumber(1, 100);

                    Debug.Log(count + ") " + "Loot roll for " + itemName + ": " + rand + " / Chance to drop: " + bnii.chance);

                    if (rand <= bnii.chance) //item dropped, add to inventory
                    {
                        Debug.Log(count + ") " + itemName + " - SUCCESS");

                        bool inInventory = false;

                        //check if item is already in inventory.  if so, increase quantity of item
                        foreach (BaseNPCItem bni in gameObject.GetComponent<NPCInventory>().inventory)
                        {
                            if (bni.itemID == bnii.ID && bni.isEquip == bnii.isEquip)
                            {
                                //Debug.Log(count + ") " + itemName + " already in inventory.  Increasing quantity of " + bni.quantity + " to " + (bni.quantity + 1));

                                bni.quantity += 1;
                                inInventory = true;
                                break;
                            }
                        }

                        //if not found, add just 1
                        if (!inInventory)
                        {
                            //Debug.Log(count + ") " + itemName + " not found in inventory.  Adding 1 of them");

                            gameObject.GetComponent<NPCInventory>().inventory.Add(GetBaseNPCItem(bnii.ID, bnii.isEquip, 1));
                        }
                    }
                    else
                    {
                        Debug.Log(count + ") " + itemName + " - FAIL");
                    }
                }
            }

            if (count == 0)
            {
                Debug.Log("No quest items needed!");
            }
        }

        bool IsQuestActive(int questID)
        {
            foreach (BaseQuest quest in QuestManager.instance.activeQuests)
            {
                if (quest.ID == questID)
                {
                    return true;
                }
            }
            return false;
        }

        void SetDialogueSystemVariable(string variableName, bool value)
        {
            Debug.Log("Setting " + variableName + " to " + value);
            DialogueLua.SetVariable(variableName, value);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }

    }

}