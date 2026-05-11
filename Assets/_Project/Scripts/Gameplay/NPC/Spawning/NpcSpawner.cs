using System;
using System.Collections;
using System.Collections.Generic;
using ToyShop.Core.Interfaces;
using ToyShop.Gameplay.NPC.States;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay.NPC.Spawning
{
    public class NpcSpawner : MonoBehaviour, INpcSpawner
    {
        [Header("Prefabs")]
        [Tooltip("Spawner randomly picks one prefab per NPC")]
        [SerializeField] private NpcController[] _npcPrefabs;

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnInterval = 5f;
        [SerializeField] private int _maxNpcsInScene = 5;

        [Header("Pool Settings")]
        [SerializeField] private int _poolDefaultCapacity = 3;
        [SerializeField] private int _poolMaxSize = 10;

        [Header("Behavior")]
        [SerializeField] private NpcBrainConfig _brainConfig;
        [SerializeField] private float _shelfIdleDuration = 2f;
        [SerializeField] private float _postPickupDelay = 1f;

        private ICheckoutService _checkoutService;
        private IPointOfInterestProvider _pointsOfInterest;

        // One pool per prefab type
        private Dictionary<NpcController, NpcPool> _pools;
        private Coroutine _spawnRoutine;

        public int ActiveNpcCount
        {
            get
            {
                int count = 0;
                if (_pools == null) return 0;
                foreach (var pool in _pools.Values)
                    count += pool.CountActive;
                return count;
            }
        }

        [Inject]
        public void Construct(
            ICheckoutService checkoutService,
            IPointOfInterestProvider pointsOfInterest)
        {
            _checkoutService = checkoutService;
            _pointsOfInterest = pointsOfInterest;
        }

        private void Start()
        {
            if (_npcPrefabs == null || _npcPrefabs.Length == 0)
            {
                Debug.LogError("NpcSpawner: No NPC prefabs assigned.");
                return;
            }

            _pools = new Dictionary<NpcController, NpcPool>();
            foreach (NpcController prefab in _npcPrefabs)
            {
                if (prefab == null) continue;
                _pools[prefab] = new NpcPool(
                    prefab, transform,
                    _poolDefaultCapacity,
                    _poolMaxSize);
            }

            StartSpawning();
        }

        public void StartSpawning()
        {
            if (_spawnRoutine != null) return;
            _spawnRoutine = StartCoroutine(SpawnRoutine());
        }

        public void StopSpawning()
        {
            if (_spawnRoutine == null) return;
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                if (ActiveNpcCount < _maxNpcsInScene)
                    SpawnNpc();
            }
        }

        private void SpawnNpc()
        {
            NpcController prefab = GetRandomPrefab();
            if (prefab == null) return;

            NpcPool pool = _pools[prefab];
            NpcController npc = pool.Get();

            npc.transform.position = _pointsOfInterest.GetEntryPoint();

            NpcContext context = BuildContext();
            INpcState initialState = BuildStates(context);

            Action returnAction = null;
            returnAction = () =>
            {
                npc.OnReadyToReturn -= returnAction;
                pool.Release(npc);
            };

            npc.OnReadyToReturn += returnAction;
            npc.Initialize(context, initialState);
        }

        private NpcController GetRandomPrefab()
        {
            if (_pools == null || _pools.Count == 0) return null;

            NpcController[] prefabs = new NpcController[_pools.Count];
            _pools.Keys.CopyTo(prefabs, 0);

            return prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
        }

        private NpcContext BuildContext()
        {
            return new NpcContext(
                new NpcBrain(_brainConfig),
                _checkoutService,
                _pointsOfInterest);
        }

        private INpcState BuildStates(NpcContext context)
        {
            var exitState = new ExitStoreState(context);
            var waitInQueue = new WaitInQueueState(context, exitState);
            var moveToCheckout = new MoveToCheckoutState(context, waitInQueue);
            var selectItem = new SelectItemState(context, moveToCheckout, exitState, _postPickupDelay);
            var idleAtShelf = new IdleAtShelfState(context, selectItem, _shelfIdleDuration);
            var browseShelf = new BrowseShelfState(context, idleAtShelf, exitState);
            var enterStore = new EnterStoreState(context, browseShelf);

            return enterStore;
        }

        private void OnDestroy() => StopSpawning();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_spawnInterval < 0.5f) _spawnInterval = 0.5f;
            if (_maxNpcsInScene < 1) _maxNpcsInScene = 1;
            if (_poolDefaultCapacity < 1) _poolDefaultCapacity = 1;
            if (_poolMaxSize < _poolDefaultCapacity) _poolMaxSize = _poolDefaultCapacity;
            if (_shelfIdleDuration < 0.5f) _shelfIdleDuration = 0.5f;
            if (_postPickupDelay < 0f) _postPickupDelay = 0f;
        }
#endif
    }
}