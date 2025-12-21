using System.Collections;
using System.Collections.Generic;
using Frogxel.Lanes;
using UnityEngine;

namespace Managers
{
    public class PickupSpawner : MonoBehaviour
    {

        [Header("Where lanes exist")]
        [SerializeField] private Transform lanesRoot;

        [Header("Pickups (add as many as you want)")]
        [SerializeField] private List<GameObject> pickupPrefabs = new();

        [Header("Spawn Settings")]
        [SerializeField] private bool spawnOnEnable = true;
        [SerializeField] private int initialSpawnCount = 3;

        [Tooltip("Cooldown between spawn")]
        [SerializeField] private float spawnInterval = 5f;

        [Tooltip("Limit pickups per lane")]
        [SerializeField] private int maxPickupsPerLane = 1;

        [Header("Middle-only lanes")]
        [SerializeField] private bool excludeFirstAndLastLane = true;


        private Coroutine _loop;

        private void OnEnable()
        {
            if (!spawnOnEnable) return;

            StartCoroutine(SpawnInitialAfterOneFrame());

            if (spawnInterval > 0f)
                _loop = StartCoroutine(SpawnLoop());
        }

        private void OnDisable()
        {
            if (_loop != null)
            {
                StopCoroutine(_loop);
                _loop = null;
            }
        }

        private IEnumerator SpawnInitialAfterOneFrame()
        {
            yield return null;

            for (int i = 0; i < initialSpawnCount; i++)
                TrySpawnOne();
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                TrySpawnOne();
            }
        }

        private void TrySpawnOne()
        {
            if (pickupPrefabs == null || pickupPrefabs.Count == 0) return;
            if (!lanesRoot) return;

            var lanes = new List<LaneController>();
            lanesRoot.GetComponentsInChildren(true, lanes);
            if (lanes.Count == 0) return;

            int startIndex = 0;
            int endIndexExclusive = lanes.Count;

            // Only middle lanes: exclude first + last
            if (excludeFirstAndLastLane)
            {
                if (lanes.Count <= 2) return;
                startIndex = 2;
                endIndexExclusive = lanes.Count - 3;
            }

            var lane = lanes[Random.Range(startIndex, endIndexExclusive)];
            if (!lane) return;

            // Respect max per lane
            if (maxPickupsPerLane > 0)
            {
                int existing = lane.GetComponentsInChildren<PickupInstance>(true).Length;
                if (existing >= maxPickupsPerLane) return;
            }

            var prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Count)];
            if (!prefab) return;

            Vector3 pos = GetRandomPointInLane(lane);

            var go = Instantiate(prefab, pos, Quaternion.identity, lane.transform);

            if (!go.GetComponent<PickupInstance>())
                go.AddComponent<PickupInstance>();
        }

        private Vector3 GetRandomPointInLane(LaneController lane)
        {
            var c2d = lane.GetComponentInChildren<Collider2D>();
            if (c2d != null)
            {
                var b = c2d.bounds;

                float minX = b.min.x;
                float maxX = b.max.x;

                float minY = b.min.y;
                float maxY = b.max.y;


                return new Vector3(
                    Random.Range(minX, maxX),
                    Random.Range(minY, maxY),
                    lane.transform.position.z
                );
            }

            return lane.transform.position;
        }
    }

    public class PickupInstance : MonoBehaviour { }
}
