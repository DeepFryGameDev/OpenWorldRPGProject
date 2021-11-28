using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeepFry
{
    public class GameManager : MonoBehaviour
    {
        //-----------------------------------
        public static GameManager instance;

        //GOLD
        [ReadOnly] public int gold;

        //SPAWN POINTS
        [HideInInspector] public string nextSpawnPoint;

        //SCENES
        [ReadOnly] public string sceneToLoad; //to load on collisions
        [ReadOnly] public string lastScene; //to load after battle

        //TEXT INPUTS
        [ReadOnly] public string textInput;
        [ReadOnly] public int numberInput;
        [ReadOnly] public string nameInput;
        [ReadOnly] public bool capsOn = false;
        [HideInInspector] public bool letterButtonPressed = false;

        //TEMP OBJECTS FOR SHOPS
        //[HideInInspector] public List<BaseShopItem> itemShopList = new List<BaseShopItem>();
        [HideInInspector] public Item itemShopItem;
        [HideInInspector] public int itemShopCost;
        //[HideInInspector] public List<BaseShopEquipment> equipShopList = new List<BaseShopEquipment>();
        [HideInInspector] public Equipment equipShopItem;
        [HideInInspector] public int equipShopCost;
        [HideInInspector] public bool inConfirmation;

        //QUESTS
        [HideInInspector] public Sprite QuestReadyForPickupIcon;
        [ReadOnly] public bool questSelected;
        [ReadOnly] public int selectedQuest;

        //FROM SCRIPT STUFF
        [HideInInspector] public string battleSceneFromScript;

        //TIME TRACKING
        [HideInInspector] public int seconds;
        [HideInInspector] public int minutes;
        [HideInInspector] public int hours;

        //FOR DIALOGUE
        [HideInInspector] public InteractDetection interactDetection;
        [ReadOnly] public GameObject lastInteracted;

        //BESTIARY
        //[ReadOnly] public List<BaseBestiaryEntry> bestiaryEntries = new List<BaseBestiaryEntry>();

        [ReadOnly] public GameStates gameState;

        //[HideInInspector] public List<BaseBattleEnemy> enemiesToBattle = new List<BaseBattleEnemy>(); //for adding enemies in encounter to the battle
        [HideInInspector] public List<GameObject> heroesToBattle = new List<GameObject>();
        [HideInInspector] public int enemyAmount; //for how many enemies can be encountered in one battle
        [HideInInspector] public List<string> enemySpawnPoints = new List<string>();
        [HideInInspector] public int heroAmount;

        [HideInInspector] public bool startBattleFromScript; //if battle is being started from script

        [HideInInspector] public string battleSceneToLoad;

        private GameObject audioSource;

        public int[] expThresholds;

        [Tooltip("Delay for when there is a player response")]
        public float dialogueResponseDelay = 2f;

        [Tooltip("How much endurance is spent every frame while sprinting.")]
        [SerializeField, Range(1, 100)]
        public int SprintDrain;
        [Tooltip("The rate at which endurance is spent while sprinting.")]
        [SerializeField, Range(.1f, 10f)]
        public float SprintDrainRate;
        [Tooltip("How much power is spent every frame while flying.")]
        [SerializeField, Range(1, 100)]
        public int FlyDrain;
        [Tooltip("The rate at which power is spent while flying.")]
        [SerializeField, Range(.1f, 10f)]
        public float FlyDrainRate;

        //GAME VARIABLES
        [Tooltip("Used when shooting projectiles - x seconds before impact gameObject is destroyed.")]
        public float secondsForImpactVFXDelay;
        [Tooltip("Quest appears visible for pickup on the map and compass if player level is x levels above or below the quest level.")]
        public int questPickupAvailableRange;


        void Awake()
        {

            if (instance == null) //check if instance exists
            {
                instance = this; //if not set the instance to this
            }
            else if (instance != this) //if it exists but is not this instance
            {
                Destroy(gameObject); //destroy it
            }
            DontDestroyOnLoad(gameObject); //set this to be persistable across scenes

            //StartCoroutine(UpdateTime());

            audioSource = Resources.Load<GameObject>("PlaySE");
            QuestReadyForPickupIcon = Resources.Load<Sprite>("Sprites/QuestReady");

            interactDetection = GameObject.Find("Player/Camera").GetComponent<InteractDetection>();
        }

        private void Update()
        {

        }


        /// <summary>
        /// Loads scene set from sceneToLoad
        /// </summary>
        public void LoadScene()
        {
            //SceneManager.LoadScene(sceneToLoad); //loads scene from collisions
            GameObject.Find("SceneLoader").GetComponent<SceneLoader>().LoadScene(sceneToLoad); //loads scene from collisions
        }

        /// <summary>
        /// Adds exp for all active heroes and levels them up if needed
        /// </summary>
        public void ProcessExp()
        {
            /*foreach (BaseHero hero in activeHeroes)
            {
                Debug.Log("currentExp: " + hero.currentExp + ", currentLevel: " + hero.currentLevel);
                while(receivedAllExp == false)
                {
                    if (hero.currentExp >= HeroDB.instance.levelEXPThresholds[(hero.currentLevel - 1)])
                    {
                        hero.levelBeforeExp = hero.currentLevel; //wrong place for this, will update later
                        hero.LevelUp();
                    } else
                    {
                        receivedAllExp = true;
                    }
                }
                receivedAllExp = false;
            }*/
        }

        /// <summary>
        /// Consistently keeps game time updated
        /// </summary>
        IEnumerator UpdateTime()
        {
            while (hours != 99 && minutes != 60 && seconds != 60)
            {
                yield return new WaitForSecondsRealtime(1);
                seconds++;

                if (seconds == 60)
                {
                    minutes++;
                    seconds = 0;
                }

                if (minutes == 60)
                {
                    hours++;
                    minutes = 0;
                }
            }
        }

        public void PlaySE(Vector3 location, AudioClip SE)
        {
            StartCoroutine(ProcessPlaySE(location, SE));
        }

        private IEnumerator ProcessPlaySE(Vector3 location, AudioClip SE)
        {
            GameObject SEobj = Instantiate(audioSource, location, Quaternion.identity);

            SEobj.GetComponent<AudioSource>().PlayOneShot(SE);

            yield return new WaitForSeconds(SE.length);

            Destroy(SEobj);
        }

    }
}
