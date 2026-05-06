using System;
using System.Collections;
using ToyShop.Core.Interfaces;
using ToyShop.Gameplay.NPC.States;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay.NPC.Spawning
{
    public class NpcSpawner : MonoBehaviour, INpcSpawner
    {
        [Header("Prefab")]
        [SerializeField] private NpcController _npcPrefab;

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnInterval = 5f;
        [SerializeField] private int _maxNpcsInScene = 5;

        [Header("Pool Settings")]
        [SerializeField] private int _poolDefaultCapacity = 5;
        [SerializeField] private int _poolMaxSize = 10;

        [Header("Brain Config")]
        [SerializeField] private NpcBrainConfig _brainConfig;

        private ICheckoutService _checkoutService;
        private IPointOfInterestProvider _pointsOfInterest;

        private NpcPool _pool;
        private Coroutine _spawnRoutine;

        public int ActiveNpcCount => _pool?.CountActive ?? 0;

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
            _pool = new NpcPool(
                _npcPrefab,
                transform,
                _poolDefaultCapacity,
                _poolMaxSize);

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

                if (_pool.CountActive < _maxNpcsInScene)
                    SpawnNpc();
            }
        }

        private void SpawnNpc()
        {
            NpcController npc = _pool.Get();

            npc.transform.position = _pointsOfInterest.GetEntryPoint();

            NpcContext context = BuildContext();
            INpcState initialState = BuildStates(context);

            // Keep reference to correctly unsubscribe lambda
            Action returnAction = null;
            returnAction = () =>
            {
                npc.OnReadyToReturn -= returnAction;
                _pool.Release(npc);
            };

            npc.OnReadyToReturn += returnAction;
            npc.Initialize(context, initialState);
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
            // Build bottom-up — each state needs reference to next
            var exitState = new ExitStoreState(context);
            var waitInQueue = new WaitInQueueState(context, exitState);
            var moveToCheckout = new MoveToCheckoutState(context, waitInQueue);
            var selectItem = new SelectItemState(context, moveToCheckout, exitState);
            var browseShelf = new BrowseShelfState(context, selectItem, exitState);
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
        }
#endif
    }
}