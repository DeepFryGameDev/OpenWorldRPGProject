using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{
    public class ParameterBars : MonoBehaviour
    {
        public GameObject PowerBar;
        public GameObject HealthBar;
        public GameObject StaminaBar;
        PlayerManager player;

        private void Start()
        {
            player = GameObject.Find("GameManager/PlayerManager").GetComponent<PlayerManager>();
        }
        // Update is called once per frame
        void Update()
        {
            SetParameterBars();
        }

        void SetParameterBars()
        {
            float curPP = player.playersCharacter.curPP;
            float maxPP = player.playersCharacter.baseMaxPP;
            float setPP = (curPP / maxPP);

            float curHP = player.playersCharacter.curHP;
            float maxHP = player.playersCharacter.baseMaxHP;
            float setHP = (curHP / maxHP);

            float curEP = player.playersCharacter.curEP;
            float maxEP = player.playersCharacter.baseMaxEP;
            float setEP = (curEP / maxEP);

            PowerBar.transform.localScale = new Vector2(Mathf.Clamp(setPP, 0, 1), 1f);
            HealthBar.transform.localScale = new Vector2(Mathf.Clamp(setHP, 0, 1), 1f);
            StaminaBar.transform.localScale = new Vector2(Mathf.Clamp(setEP, 0, 1), 1f);
        }
    }
}

