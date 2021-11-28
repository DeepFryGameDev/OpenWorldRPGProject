using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepFry
{

    [System.Serializable]
    public class SpawnZone : MonoBehaviour
    {
        public List<BaseSpawnZone> enemySpawns = new List<BaseSpawnZone>();
        public float zoneRange;

        private Transform enemiesHeader;

        // Start is called before the first frame update
        void Start()
        {
            enemiesHeader = GameObject.Find("[Enemies]").transform;

            OriginalSpawning();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OriginalSpawning()
        {
            for (int i = 0; i < enemySpawns.Count; i++)
            {
                Vector3 randomSpawnPoint = RandomPointInBounds();

                Debug.Log(gameObject.name + " - Spawning " + EnemyDB.instance.GetEnemy(enemySpawns[i].enemyID).name + " at " + randomSpawnPoint);
                SpawnEnemy(EnemyDB.instance.GetEnemy(enemySpawns[i].enemyID).NewEnemy(), randomSpawnPoint, i);
            }

        }

        public void SpawnEnemy(BaseEnemy enemy, Vector3 position, int ID)
        {
            var enemyObj = Instantiate(enemy.prefab, position, Quaternion.identity) as GameObject;
            enemyObj.transform.SetParent(enemiesHeader);

            enemyObj.name = ID + ") " + enemy.name;

            enemyObj.GetComponent<BaseCombatAI>().encounterID = ID;
            enemyObj.GetComponent<BaseCombatAI>().respawnSeconds = enemySpawns[ID].respawnSeconds;
            enemyObj.GetComponent<BaseCombatAI>().spawnZone = this;
            enemyObj.GetComponent<BaseCombatAI>().enemyID = enemySpawns[ID].enemyID;

            enemyObj.GetComponent<BaseCombatAI>().SetEnemy();
        }

        public Vector3 RandomPointInBounds()
        {
            Vector3 newPos = (Random.insideUnitSphere * zoneRange) + transform.position;

            Vector3 getYPos = new Vector3(newPos.x, 0, newPos.z);
            float adjY = Terrain.activeTerrain.SampleHeight(getYPos);

            return new Vector3(newPos.x, adjY, newPos.z);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, zoneRange);
        }
    }

}