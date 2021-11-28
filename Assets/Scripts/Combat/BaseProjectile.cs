using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    public class BaseProjectile : MonoBehaviour
    {
        public GameObject impactVFX;

        private bool collided;

        [ReadOnly] public BasePower power;

        private void OnCollisionEnter(Collision co)
        {
            //Debug.Log("Collided with " + co.gameObject.name);

            if (co.gameObject.tag != "Projectile" && co.gameObject.tag != "Player" && !collided)
            {
                collided = true;

                var impact = Instantiate(impactVFX, co.contacts[0].point, Quaternion.identity) as GameObject;

                //play impact SFX
                GameManager.instance.PlaySE(co.contacts[0].point, power.impact_sfx);

                //destroy objects
                Destroy(impact, GameManager.instance.secondsForImpactVFXDelay);
                Destroy(gameObject);
            }

            if (co.gameObject.tag == "NPC")
            {
                Debug.Log("collided with " + co.gameObject.name + "!");
                BaseCombatAI bcai = co.gameObject.GetComponent<BaseCombatAI>();
                if (!bcai.isDead)
                {
                    ProcessDamage(bcai);
                }
            }

            if (co.gameObject.tag == "Enemy")
            {
                Debug.Log("collided with " + co.gameObject.name + "!");
                BaseCombatAI bcai = co.gameObject.GetComponent<BaseCombatAI>();
                if (!bcai.isDead)
                {
                    ProcessDamage(bcai);
                }
            }
        }

        private void ProcessDamage(BaseCombatAI bcai)
        {
            bcai.TakeDamage(25); //needs to be changed
        }
    }
}