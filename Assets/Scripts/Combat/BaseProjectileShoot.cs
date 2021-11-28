using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{
    public class BaseProjectileShoot : MonoBehaviour
    {

        public Camera cam;
        GameObject projectile;
        public Transform LHFirePoint, RHFirePoint;
        public float fireRate = 4;
        public float arcRange = 1;

        private Vector3 destination;
        private float timeToFire;

        private BasePower power;

        BaseHero hero;

        private GameObject player;

        private GameMenu menu;
        private ShopInteraction si;

        // Start is called before the first frame update
        void Start()
        {
            hero = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().playersCharacter;
            player = GameObject.Find("Player");
            menu = GameObject.Find("GameManager/Menus").GetComponent<GameMenu>();
            si = GameObject.Find("ShopCanvas").GetComponent<ShopInteraction>();
        }

        // Update is called once per frame
        void Update()
        {
            if (player.GetComponent<SmartFPController.FirstPersonController>().enabled && !menu.inMenu && !si.open)
            {
                if (Input.GetButton("Fire1") && Time.time >= timeToFire)
                {
                    power = hero.leftHandPower;

                    projectile = power.projectile;

                    timeToFire = Time.time + 1 / power.attackRate;

                    ShootProjectile(true);
                }
                else if (Input.GetButton("Fire2") && Time.time >= timeToFire)
                {
                    power = hero.rightHandPower;

                    projectile = power.projectile;

                    timeToFire = Time.time + 1 / power.attackRate;

                    ShootProjectile(false);
                }
            }
        }

        void ShootProjectile(bool leftHand)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                destination = hit.point;
            else
                destination = ray.GetPoint(1000);

            if (leftHand)
            {
                InstantiateProjectile(LHFirePoint);
            }
            else
            {
                InstantiateProjectile(RHFirePoint);
            }
        }

        void InstantiateProjectile(Transform firePoint)
        {
            GameObject projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity);

            projectileObj.GetComponent<BaseProjectile>().power = power;

            //set the name with power ID so it can be read by BaseProjectile script
            //projectileObj.name = 


            projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * power.projectileSpeed;
            projectileObj.transform.LookAt(projectileObj.transform.position + projectileObj.GetComponent<Rigidbody>().velocity);

            //Give projectiles custom movement - if power.customMovement == ___        
            //iTween.PunchPosition(projectileObj, new Vector3(Random.Range(arcRange, arcRange), Random.Range(arcRange, arcRange), 0), Random.Range(0.5f, 2));

            //Add sound effects
            GameManager.instance.PlaySE(firePoint.position, power.shoot_sfx);
        }

    }
}
