using PixelCrushers.Wrappers;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeepFry
{
    public class InteractDetection : MonoBehaviour
    {
        public float distance;
        public bool debugging;

        public GameObject confirmGraphic;

        private Text objectName;

        private GameMenu menu;
        private GameObject player;
        private GameObject target;
        [ReadOnly] public GameObject hoveredTarget;
        private GameObject cam;
        public float convCamRotSpd = 2.0f;

        private bool aimCamera;
        private Quaternion tempCamRot;
        private bool inRotation;
        private bool camRotConvEnd;

        NPCPathing npcPathing;

        PlayerInventory pi;

        public GameObject npcItemPrefab;
        private GameObject npcInventoryList, npcInventoryCanvas;

        private GameManager gm;

        private PixelCrushers.DialogueSystem.DialogueSystemTrigger dst;
        private bool inDst, runDstOnce;

        private CanvasGroup crosshairCG;

        //bool inEvent;

        // Start is called before the first frame update
        void Start()
        {
            objectName = GameObject.Find("GameManager/HUD/Interactable/ObjectName").GetComponent<Text>();
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
            player = GameObject.Find("Player");
            cam = player.transform.Find("Camera").gameObject;
            pi = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerInventory>();
            npcInventoryList = GameObject.Find("GameManager/NPCInventory/NPCInventoryCanvas/NPCInventoryPanel/InventoryListPanel/Scroll View/Viewport/Content/InventoryList");
            npcInventoryCanvas = GameObject.Find("GameManager/NPCInventory/NPCInventoryCanvas");
            crosshairCG = GameObject.Find("GameManager/CrosshairCanvas").gameObject.GetComponent<CanvasGroup>();
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (aimCamera)
            {
                //cam.transform.LookAt(target.transform.Find("Interactable"));

                cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.LookRotation(target.transform.Find("Interactable").position - cam.transform.position),
                    convCamRotSpd * Time.deltaTime);

                inRotation = true;
            }
            else
            {
                if (cam.transform.rotation != tempCamRot && inRotation)
                {
                    //cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, tempCamRot, (convCamRotSpd * 3) * Time.deltaTime);
                    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, tempCamRot, (convCamRotSpd * 2f) * Time.deltaTime);
                }

                if (cam.transform.rotation == tempCamRot && camRotConvEnd == false && inRotation)
                {
                    camRotConvEnd = true;
                    inRotation = false;
                }
            }
        }

        private void FixedUpdate()
        {
            objectName.text = "";

            Vector3 fwd = transform.TransformDirection(Vector3.forward) * distance;
            RaycastHit objectHit;

            if (Physics.Raycast(transform.position, fwd, out objectHit, distance))
            {
                GameObject hit = objectHit.collider.gameObject;

                if (!hit.tag.Equals("Terrain"))
                {
                    hoveredTarget = hit;

                    if (hit.tag.Equals("WorldObject"))
                    {
                        if (debugging)
                        {
                            objectName.text = hit.name;
                        }
                        else
                        {
                            objectName.text = "";
                        }
                    }
                    else if (hit.CompareTag("NPC"))
                    {
                        NPCInteraction npci = hit.transform.Find("Interactable").GetComponent<NPCInteraction>();
                        dst = hit.GetComponent<PixelCrushers.DialogueSystem.DialogueSystemTrigger>();

                        if (dst.conversationStarted)
                        {
                            inDst = true;
                        }
                        else
                        {
                            inDst = false;
                        }


                        if (inDst && !runDstOnce)
                        {
                            target = hit;
                            StartCoroutine(StartConversationEvents(hit));
                        }                        

                        //Debug.Log("-----Interactable: " + hit.name + "-----");

                        if (Input.GetButtonDown("Confirm") && !GameObject.Find("ShopCanvas").GetComponent<ShopInteraction>().open && !hit.GetComponent<BaseCombatAI>().isDead)
                        {                            
                            Debug.Log("-----Confirm pressed on: " + hit.name + "-----");

                            bool proceedWithEvents = false;

                            if (npci.processIfTrue.Count > 0)
                            {
                                foreach (int checkBool in npci.processIfTrue)
                                {
                                    if (GlobalBoolsDB.instance.globalBools[checkBool])
                                    {
                                        proceedWithEvents = true;
                                    }
                                }
                            }

                            if (npci.dontProcessIfTrue.Count > 0)
                            {
                                foreach (int checkBool in npci.dontProcessIfTrue)
                                {
                                    if (GlobalBoolsDB.instance.globalBools[checkBool])
                                    {
                                        proceedWithEvents = false;
                                        break;
                                    }
                                }
                            }

                            //if should proceed
                            if (proceedWithEvents)
                            {
                                RunEvent(npci);
                            }
                        }
                    }
                    else if (hit.CompareTag("Enemy")) //interacting with enemy
                    {
                        BaseCombatAI bcai = hit.GetComponent<BaseCombatAI>();

                        if (bcai.isDead) //looting enemy
                        {
                            ShowConfirmGraphic(true);
                            if (Input.GetButtonDown("Confirm"))
                            {
                                //clear old list
                                foreach (Transform child in npcInventoryList.transform)
                                {
                                    Destroy(child.gameObject);
                                }

                                //set new list
                                NPCInventory npci = hit.GetComponent<NPCInventory>();

                                foreach (BaseNPCItem bnpci in npci.inventory)
                                {
                                    GameObject newItemSlot;
                                    newItemSlot = Instantiate(npcItemPrefab);

                                    Item item = new Item();

                                    if (!bnpci.isEquip)
                                    {
                                        item = ItemDB.instance.GetItem(bnpci.itemID).item;
                                    }
                                    else
                                    {
                                        item = EquipmentDB.instance.GetEquip(bnpci.itemID).equipment;
                                    }

                                    newItemSlot.name = bnpci.itemID + ") " + item.name;
                                    newItemSlot.transform.Find("BG/Icon").GetComponent<Image>().sprite = item.icon;
                                    newItemSlot.transform.Find("BG/Name").GetComponent<Text>().text = item.name;
                                    newItemSlot.transform.Find("BG/Count").GetComponent<Text>().text = bnpci.quantity.ToString();

                                    newItemSlot.GetComponent<NPCItemSlotInteraction>().npcInventory = npci;
                                    newItemSlot.GetComponent<NPCItemSlotInteraction>().npcItemPrefab = npcItemPrefab;
                                    newItemSlot.GetComponent<NPCItemSlotInteraction>().isEquip = bnpci.isEquip;

                                    newItemSlot.transform.SetParent(npcInventoryList.transform, false);
                                }

                                //show window
                                npcInventoryCanvas.GetComponent<CanvasGroup>().alpha = 1;
                                npcInventoryCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
                                npcInventoryCanvas.GetComponent<CanvasGroup>().interactable = true;

                                //show mouse
                                menu.CursorActive(true);

                                //lock camera rotation
                                player.GetComponent<SmartFPController.FirstPersonController>().enabled = false;

                                //disable menu
                                menu.inNpcInventory = true;
                            }
                        }

                    }
                    else if (hit.CompareTag("Item") || hit.CompareTag("ItemPart"))
                    {
                        GameObject itemObject = GetTopLevelObject(hit.transform);
                        BaseItemPickup bip = itemObject.GetComponent<BaseItemPickup>();
                        Item item = GetItem(bip.isEquip, bip.itemID);

                        if (bip.owner != null)
                        {
                            objectName.color = Color.red;
                            objectName.text = "Steal " + item.name;
                        }
                        else
                        {
                            objectName.color = Color.white;
                            objectName.text = item.name;
                        }

                        if (Input.GetButtonDown("Confirm"))
                        {
                            Debug.Log("Looting " + item.name);

                            if (itemObject.GetComponent<BaseItemPickup>().itemID == 0) //gold
                            {
                                GameManager.instance.gold += 1;
                                //play gold loot SE
                            }
                            else
                            {
                                BasePlayerItem bpi = new BasePlayerItem
                                {
                                    isEquip = itemObject.GetComponent<BaseItemPickup>().isEquip,
                                    itemID = itemObject.GetComponent<BaseItemPickup>().itemID
                                };
                                pi.inventory.Add(bpi);

                                //play item loot SE
                            }

                            Destroy(itemObject.gameObject);

                        }
                    }
                }
                else
                {
                    objectName.text = "";
                }
            }
            else
            {
                ShowConfirmGraphic(false);
                objectName.text = "";
            }
        }

        GameObject GetTopLevelObject(Transform t)
        {
            GameObject tlo = null;
            bool found = false;
            GameObject parent;

            if (t.CompareTag("ItemPart"))
            {
                parent = t.parent.gameObject;

                //check each parent for "item"
                while (!found)
                {
                    if (parent.CompareTag("Item"))
                    {
                        found = true;
                        return parent;
                    }
                    parent = parent.transform.parent.gameObject;
                }
            }
            else if (t.CompareTag("Item"))
            {
                tlo = t.gameObject;
            }

            return tlo;
        }

        Item GetItem(bool isEquip, int ID)
        {
            if (isEquip)
            {
                foreach (EquipmentDBEntry entry in EquipmentDB.instance.equipment)
                {
                    if (entry.ID == ID)
                    {
                        return entry.equipment;
                    }
                }
            }
            else
            {
                foreach (ItemDBEntry entry in ItemDB.instance.items)
                {
                    if (entry.ID == ID)
                    {
                        return entry.item;
                    }
                }
            }

            return null;
        }

        public IEnumerator StartConversationEvents(GameObject targetObj)
        {
            npcPathing = targetObj.GetComponent(typeof(NPCPathing)) as NPCPathing;

            if (npcPathing != null)
            {
                if (npcPathing.walking)
                {
                    npcPathing.paused = true;
                    npcPathing.agent.isStopped = true;
                    npcPathing.ChangeAnimationState(0);
                }
                else
                {
                    //Debug.Log("NPC does not have an NPCPathing script.");
                }
            }

                runDstOnce = true;

            tempCamRot = cam.transform.rotation;

            yield return new WaitUntil(() => inDst);

            //stop movement, camera movement
            player.GetComponent<SmartFPController.FirstPersonController>().enabled = false;

            //aim camera at interactable object
            aimCamera = true;

            //hide HUD
            menu.ShowCanvas(menu.paramCanvas, false);
            menu.ShowCanvas(menu.compassCanvas, false);

            //hide crosshair
            crosshairCG.alpha = 0;

            //Show cursor if needed
            if (DialogueManager.CurrentConversationState.hasPCResponses)
            {
                menu.CursorActive(true);
            }

            gm.lastInteracted = targetObj;

            StartCoroutine(FinishConversationEvents());
        }

        IEnumerator FinishConversationEvents()
        {
            yield return new WaitUntil(() => !inDst);

            if (npcPathing != null)
            {
                npcPathing.paused = false;
                npcPathing.agent.isStopped = false;
                npcPathing.ChangeAnimationState(1);
            }
            else
            {
                //Debug.Log("NPC does not have an NPCPathing script.");
            }

            //allow movement, camera movement
            aimCamera = false;

            yield return new WaitUntil(() => camRotConvEnd);

            camRotConvEnd = false;

            player.GetComponent<SmartFPController.FirstPersonController>().enabled = true;

            if (!GameObject.Find("ShopCanvas").GetComponent<ShopInteraction>().open)
            {
                //show HUD
                menu.ShowCanvas(menu.paramCanvas, true);
                menu.ShowCanvas(menu.compassCanvas, true);

                //show crosshair
                crosshairCG.alpha = 1;

                //hide cursor
                menu.CursorActive(false);
            }

            runDstOnce = false;
        }

        bool ConvFinished()
        {
            if (DialogueManager.isConversationActive)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        bool ConvStarted()
        {
            if (DialogueManager.currentConversationState == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        void RunEvent(NPCInteraction npci)
        {
            BaseScriptedEvent bse = GameObject.Find("GameManager/DialogueCanvas").GetComponent<BaseScriptedEvent>();

            if (npci.events.Count > 0)
            {
                foreach (BaseEvent runEvent in npci.events) //for each event to run before
                {
                    if (runEvent.method.Length > 0) //if there is a method added (there should be, but skips over if there is nothing)
                    {
                        npci.gameObject.GetComponent<BaseScriptedEvent>().Invoke(runEvent.method, runEvent.waitTime);
                    }
                }
            }

            //inEvent = false;
        }

        void ShowConfirmGraphic(bool show)
        {
            if (show)
            {
                confirmGraphic.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                confirmGraphic.GetComponent<CanvasGroup>().alpha = 0;
            }
            confirmGraphic.GetComponent<CanvasGroup>().interactable = show;
            confirmGraphic.GetComponent<CanvasGroup>().blocksRaycasts = show;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Vector3 fwd = transform.TransformDirection(Vector3.forward) * distance;

            Gizmos.DrawRay(transform.position, fwd);
        }
    }
}
