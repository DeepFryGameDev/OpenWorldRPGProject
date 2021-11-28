using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DeepFry
{

    public class GameMenu : MonoBehaviour
    {
        private bool drawingGUI;
        private bool menuDrawn;
        private BaseHero playerHero;
        private PlayerInventory playerInventory;

        private enum menuStates
        {
            MAIN,
            INVENTORY,
            POWERS,
            JOURNAL,
            MAP,
            IDLE
        }

        [ReadOnly] menuStates menuState;

        int buttonClicked;

        [ReadOnly] public bool inMenu;
        [ReadOnly] public bool inNpcInventory;

        private GameObject playerPrefab;
        private GameObject mainMenuCanvas;
        private GameObject inventoryMenuCanvas;
        private GameObject powersMenuCanvas;
        private GameObject journalMenuCanvas;
        private GameObject mapMenuCanvas;

        private GameObject navigationCanvas;
        Color powerButtonColor, inventoryButtonColor, journalButtonColor, mapButtonColor, mainButtonColor;

        private GameObject inventoryListItemPrefab, equipListItemPrefab, equipDescPanel;
        private PlayerInventory inventory;
        GameObject newItemSlot;
        GameObject inventoryList;
        [HideInInspector] public GameObject inventoryInfoPanel, itemTypePanel;
        [HideInInspector] public int selectedItemTypeIndex;

        private GameObject powerListPowerPrefab;
        GameObject newPowerSlot;
        GameObject powerList;
        [HideInInspector] public GameObject powerInfoPanel;

        private GameObject questListPrefab;
        private GameObject newQuestSlot;
        private GameObject questList;

        [HideInInspector] public GameObject questInfoPanel;
        [HideInInspector] public GameObject questButtonSelected;
        [HideInInspector] public Text QUESTDESCTEXT;
        [HideInInspector] public Text QUESTOBJTEXT;
        [HideInInspector] public Text QUESTLEVELTEXT;
        [HideInInspector] public Text REWARDEXPTEXT;
        [HideInInspector] public Text REWARDGOLDTEXT;
        [HideInInspector] public Text REWARDITEMTEXT;
        [HideInInspector] public Image REWARDITEMICON;

        private List<AttackDB.powerClasses> powerClasses = new List<AttackDB.powerClasses>();

        private TextMeshProUGUI powerSetNameText;
        private GameObject leftPowerScrollButton;
        private GameObject rightPowerScrollButton;

        private MapMinimap.MapManager mm;
        private bool inMap;

        [HideInInspector] public GameObject paramCanvas;
        [HideInInspector] public GameObject compassCanvas;

        [HideInInspector] public GameObject crosshairCG;

        UnityAction powerMenuAction, inventoryMenuAction, journalMenuAction, mapMenuAction, mainMenuAction;
        int oldMenu;

        Animator anim_button0, anim_button1, anim_button2, anim_button3;
        Animator anim_mainMenu, anim_powerMenu, anim_journalMenu, anim_inventoryMenu;

        //------------------

        [ReadOnly] public int selectedClassIndex;
        [ReadOnly] public bool questSelected;
        [ReadOnly] public int questSelectedID;

        public string mainMenuLabel, inventoryMenuLabel, powersMenuLabel, journalMenuLabel, mapMenuLabel;

        public float scrollSpeed;

        // Start is called before the first frame update
        void Start()
        {
            playerPrefab = GameObject.FindGameObjectWithTag("Player");

            playerHero = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerManager>().playersCharacter;
            playerInventory = GameObject.Find("PlayerManager").GetComponent<PlayerInventory>();

            paramCanvas = GameObject.Find("GameManager/HUD/Parameters");
            compassCanvas = GameObject.Find("GameManager/Compass");

            inventory = GameObject.Find("PlayerManager").GetComponent<PlayerInventory>();
            inventoryList = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/InventoryPanel/Scroll View/Viewport/InventoryList");
            inventoryInfoPanel = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/InfoPanel");
            itemTypePanel = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/ItemTypePanel");
            inventoryListItemPrefab = Resources.Load<GameObject>("Prefabs/UI/InventoryMenuItemSlot");
            equipListItemPrefab = Resources.Load<GameObject>("Prefabs/UI/EquipMenuItemSlot");
            equipDescPanel = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/InfoPanel/EquipDesc/Description");

            powerListPowerPrefab = Resources.Load<GameObject>("Prefabs/UI/PowerMenuPowerSlot");
            powerList = GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/PowersListPanel/PowersScroller/Viewport/PowerList");
            powerInfoPanel = GameObject.Find("GameManager/Menus/PowerMenuCanvas/MenuPanel/PowersDescPanel");

            questList = GameObject.Find("GameManager/Menus/JournalMenuCanvas/MenuPanel/QuestListPanel/QuestScroller/Viewport/QuestList");
            questInfoPanel = GameObject.Find("GameManager/Menus/JournalMenuCanvas/MenuPanel/QuestDescPanel");
            questListPrefab = Resources.Load<GameObject>("Prefabs/UI/QuestMenuSlot");

            mainMenuCanvas = GameObject.Find("GameManager/Menus/MainMenuCanvas");
            inventoryMenuCanvas = GameObject.Find("GameManager/Menus/InventoryMenuCanvas");
            powersMenuCanvas = GameObject.Find("GameManager/Menus/PowersMenuCanvas");
            journalMenuCanvas = GameObject.Find("GameManager/Menus/JournalMenuCanvas");
            mapMenuCanvas = GameObject.Find("GameManager/Menus/MapMenuCanvas");
            navigationCanvas = GameObject.Find("GameManager/Menus/NavigationButtons");

            powerButtonColor = new Color(navigationCanvas.transform.Find("PowersButton").GetComponent<Image>().color.r,
            navigationCanvas.transform.Find("PowersButton").GetComponent<Image>().color.g,
            navigationCanvas.transform.Find("PowersButton").GetComponent<Image>().color.b);

            inventoryButtonColor = new Color(navigationCanvas.transform.Find("InventoryButton").GetComponent<Image>().color.r,
            navigationCanvas.transform.Find("InventoryButton").GetComponent<Image>().color.g,
            navigationCanvas.transform.Find("InventoryButton").GetComponent<Image>().color.b);

            journalButtonColor = new Color(navigationCanvas.transform.Find("JournalButton").GetComponent<Image>().color.r,
            navigationCanvas.transform.Find("JournalButton").GetComponent<Image>().color.g,
            navigationCanvas.transform.Find("JournalButton").GetComponent<Image>().color.b);

            mapButtonColor = new Color(navigationCanvas.transform.Find("MapButton").GetComponent<Image>().color.r,
            navigationCanvas.transform.Find("MapButton").GetComponent<Image>().color.g,
            navigationCanvas.transform.Find("MapButton").GetComponent<Image>().color.b);

            mainButtonColor = Color.red;

            QUESTDESCTEXT = GameObject.Find("GameManager/Menus/JournalMenuCanvas/" +
            "MenuPanel/QuestDescPanel/QuestDescText").GetComponent<Text>();

            QUESTLEVELTEXT = GameObject.Find("GameManager/Menus/JournalMenuCanvas/" +
            "MenuPanel/QuestDescPanel/QuestLevelText").GetComponent<Text>();

            QUESTOBJTEXT = GameObject.Find("GameManager/Menus/JournalMenuCanvas/" +
            "MenuPanel/QuestDescPanel/QuestObjectivesText").GetComponent<Text>();

            REWARDEXPTEXT = GameObject.Find("GameManager/Menus/JournalMenuCanvas/" +
            "MenuPanel/QuestRewardsPanel/ExpText").GetComponent<Text>();

            REWARDGOLDTEXT = GameObject.Find("GameManager/Menus/JournalMenuCanvas/" +
            "MenuPanel/QuestRewardsPanel/GoldText").GetComponent<Text>();

            REWARDITEMICON = GameObject.Find("GameManager/Menus/JournalMenuCanvas/" +
            "MenuPanel/QuestRewardsPanel/ItemIcon").GetComponent<Image>();

            REWARDITEMTEXT = GameObject.Find("GameManager/Menus/JournalMenuCanvas/" +
            "MenuPanel/QuestRewardsPanel/ItemText").GetComponent<Text>();

            leftPowerScrollButton = GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/PreviousPowerButton");
            rightPowerScrollButton = GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/NextPowerButton");

            powerSetNameText = GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/PowerSetNameText").GetComponent<TextMeshProUGUI>();

            mm = GameObject.Find("[MAP]/MapManager/Manager").GetComponent<MapMinimap.MapManager>();

            crosshairCG = GameObject.Find("GameManager/CrosshairCanvas");

            //set animators
            anim_button0 = navigationCanvas.transform.GetChild(0).GetComponent<Animator>();
            anim_button1 = navigationCanvas.transform.GetChild(1).GetComponent<Animator>();
            anim_button2 = navigationCanvas.transform.GetChild(2).GetComponent<Animator>();
            anim_button3 = navigationCanvas.transform.GetChild(3).GetComponent<Animator>();

            anim_mainMenu = GameObject.Find("GameManager/Menus/MainMenuCanvas/MenuPanel").GetComponent<Animator>();
            anim_powerMenu = GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel").GetComponent<Animator>();
            anim_journalMenu = GameObject.Find("GameManager/Menus/JournalMenuCanvas/MenuPanel").GetComponent<Animator>();
            anim_inventoryMenu = GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel").GetComponent<Animator>();

            mainMenuAction = () => StartCoroutine(DrawMainMenu());
            powerMenuAction = () => StartCoroutine(DrawPowersMenu());
            inventoryMenuAction = () => StartCoroutine(DrawInventoryMenu());
            journalMenuAction = () => StartCoroutine(DrawJournalMenu());
            mapMenuAction = () => StartCoroutine(DrawMapMenu());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Menu") && !inMenu && !inNpcInventory)
            {
                StartCoroutine(OpenMenu());
            }

            if (Input.GetButtonDown("Cancel") && inMenu && inMap)
            {
                inMap = false;
                mm.CloseMap();
            }

            if (Input.GetButtonDown("Cancel") && inMenu && !inMap)
            {
                Debug.Log("exiting menu");

                StartCoroutine(ExitMenu());
            }

            if (menuState == menuStates.IDLE)
            {
                //hide stuff
            }
        }

        IEnumerator OpenMenu()
        {
            navigationCanvas.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            navigationCanvas.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            navigationCanvas.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            navigationCanvas.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();

            //set nav buttons for first time use
            navigationCanvas.transform.GetChild(0).gameObject.name = "PowersButton";
            navigationCanvas.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(powerMenuAction);
            navigationCanvas.transform.GetChild(0).gameObject.GetComponent<Image>().color = powerButtonColor;
            navigationCanvas.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = powersMenuLabel;

            navigationCanvas.transform.GetChild(1).gameObject.name = "JournalButton";
            navigationCanvas.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(journalMenuAction);
            navigationCanvas.transform.GetChild(1).gameObject.GetComponent<Image>().color = journalButtonColor;
            navigationCanvas.transform.GetChild(1).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = journalMenuLabel;

            navigationCanvas.transform.GetChild(2).gameObject.name = "MapButton";
            navigationCanvas.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(mapMenuAction);
            navigationCanvas.transform.GetChild(2).gameObject.GetComponent<Image>().color = mapButtonColor;
            navigationCanvas.transform.GetChild(2).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = mapMenuLabel;

            navigationCanvas.transform.GetChild(3).gameObject.name = "InventoryButton";
            navigationCanvas.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(inventoryMenuAction);
            navigationCanvas.transform.GetChild(3).gameObject.GetComponent<Image>().color = inventoryButtonColor;
            navigationCanvas.transform.GetChild(3).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = inventoryMenuLabel;

            inventoryList.transform.localPosition = new Vector3(inventoryList.transform.localPosition.x, 0, inventoryList.transform.localPosition.z);
            powerList.transform.localPosition = new Vector3(powerList.transform.localPosition.x, 0, powerList.transform.localPosition.z);
            questList.transform.localPosition = new Vector3(questList.transform.localPosition.x, 0, questList.transform.localPosition.z);

            oldMenu = 0;

            drawingGUI = true;
            inMenu = true;

            CursorActive(true);

            menuDrawn = false;

            menuState = menuStates.MAIN;

            ShowCanvas(crosshairCG, false);

            StartCoroutine(RunAnimateButtons(true));
            yield return StartCoroutine(AnimateMenus(true));

            //playerPrefab.GetComponent<SmartFPController.SmartInputManager>().Pause();
        }

        IEnumerator ExitMenu()
        {
            StartCoroutine(AnimateMenus(false));
            yield return StartCoroutine(RunAnimateButtons(false));

            ShowCanvas(navigationCanvas, false);

            inMenu = false;
            drawingGUI = false;

            CursorActive(false);

            ShowCanvas(crosshairCG, true);

            if (menuState == menuStates.MAIN)
            {
                ShowCanvas(mainMenuCanvas, false);
            }
            else if (menuState == menuStates.INVENTORY)
            {
                ShowCanvas(inventoryMenuCanvas, false);
            }
            else if (menuState == menuStates.POWERS)
            {
                ShowCanvas(powersMenuCanvas, false);
            }
            else if (menuState == menuStates.JOURNAL)
            {
                ShowCanvas(journalMenuCanvas, false);
            }
            else if (menuState == menuStates.MAP)
            {
                ShowCanvas(mapMenuCanvas, false);
            }

            ShowCanvas(paramCanvas, true);
            ShowCanvas(compassCanvas, true);

            menuState = menuStates.IDLE;
        }

        private void OnGUI()
        {
            if (drawingGUI && !menuDrawn)
            {
                if (menuState == menuStates.IDLE)
                {

                }
                else if (menuState == menuStates.MAIN)
                {
                    ShowCanvas(paramCanvas, false);
                    ShowCanvas(compassCanvas, false);

                    ShowCanvas(powersMenuCanvas, false);
                    ShowCanvas(journalMenuCanvas, false);
                    ShowCanvas(mapMenuCanvas, false);
                    mm.CloseMap();
                    ShowCanvas(inventoryMenuCanvas, false);

                    ShowCanvas(mainMenuCanvas, true);

                    ShowCanvas(navigationCanvas, true);

                    UpdateMainMenuUI();
                }
                else if (menuState == menuStates.INVENTORY)
                {
                    ShowCanvas(mainMenuCanvas, false);
                    ShowCanvas(powersMenuCanvas, false);
                    ShowCanvas(journalMenuCanvas, false);
                    ShowCanvas(mapMenuCanvas, false);
                    mm.CloseMap();
                    ShowCanvas(inventoryMenuCanvas, true);

                    ShowCanvas(navigationCanvas, true);
                }
                else if (menuState == menuStates.POWERS)
                {
                    ShowCanvas(mainMenuCanvas, false);
                    ShowCanvas(powersMenuCanvas, true);
                    ShowCanvas(journalMenuCanvas, false);
                    ShowCanvas(mapMenuCanvas, false);
                    mm.CloseMap();
                    ShowCanvas(inventoryMenuCanvas, false);

                    ShowCanvas(navigationCanvas, true);
                }
                else if (menuState == menuStates.JOURNAL)
                {
                    ShowCanvas(mainMenuCanvas, false);
                    ShowCanvas(powersMenuCanvas, false);
                    ShowCanvas(journalMenuCanvas, true);
                    ShowCanvas(mapMenuCanvas, false);
                    mm.CloseMap();
                    ShowCanvas(inventoryMenuCanvas, false);

                    ShowCanvas(navigationCanvas, true);
                }
                else if (menuState == menuStates.MAP)
                {
                    ShowCanvas(mainMenuCanvas, false);
                    ShowCanvas(powersMenuCanvas, false);
                    ShowCanvas(journalMenuCanvas, false);
                    ShowCanvas(mapMenuCanvas, true);
                    ShowCanvas(inventoryMenuCanvas, false);

                    ShowCanvas(navigationCanvas, true);
                }
            }
        }

        #region NAVIGATION BUTTONS

        public void SetButtonClicked(int index)
        {
            buttonClicked = index;
        }

        GameObject GetNavButton(int index)
        {
            return navigationCanvas.transform.GetChild(index).gameObject;
        }

        void UpdateNavigationButtons()
        {
            GameObject objToModify = GetNavButton(buttonClicked);

            string objectName = "", menuLabel = "";
            UnityAction newAction = null;
            Color colorToUse = new Color(0, 0, 0);


            switch (oldMenu)
            {
                case 0:
                    Debug.Log("oldMenu was statsButton");
                    objectName = "StatsButton";
                    menuLabel = mainMenuLabel;
                    newAction = mainMenuAction;
                    colorToUse = mainButtonColor;
                    break;
                case 1:
                    Debug.Log("oldMenu was inventoryButton");
                    objectName = "InventoryButton";
                    menuLabel = inventoryMenuLabel;
                    newAction = inventoryMenuAction;
                    colorToUse = inventoryButtonColor;
                    break;
                case 2:
                    Debug.Log("oldMenu was powerButton");
                    objectName = "PowersButton";
                    menuLabel = powersMenuLabel;
                    newAction = powerMenuAction;
                    colorToUse = powerButtonColor;
                    break;
                case 3:
                    Debug.Log("oldMenu was journalButton");
                    objectName = "JournalButton";
                    menuLabel = journalMenuLabel;
                    newAction = journalMenuAction;
                    colorToUse = journalButtonColor;
                    break;
                case 4:
                    Debug.Log("oldMenu was mapButton");
                    objectName = "MapButton";
                    menuLabel = mapMenuLabel;
                    newAction = mapMenuAction;
                    colorToUse = mapButtonColor;
                    break;
            }

            objToModify.gameObject.name = objectName;
            objToModify.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = menuLabel;
            objToModify.gameObject.GetComponent<Image>().color = colorToUse;

            objToModify.GetComponent<Button>().onClick.RemoveAllListeners();

            objToModify.GetComponent<Button>().onClick.AddListener(newAction);

        }

        IEnumerator RunAnimateButtons(bool run)
        {
            yield return AnimateButtons(run);
        }

        IEnumerator AnimateButtons(bool open)
        {
            PlayNavigationSE();

            anim_button0.SetBool("run", open);
            anim_button1.SetBool("run", open);
            anim_button2.SetBool("run", open);
            anim_button3.SetBool("run", open);
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator AnimateMenus(bool open)
        {
            if (!open)
            {
                GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/InventoryPanel/Scroll View").GetComponent<ScrollRect>().enabled = false;
                GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/PowersListPanel/PowersScroller").GetComponent<ScrollRect>().enabled = false;
                GameObject.Find("GameManager/Menus/JournalMenuCanvas/MenuPanel/QuestListPanel/QuestScroller").GetComponent<ScrollRect>().enabled = false;
            }

            anim_mainMenu.SetBool("open", open);
            anim_inventoryMenu.SetBool("open", open);
            anim_powerMenu.SetBool("open", open);
            anim_journalMenu.SetBool("open", open);
            anim_mainMenu.SetBool("open", open);
            yield return new WaitForSeconds(.5f);
        }

        void SetAnimBool(Animator anim, bool open)
        {
            anim.SetBool("open", open);
        }

        void PlayNavigationSE()
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/OpenMenu"));
        }
        #endregion

        #region MAIN MENU

        public IEnumerator DrawMainMenu()
        {
            StartCoroutine(AnimateMenus(false));
            yield return StartCoroutine(AnimateButtons(false));

            UpdateNavigationButtons();

            oldMenu = 0;
            Debug.Log("Drawing main menu");

            menuState = menuStates.MAIN;

            SetAnimBool(anim_mainMenu, true);
            yield return StartCoroutine(AnimateButtons(true));

            //UpdateNavigationButtons(buttonClicked);
        }

        private void UpdateMainMenuUI()
        {
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/StatsPanel/StatsGrid/MgtEntry").gameObject, playerHero.finalMGT);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/StatsPanel/StatsGrid/DefEntry").gameObject, playerHero.finalDEF);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/StatsPanel/StatsGrid/PowEntry").gameObject, playerHero.finalPOW);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/StatsPanel/StatsGrid/PDefEntry").gameObject, playerHero.finalPDEF);

            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/StrEntry").gameObject, playerHero.finalSTR);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/StaEntry").gameObject, playerHero.finalSTA);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/AgiEntry").gameObject, playerHero.finalAGI);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/ChaEntry").gameObject, playerHero.finalCHA);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/IntEntry").gameObject, playerHero.finalINT);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/EndEntry").gameObject, playerHero.finalEND);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/ConEntry").gameObject, playerHero.finalCON);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/LukEntry").gameObject, playerHero.finalLUK);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/WisEntry").gameObject, playerHero.finalWIS);
            UpdatePrimaryStatUI(mainMenuCanvas.transform.Find("MenuPanel/SecStatsPanel/StatsGrid/PerEntry").gameObject, playerHero.finalPER);

            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/OffStatsPanel/StatsGrid/1H").gameObject, playerHero.oneHandExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/OffStatsPanel/StatsGrid/2H").gameObject, playerHero.twoHandExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/OffStatsPanel/StatsGrid/Explosives").gameObject, playerHero.explosivesExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/OffStatsPanel/StatsGrid/Ranged").gameObject, playerHero.rangedExp);

            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/DefStatsPanel/StatsGrid/Resilience").gameObject, playerHero.resilienceExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/DefStatsPanel/StatsGrid/Fitness").gameObject, playerHero.fitnessExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/DefStatsPanel/StatsGrid/Willpower").gameObject, playerHero.willpowerExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/DefStatsPanel/StatsGrid/Lifesteal").gameObject, playerHero.lifestealExp);

            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Fire").gameObject, playerHero.fireExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Frost").gameObject, playerHero.frostExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Thunder").gameObject, playerHero.thunderExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Water").gameObject, playerHero.waterExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Nature").gameObject, playerHero.natureExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Earth").gameObject, playerHero.earthExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Wind").gameObject, playerHero.windExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Mind").gameObject, playerHero.mindExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Light").gameObject, playerHero.lightExp);
            UpdateStatUI(mainMenuCanvas.transform.Find("MenuPanel/EleStatsPanel/StatsGrid/Dark").gameObject, playerHero.darkExp);


            mainMenuCanvas.transform.Find("MenuPanel/ParamsPanel/HPText").GetComponent<TextMeshProUGUI>().text =
                playerHero.curHP.ToString() + " / " + playerHero.finalMaxHP.ToString();
            mainMenuCanvas.transform.Find("MenuPanel/ParamsPanel/HPBarBG/HPBar").transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesHP(), 0, 1), 1f);

            mainMenuCanvas.transform.Find("MenuPanel/ParamsPanel/MPText").GetComponent<TextMeshProUGUI>().text =
                playerHero.curEP.ToString() + " / " + playerHero.finalMaxEP.ToString();
            mainMenuCanvas.transform.Find("MenuPanel/ParamsPanel/MPBarBG/MPBar").transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesMP(), 0, 1), 1f);
        }

        private void UpdateStatUI(GameObject statsField, int expVal)
        {
            int levelVal = GetParamLevel(expVal);

            statsField.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = levelVal.ToString();

            AddIfSingleDigit(statsField.transform.Find("Text").GetComponent<TextMeshProUGUI>());

            statsField.transform.Find("ExpBarBG/Image").transform.localScale = new Vector2(Mathf.Clamp(GetProgressBarValuesEXP(expVal, levelVal), 0, 1), 1f);
        }

        private void UpdatePrimaryStatUI(GameObject statsField, int finalVal)
        {
            statsField.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = finalVal.ToString();

            AddIfSingleDigit(statsField.transform.Find("Text").GetComponent<TextMeshProUGUI>());
        }

        private int GetParamLevel(int expVal)
        {
            int[] thresholds = GameManager.instance.expThresholds;

            int tempLevel = 0;

            foreach (int threshold in thresholds)
            {
                if (expVal >= threshold)
                {
                    tempLevel++;
                }
            }

            return tempLevel + 1;
        }

        #endregion

        #region INVENTORY MENU

        IEnumerator DrawInventoryMenu()
        {
            StartCoroutine(AnimateMenus(false));
            yield return StartCoroutine(AnimateButtons(false));

            UpdateNavigationButtons();

            oldMenu = 1;

            menuState = menuStates.INVENTORY;

            SetAnimBool(anim_inventoryMenu, true);
            yield return StartCoroutine(AnimateButtons(true));

            GameObject.Find("GameManager/Menus/InventoryMenuCanvas/MenuPanel/InventoryPanel/Scroll View").GetComponent<ScrollRect>().enabled = true;

            DrawInventoryItems();
        }

        public void DrawInventoryItems()
        {
            //show All items when opening player inventory
            DrawItems(1);
        }

        public void DrawItems(int index)
        {
            //set new index
            selectedItemTypeIndex = index;

            //highlight button
            HighlightButton(index);

            //erase old list
            //clears list
            foreach (Transform child in inventoryList.transform)
            {
                Destroy(child.gameObject);
            }

            //generate new list
            switch (index)
            {
                case 0:
                    DrawFavorites();
                    break;
                case 1:
                    DrawAllItems();
                    break;
                case 2:
                    DrawWeapons();
                    break;
                case 3:
                    DrawArmor();
                    break;
                case 4:
                    DrawPotions();
                    break;
                case 5:
                    DrawFood();
                    break;
                case 6:
                    DrawScrolls();
                    break;
                case 7:
                    DrawBooks();
                    break;
                case 8:
                    DrawIngredients();
                    break;
                case 9:
                    DrawMiscQuests();
                    break;
            }

        }

        void DrawAllItems()
        {
            //always show gold first
            Item goldItem = ItemDB.instance.GetItem(0).item;

            if (playerInventory.gold > 0)
            {
                newItemSlot = Instantiate(inventoryListItemPrefab);
                newItemSlot.name = "0) " + goldItem.name;
                newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = goldItem.icon;
                newItemSlot.transform.Find("Name").GetComponent<Text>().text = goldItem.name;
                newItemSlot.transform.Find("Count").GetComponent<Text>().text = GameManager.instance.gold.ToString();

                newItemSlot.GetComponent<ItemSlotInteraction>().ID = 0;
                newItemSlot.GetComponent<ItemSlotInteraction>().item = goldItem;

                newItemSlot.transform.SetParent(inventoryList.transform, false);
            }

            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0) //if item and not gold
                {
                    Item item = ItemDB.instance.GetItem(inventory.inventory[i].itemID).item;
                    if (!itemsAccountedFor.Contains(item))
                    {
                        int count = 0;
                        foreach (BasePlayerItem bpi in inventory.inventory)
                        {
                            if (bpi.itemID == inventory.inventory[i].itemID && !bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        newItemSlot = Instantiate(inventoryListItemPrefab);

                        newItemSlot.name = inventory.inventory[i].itemID + ") " + item.name;
                        newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("Name").GetComponent<Text>().text = item.name;
                        newItemSlot.transform.Find("Count").GetComponent<Text>().text = count.ToString();

                        newItemSlot.GetComponent<ItemSlotInteraction>().ID = inventory.inventory[i].itemID;
                        newItemSlot.GetComponent<ItemSlotInteraction>().item = item;
                    }

                    itemsAccountedFor.Add(item);
                }
                else //if equip
                {
                    Equipment equip = EquipmentDB.instance.GetEquip(inventory.inventory[i].itemID).equipment;

                    newItemSlot = Instantiate(equipListItemPrefab);
                    newItemSlot.name = inventory.inventory[i].itemID + ") " + equip.name;
                    newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = equip.icon;
                    newItemSlot.transform.Find("Name").GetComponent<Text>().text = equip.name;
                    newItemSlot.transform.Find("Count").GetComponent<Text>().text = "";

                    newItemSlot.GetComponent<EquipSlotInteraction>().ID = inventory.inventory[i].itemID;
                    newItemSlot.GetComponent<EquipSlotInteraction>().equip = equip;

                    itemsAccountedFor.Add(equip);
                }

                newItemSlot.transform.SetParent(inventoryList.transform, false);
            }
        }

        void DrawFavorites()
        {
            bool anyFavorites = false;

            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0 && ItemDB.instance.GetItem(inventory.inventory[i].itemID).item.isFavorite) //if item and not gold
                {
                    anyFavorites = true;
                    Item item = ItemDB.instance.GetItem(inventory.inventory[i].itemID).item;
                    if (!itemsAccountedFor.Contains(item))
                    {
                        int count = 0;
                        foreach (BasePlayerItem bpi in inventory.inventory)
                        {
                            if (bpi.itemID == inventory.inventory[i].itemID && !bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        newItemSlot = Instantiate(inventoryListItemPrefab);

                        newItemSlot.name = inventory.inventory[i].itemID + ") " + item.name;
                        newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("Name").GetComponent<Text>().text = item.name;
                        newItemSlot.transform.Find("Count").GetComponent<Text>().text = count.ToString();

                        newItemSlot.GetComponent<ItemSlotInteraction>().ID = inventory.inventory[i].itemID;
                        newItemSlot.GetComponent<ItemSlotInteraction>().item = item;
                    }

                    itemsAccountedFor.Add(item);
                }
                else if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0 && EquipmentDB.instance.GetEquip(inventory.inventory[i].itemID).equipment.isFavorite) //if equip
                {
                    anyFavorites = true;
                    Equipment equip = EquipmentDB.instance.GetEquip(inventory.inventory[i].itemID).equipment;

                    newItemSlot = Instantiate(equipListItemPrefab);
                    newItemSlot.name = inventory.inventory[i].itemID + ") " + equip.name;
                    newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = equip.icon;
                    newItemSlot.transform.Find("Name").GetComponent<Text>().text = equip.name;
                    newItemSlot.transform.Find("Count").GetComponent<Text>().text = "";

                    newItemSlot.GetComponent<EquipSlotInteraction>().ID = inventory.inventory[i].itemID;
                    newItemSlot.GetComponent<EquipSlotInteraction>().equip = equip;

                    itemsAccountedFor.Add(equip);
                }

                if (anyFavorites)
                {
                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        void DrawWeapons()
        {
            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                //if equip
                if (inventory.inventory[i].isEquip &&
                    EquipmentDB.instance.GetEquip(inventory.inventory[i].itemID).equipment.type == Item.Types.WEAPON) //if equip and weapon
                {
                    Equipment equip = EquipmentDB.instance.GetEquip(inventory.inventory[i].itemID).equipment;

                    newItemSlot = Instantiate(equipListItemPrefab);
                    newItemSlot.name = inventory.inventory[i].itemID + ") " + equip.name;
                    newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = equip.icon;
                    newItemSlot.transform.Find("Name").GetComponent<Text>().text = equip.name;
                    newItemSlot.transform.Find("Count").GetComponent<Text>().text = "";

                    newItemSlot.GetComponent<EquipSlotInteraction>().ID = inventory.inventory[i].itemID;
                    newItemSlot.GetComponent<EquipSlotInteraction>().equip = equip;

                    itemsAccountedFor.Add(equip);

                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        void DrawArmor()
        {
            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                //if equip
                if (inventory.inventory[i].isEquip &&
                    (EquipmentDB.instance.GetEquip(inventory.inventory[i].itemID).equipment.type == Item.Types.ARMOR ||
                    EquipmentDB.instance.GetEquip(inventory.inventory[i].itemID).equipment.type == Item.Types.CLOTHING)) //if equip and weapon
                {
                    Equipment equip = EquipmentDB.instance.GetEquip(inventory.inventory[i].itemID).equipment;

                    newItemSlot = Instantiate(equipListItemPrefab);
                    newItemSlot.name = inventory.inventory[i].itemID + ") " + equip.name;
                    newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = equip.icon;
                    newItemSlot.transform.Find("Name").GetComponent<Text>().text = equip.name;
                    newItemSlot.transform.Find("Count").GetComponent<Text>().text = "";

                    newItemSlot.GetComponent<EquipSlotInteraction>().ID = inventory.inventory[i].itemID;
                    newItemSlot.GetComponent<EquipSlotInteraction>().equip = equip;

                    itemsAccountedFor.Add(equip);

                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        void DrawPotions()
        {
            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0 &&
                    ItemDB.instance.GetItem(inventory.inventory[i].itemID).item.type == Item.Types.POTION) //if item and not gold
                {
                    Item item = ItemDB.instance.GetItem(inventory.inventory[i].itemID).item;
                    if (!itemsAccountedFor.Contains(item))
                    {
                        int count = 0;
                        foreach (BasePlayerItem bpi in inventory.inventory)
                        {
                            if (bpi.itemID == inventory.inventory[i].itemID && !bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        newItemSlot = Instantiate(inventoryListItemPrefab);

                        newItemSlot.name = inventory.inventory[i].itemID + ") " + item.name;
                        newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("Name").GetComponent<Text>().text = item.name;
                        newItemSlot.transform.Find("Count").GetComponent<Text>().text = count.ToString();

                        newItemSlot.GetComponent<ItemSlotInteraction>().ID = inventory.inventory[i].itemID;
                        newItemSlot.GetComponent<ItemSlotInteraction>().item = item;
                    }

                    itemsAccountedFor.Add(item);

                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        void DrawFood()
        {
            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0 &&
                    (ItemDB.instance.GetItem(inventory.inventory[i].itemID).item.type == Item.Types.FOOD ||
                    ItemDB.instance.GetItem(inventory.inventory[i].itemID).item.type == Item.Types.BEVERAGE)) //if item and not gold
                {
                    Item item = ItemDB.instance.GetItem(inventory.inventory[i].itemID).item;
                    if (!itemsAccountedFor.Contains(item))
                    {
                        int count = 0;
                        foreach (BasePlayerItem bpi in inventory.inventory)
                        {
                            if (bpi.itemID == inventory.inventory[i].itemID && !bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        newItemSlot = Instantiate(inventoryListItemPrefab);

                        newItemSlot.name = inventory.inventory[i].itemID + ") " + item.name;
                        newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("Name").GetComponent<Text>().text = item.name;
                        newItemSlot.transform.Find("Count").GetComponent<Text>().text = count.ToString();

                        newItemSlot.GetComponent<ItemSlotInteraction>().ID = inventory.inventory[i].itemID;
                        newItemSlot.GetComponent<ItemSlotInteraction>().item = item;
                    }

                    itemsAccountedFor.Add(item);

                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        void DrawScrolls()
        {
            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0 &&
                    ItemDB.instance.GetItem(inventory.inventory[i].itemID).item.type == Item.Types.SCROLL) //if item and not gold
                {
                    Item item = ItemDB.instance.GetItem(inventory.inventory[i].itemID).item;
                    if (!itemsAccountedFor.Contains(item))
                    {
                        int count = 0;
                        foreach (BasePlayerItem bpi in inventory.inventory)
                        {
                            if (bpi.itemID == inventory.inventory[i].itemID && !bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        newItemSlot = Instantiate(inventoryListItemPrefab);

                        newItemSlot.name = inventory.inventory[i].itemID + ") " + item.name;
                        newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("Name").GetComponent<Text>().text = item.name;
                        newItemSlot.transform.Find("Count").GetComponent<Text>().text = count.ToString();

                        newItemSlot.GetComponent<ItemSlotInteraction>().ID = inventory.inventory[i].itemID;
                        newItemSlot.GetComponent<ItemSlotInteraction>().item = item;
                    }

                    itemsAccountedFor.Add(item);

                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        void DrawBooks()
        {
            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0 &&
                    ItemDB.instance.GetItem(inventory.inventory[i].itemID).item.type == Item.Types.BOOK) //if item and not gold
                {
                    Item item = ItemDB.instance.GetItem(inventory.inventory[i].itemID).item;
                    if (!itemsAccountedFor.Contains(item))
                    {
                        int count = 0;
                        foreach (BasePlayerItem bpi in inventory.inventory)
                        {
                            if (bpi.itemID == inventory.inventory[i].itemID && !bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        newItemSlot = Instantiate(inventoryListItemPrefab);

                        newItemSlot.name = inventory.inventory[i].itemID + ") " + item.name;
                        newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("Name").GetComponent<Text>().text = item.name;
                        newItemSlot.transform.Find("Count").GetComponent<Text>().text = count.ToString();

                        newItemSlot.GetComponent<ItemSlotInteraction>().ID = inventory.inventory[i].itemID;
                        newItemSlot.GetComponent<ItemSlotInteraction>().item = item;
                    }

                    itemsAccountedFor.Add(item);

                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        void DrawIngredients()
        {
            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0 &&
                    ItemDB.instance.GetItem(inventory.inventory[i].itemID).item.type == Item.Types.INGREDIENT) //if item and not gold
                {
                    Item item = ItemDB.instance.GetItem(inventory.inventory[i].itemID).item;
                    if (!itemsAccountedFor.Contains(item))
                    {
                        int count = 0;
                        foreach (BasePlayerItem bpi in inventory.inventory)
                        {
                            if (bpi.itemID == inventory.inventory[i].itemID && !bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        newItemSlot = Instantiate(inventoryListItemPrefab);

                        newItemSlot.name = inventory.inventory[i].itemID + ") " + item.name;
                        newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("Name").GetComponent<Text>().text = item.name;
                        newItemSlot.transform.Find("Count").GetComponent<Text>().text = count.ToString();

                        newItemSlot.GetComponent<ItemSlotInteraction>().ID = inventory.inventory[i].itemID;
                        newItemSlot.GetComponent<ItemSlotInteraction>().item = item;
                    }

                    itemsAccountedFor.Add(item);

                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        void DrawMiscQuests()
        {
            //always show gold first
            Item goldItem = ItemDB.instance.GetItem(0).item;

            int goldCount = 0;
            foreach (BasePlayerItem bpi in inventory.inventory)
            {
                if (bpi.itemID == 0 && !bpi.isEquip)
                {
                    goldCount++;
                }
            }

            if (goldCount > 0)
            {
                newItemSlot = Instantiate(inventoryListItemPrefab);
                newItemSlot.name = "0) " + goldItem.name;
                newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = goldItem.icon;
                newItemSlot.transform.Find("Name").GetComponent<Text>().text = goldItem.name;
                newItemSlot.transform.Find("Count").GetComponent<Text>().text = goldCount.ToString();

                newItemSlot.GetComponent<ItemSlotInteraction>().ID = 0;
                newItemSlot.GetComponent<ItemSlotInteraction>().item = goldItem;

                newItemSlot.transform.SetParent(inventoryList.transform, false);
            }

            //building new list
            List<Item> itemsAccountedFor = new List<Item>();

            //for each item in player inventory
            for (int i = 0; i <= inventory.inventory.Count - 1; i++)
            {
                if (!inventory.inventory[i].isEquip && inventory.inventory[i].itemID != 0 &&
                    ItemDB.instance.GetItem(inventory.inventory[i].itemID).item.type == Item.Types.MISC) //if item and not gold
                {
                    Item item = ItemDB.instance.GetItem(inventory.inventory[i].itemID).item;
                    if (!itemsAccountedFor.Contains(item))
                    {
                        int count = 0;
                        foreach (BasePlayerItem bpi in inventory.inventory)
                        {
                            if (bpi.itemID == inventory.inventory[i].itemID && !bpi.isEquip)
                            {
                                count++;
                            }
                        }

                        newItemSlot = Instantiate(inventoryListItemPrefab);

                        newItemSlot.name = inventory.inventory[i].itemID + ") " + item.name;
                        newItemSlot.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
                        newItemSlot.transform.Find("Name").GetComponent<Text>().text = item.name;
                        newItemSlot.transform.Find("Count").GetComponent<Text>().text = count.ToString();

                        newItemSlot.GetComponent<ItemSlotInteraction>().ID = inventory.inventory[i].itemID;
                        newItemSlot.GetComponent<ItemSlotInteraction>().item = item;
                    }

                    itemsAccountedFor.Add(item);

                    newItemSlot.transform.SetParent(inventoryList.transform, false);
                }
            }
        }

        public void DestroyEquipDescs()
        {
            foreach (Transform child in equipDescPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }

        void HighlightButton(int index)
        {
            //unhighlight all buttons
            foreach (Transform child in itemTypePanel.transform)
            {
                ColorBlock dimColor = child.GetComponent<Button>().colors;

                dimColor.normalColor = new Color(dimColor.disabledColor.r, dimColor.disabledColor.g, dimColor.disabledColor.b, .5f);
                child.GetComponent<Button>().colors = dimColor;

            }

            //highlight just the 1
            ColorBlock highlightColor = itemTypePanel.transform.GetChild(index).GetComponent<Button>().colors;

            highlightColor.normalColor = new Color(highlightColor.disabledColor.r, highlightColor.disabledColor.g, highlightColor.disabledColor.b, 1f);

            itemTypePanel.transform.GetChild(index).GetComponent<Button>().colors = highlightColor;
        }

        #endregion

        #region POWERS MENU
        IEnumerator DrawPowersMenu()
        {
            StartCoroutine(AnimateMenus(false));
            yield return StartCoroutine(AnimateButtons(false));

            UpdateNavigationButtons();

            oldMenu = 2;

            menuState = menuStates.POWERS;

            SetAnimBool(anim_powerMenu, true);
            yield return StartCoroutine(AnimateButtons(true));

            GameObject.Find("GameManager/Menus/PowersMenuCanvas/MenuPanel/PowersListPanel/PowersScroller").GetComponent<ScrollRect>().enabled = true;

            DrawPowers();
        }

        public void DrawPowers()
        {
            //clears list
            foreach (Transform child in powerList.transform)
            {
                Destroy(child.gameObject);
            }

            powerClasses.Clear();

            //build list to cycle through powersets
            //foreach power type

            selectedClassIndex = 0; //initialize this to 0, to be able to scroll through powersets

            List<AttackDB.powerClasses> classesAccountedFor = new List<AttackDB.powerClasses>();

            foreach (HeroAttackDBEntry hadbe in playerHero.powers)
            {
                BasePower power = AttackDB.instance.GetAttack(hadbe.ID);
                if (!classesAccountedFor.Contains(power.powerClass))
                {
                    //add to classes accounted for
                    classesAccountedFor.Add(power.powerClass);

                    //add to the powerClasses list
                    powerClasses.Add(power.powerClass);
                }
            }

            //draw left and right button
            if (powerClasses.Count > 0)
            {
                leftPowerScrollButton.transform.Find("Text").GetComponent<Text>().text = powerClasses[powerClasses.Count - 1].ToString();
                rightPowerScrollButton.transform.Find("Text").GetComponent<Text>().text = powerClasses[1].ToString();
            }

            //for powers in primary set, draw powers in power list
            DrawPowerSlots(0);

            BoldPowers();

            //Build talents/passives panel
        }

        public void DrawPowerSlots(int ID)
        {
            powerSetNameText.text = powerClasses[ID].ToString();

            foreach (HeroAttackDBEntry hae in playerHero.powers)
            {
                BasePower attack = AttackDB.instance.attacks[hae.ID].power;

                if (attack.powerClass == powerClasses[ID])
                {
                    newPowerSlot = Instantiate(powerListPowerPrefab);
                    newPowerSlot.name = hae.ID + ") " + attack.name;
                    newPowerSlot.transform.Find("BG/Icon").GetComponent<Image>().sprite = attack.icon;
                    newPowerSlot.transform.Find("BG/Name").GetComponent<Text>().text = attack.name;

                    newPowerSlot.transform.SetParent(powerList.transform, false);

                    newPowerSlot.GetComponent<PowerButtonMenuInteraction>().ID = hae.ID;
                    newPowerSlot.GetComponent<PowerButtonMenuInteraction>().power = AttackDB.instance.GetAttack(hae.ID);
                }
            }
        }

        void BoldPowers()
        {
            foreach (Transform child in powerList.transform)
            {
                if (playerHero.leftHandPower == AttackDB.instance.GetAttack(child.gameObject.GetComponent<PowerButtonMenuInteraction>().ID))
                {
                    child.Find("BG/Name").GetComponent<Text>().text = "(L) " + playerHero.leftHandPower.name;
                    child.Find("BG/Name").GetComponent<Text>().fontStyle = FontStyle.Bold;
                }
                else if (playerHero.rightHandPower == AttackDB.instance.GetAttack(child.gameObject.GetComponent<PowerButtonMenuInteraction>().ID))
                {
                    child.Find("BG/Name").GetComponent<Text>().text = "(R) " + playerHero.rightHandPower.name;
                    child.Find("BG/Name").GetComponent<Text>().fontStyle = FontStyle.Bold;
                }
                else
                {
                    child.Find("BG/Name").GetComponent<Text>().text =
                        AttackDB.instance.GetAttack(child.gameObject.GetComponent<PowerButtonMenuInteraction>().ID).name;
                    child.Find("BG/Name").GetComponent<Text>().fontStyle = FontStyle.Normal;
                }
            }
        }

        public void ChangePowerDisplay(bool left)
        {
            if (left) //left button clicked - go backwards in the array
            {
                selectedClassIndex--;

                if (selectedClassIndex < 0)
                {
                    selectedClassIndex = powerClasses.Count - 1;
                }
            }
            else //right button clicked - go forwards in the array
            {
                selectedClassIndex++;

                if (selectedClassIndex == powerClasses.Count)
                {
                    selectedClassIndex = 0;
                }
            }

            foreach (Transform child in powerList.transform)
            {
                Destroy(child.gameObject);
            }

            DrawPowerSlots(selectedClassIndex);

            if ((selectedClassIndex - 1) < 0)
            {
                leftPowerScrollButton.transform.Find("Text").GetComponent<Text>().text = powerClasses[powerClasses.Count - 1].ToString();
            }
            else
            {
                leftPowerScrollButton.transform.Find("Text").GetComponent<Text>().text = powerClasses[selectedClassIndex - 1].ToString();
            }

            if ((selectedClassIndex + 1) == powerClasses.Count)
            {
                rightPowerScrollButton.transform.Find("Text").GetComponent<Text>().text = powerClasses[0].ToString();
            }
            else
            {
                rightPowerScrollButton.transform.Find("Text").GetComponent<Text>().text = powerClasses[selectedClassIndex + 1].ToString();
            }

            BoldPowers();
        }

        #endregion

        #region JOURNAL MENU
        //JOURNAL MENU
        public IEnumerator DrawJournalMenu()
        {
            StartCoroutine(AnimateMenus(false));
            yield return StartCoroutine(AnimateButtons(false));

            UpdateNavigationButtons();

            oldMenu = 3;

            menuState = menuStates.JOURNAL;

            DrawQuestDetails();

            DrawQuestList();

            SetAnimBool(anim_journalMenu, true);
            yield return StartCoroutine(AnimateButtons(true));

            GameObject.Find("GameManager/Menus/JournalMenuCanvas/MenuPanel/QuestListPanel/QuestScroller").GetComponent<ScrollRect>().enabled = true;
        }

        void DrawQuestDetails()
        {
            SetObjectivesText();
            SetRewardsFields();
        }

        void DrawQuestList()
        {
            foreach (Transform child in questList.transform)
            {
                if (!child.name.StartsWith("[") && !child.name.EndsWith("]"))
                {
                    Destroy(child.gameObject);
                }
            }

            foreach (BaseQuest quest in GameObject.Find("GameManager").GetComponent<QuestManager>().activeQuests)
            {
                newQuestSlot = Instantiate(questListPrefab);
                newQuestSlot.name = quest.ID + ") " + quest.name;
                newQuestSlot.transform.Find("LevelText").GetComponent<Text>().text = quest.level.ToString();
                newQuestSlot.transform.Find("NameText").GetComponent<Text>().text = quest.name;

                if (questSelected && questSelectedID == quest.ID)
                {
                    newQuestSlot.transform.Find("NameText").GetComponent<Text>().fontStyle = FontStyle.Bold;
                    newQuestSlot.transform.Find("LevelText").GetComponent<Text>().fontStyle = FontStyle.Bold;
                }

                newQuestSlot.GetComponent<QuestSlotInteraction>().activeQuest = true;
                newQuestSlot.GetComponent<QuestSlotInteraction>().thisQuest = quest;
                newQuestSlot.GetComponent<QuestSlotInteraction>().questID = quest.ID;

                newQuestSlot.transform.SetParent(questList.transform, false);

                if (quest.mainQuest)
                {
                    newQuestSlot.transform.SetSiblingIndex(2);
                }
                else
                {
                    newQuestSlot.transform.SetSiblingIndex((GameObject.Find("GameManager")
                        .GetComponent<QuestManager>().GetActiveSideChild()) + 1);
                }


            }

            foreach (BaseQuest quest in GameObject.Find("GameManager").GetComponent<QuestManager>().completedQuests)
            {
                newQuestSlot = Instantiate(questListPrefab);
                newQuestSlot.name = quest.ID + ") " + quest.name;
                newQuestSlot.transform.Find("LevelText").GetComponent<Text>().text = quest.level.ToString();
                newQuestSlot.transform.Find("NameText").GetComponent<Text>().text = quest.name;

                newQuestSlot.GetComponent<QuestSlotInteraction>().activeQuest = false;
                newQuestSlot.GetComponent<QuestSlotInteraction>().thisQuest = quest;
                newQuestSlot.GetComponent<QuestSlotInteraction>().questID = quest.ID;

                newQuestSlot.transform.SetParent(questList.transform, false);

                newQuestSlot.transform.SetSiblingIndex(questList.transform.childCount);
            }
        }

        public string SetObjectivesText()
        {
            string fullText = "";

            BaseQuest thisQuest;

            if (questSelected)
            {
                thisQuest = QuestManager.instance.GetActiveQuest(questSelectedID);

                if (thisQuest.type == BaseQuest.types.BOOLEAN)
                {
                    foreach (BaseQuestBoolRequirement bqbr in thisQuest.boolReqs)
                    {
                        fullText = fullText + bqbr.objectiveDescription + " - ";
                        if (bqbr.objectiveFulfilled)
                        {
                            fullText = fullText + "Completed";
                        }
                        else
                        {
                            fullText = fullText + "In Progress";
                        }
                        fullText = fullText + "\n";
                    }
                }
                else if (thisQuest.type == BaseQuest.types.GATHER)
                {
                    foreach (BaseQuestGatherRequirement bqgr in thisQuest.gatherReqs)
                    {
                        fullText = fullText + ItemDB.instance.GetItem(bqgr.itemID).item.name + ": " + bqgr.inInventory + "/" + bqgr.quantity + "\n";
                    }
                }
                else if (thisQuest.type == BaseQuest.types.KILLTARGETS)
                {
                    foreach (BaseQuestKillRequirement bqkr in thisQuest.killReqs)
                    {
                        fullText = fullText + EnemyDB.instance.GetEnemy(bqkr.enemyID).name + ": " + bqkr.targetsKilled + "/" + bqkr.quantity + "\n";
                    }
                }

                if (fullText.Length == 0)
                {
                    fullText = QuestManager.instance.GetActiveQuest(questSelectedID).completedText;
                }
            }
            return fullText;
        }

        void SetRewardsFields()
        {
            if (questSelected)
            {
                REWARDEXPTEXT.text = QuestManager.instance.GetActiveQuest(questSelectedID).rewardExp.ToString();
                REWARDGOLDTEXT.text = QuestManager.instance.GetActiveQuest(questSelectedID).rewardGold.ToString();

                if (QuestManager.instance.GetActiveQuest(questSelectedID).rewardItems.Count > 0)
                {
                    Color showIcon = new Color(REWARDITEMICON.color.r, REWARDITEMICON.color.g, REWARDITEMICON.color.b, 1);
                    REWARDITEMICON.color = showIcon;

                    REWARDITEMICON.sprite = QuestManager.instance.GetActiveQuest(questSelectedID).rewardItems[0].item.icon;
                    REWARDITEMTEXT.text = QuestManager.instance.GetActiveQuest(questSelectedID).rewardItems[0].item.name;
                }
            }
            else
            {
                REWARDEXPTEXT.text = "";
                REWARDGOLDTEXT.text = "";

                Color hideIcon = new Color(REWARDITEMICON.color.r, REWARDITEMICON.color.g, REWARDITEMICON.color.b, 0);
                REWARDITEMICON.color = hideIcon;

                REWARDITEMICON.sprite = null;
                REWARDITEMTEXT.text = "";
            }
        }

        #endregion

        #region MAP MENU
        public IEnumerator DrawMapMenu()
        {
            StartCoroutine(AnimateMenus(false));
            yield return StartCoroutine(AnimateButtons(false));

            UpdateNavigationButtons();

            oldMenu = 4;

            menuState = menuStates.MAP;

            inMap = true;
            DrawMapUI();

            yield return StartCoroutine(AnimateButtons(true));
        }

        void DrawMapUI()
        {
            mm.OpenMap();
        }

        #endregion

        private void AddIfSingleDigit(TextMeshProUGUI text)
        {
            if (text.text.Length == 1)
            {
                text.text = "0" + text.text;
            }
        }

        /// <summary>
        /// Calculates the EXP for progress bar for given hero - returns a calculation of the EXP needed to reach the next level - their current EXP
        /// </summary>
        /// <param name="hero">Hero to gather EXP data from</param>
        float GetProgressBarValuesEXP(int expVal, int levelVal)
        {
            float baseLineEXP;
            float heroEXP;

            int[] expLevelThresholds = GameManager.instance.expThresholds;

            if (expVal < expLevelThresholds[0])
            {
                baseLineEXP = expLevelThresholds[0];
                heroEXP = expVal;
            }
            else
            {
                baseLineEXP = (expLevelThresholds[levelVal - 1] - expLevelThresholds[levelVal - 2]);
                heroEXP = (expVal - expLevelThresholds[levelVal - 2]);
                /*Debug.Log("-----");
                Debug.Log("expVal: " + expVal + ", levelVal: " + levelVal);
                Debug.Log("adjVal: " + expLevelThresholds[levelVal-1] + ", and other val should be: " + expLevelThresholds[levelVal - 2]);
                Debug.Log("full threshold: " + baseLineEXP);
                Debug.Log("current exp in bracket:" + heroEXP);
                Debug.Log("-----");*/
            }

            float calcEXP = heroEXP / baseLineEXP;

            return calcEXP;
        }

        /// <summary>
        /// Calculates the HP for progress bar for given hero - returns their current HP / their max HP
        /// </summary>
        /// <param name="hero">Hero to gather HP data from</param>
        float GetProgressBarValuesHP()
        {
            float heroHP = playerHero.curHP;
            float finalMaxHP = playerHero.finalMaxHP;
            float calc_HP;

            calc_HP = heroHP / finalMaxHP;

            return calc_HP;
        }

        /// <summary>
        /// Calculates the MP for progress bar for given hero - returns their current MP / their max MP
        /// </summary>
        /// <param name="hero">Hero to gather MP data from</param>
        float GetProgressBarValuesMP()
        {
            float heroEP = playerHero.curEP;
            float finalMaxEP = playerHero.finalMaxEP;
            float calc_MP;

            calc_MP = heroEP / finalMaxEP;

            return calc_MP;
        }

        public void CursorActive(bool active)
        {
            if (active)
            {
                playerPrefab.GetComponent<SmartFPController.SmartInputManager>().UnblockCursor();
            }
            else
            {
                playerPrefab.GetComponent<SmartFPController.SmartInputManager>().BlockCursor();
            }

        }

        public void ShowCanvas(GameObject canvas, bool active)
        {
            canvas.GetComponent<CanvasGroup>().interactable = active;
            canvas.GetComponent<CanvasGroup>().blocksRaycasts = active;

            if (active)
            {
                canvas.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                canvas.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

}