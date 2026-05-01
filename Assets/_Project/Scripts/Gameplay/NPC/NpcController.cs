using System;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using UnityEngine;
using UnityEngine.AI;

namespace ToyShop.Gameplay.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcController : MonoBehaviour, INpcController
    {
        [Header("Navigation")]
        [SerializeField] private float _stoppingDistance = 0.5f;
        [SerializeField] private float _reachThreshold = 0.2f;

        private NavMeshAgent _agent;
        private NpcAnimator _npcAnimator;
        private NpcStateMachine _stateMachine;
        private NpcContext _context;

        // INpcController — data
        public ToyData SelectedToy { get; set; }
        public bool HasItem { get; set; }
        public IShelfSlot TargetSlot { get; set; }
        public Transform Transform => transform;

        // Pool lifecycle
        public event Action OnReadyToReturn;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _npcAnimator = GetComponent<NpcAnimator>();
            _agent.stoppingDistance = _stoppingDistance;
            _stateMachine = new NpcStateMachine();
        }

        private void Update()
        {
            _stateMachine.CurrentState?.Update(this);
            UpdateAnimator();
        }

        // Called by NpcSpawner after taking from pool
        public void Initialize(NpcContext context, INpcState initialState)
        {
            _context = context;
            HasItem = false;
            SelectedToy = null;
            TargetSlot = null;
            _stateMachine.ChangeState(initialState, this);
        }

        // Called by NpcSpawner when returning to pool
        public void ResetNpc()
        {
            _agent.ResetPath();
            HasItem = false;
            SelectedToy = null;
            TargetSlot = null;
        }

        public void MoveTo(Vector3 destination)
        {
            if (!_agent.isOnNavMesh)
            {
                Debug.LogWarning($"NpcController: agent is not on NavMesh. Object: {gameObject.name}");
                return;
            }

            _agent.SetDestination(destination);
        }

        public bool HasReachedDestination()
        {
            if (!_agent.isOnNavMesh) return false;
            if (_agent.pathPending) return false;
            if (_agent.remainingDistance > _agent.stoppingDistance + _reachThreshold) return false;

            return true;
        }

        public void ChangeState(INpcState newState)
        {
            _stateMachine.ChangeState(newState, this);
        }

        // Fired by ExitStoreState when NPC reaches exit
        public void NotifyReadyToReturn()
        {
            OnReadyToReturn?.Invoke();
        }

        private void UpdateAnimator()
        {
            if (_npcAnimator == null) return;

            float speed = _agent.velocity.magnitude;
            _npcAnimator.SetSpeed(speed);
            _npcAnimator.SetMoving(speed > 0.1f);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stoppingDistance < 0f) _stoppingDistance = 0f;
            if (_reachThreshold < 0f) _reachThreshold = 0f;
        }
#endif
    }
}
