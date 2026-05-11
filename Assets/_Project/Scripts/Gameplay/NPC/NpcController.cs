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
        [SerializeField] private float _reachThreshold = 0.3f;
        [SerializeField] private float _rotationSpeed = 360f;
        [SerializeField] private float _moveSpeed = 2f;

        private NavMeshAgent _agent;
        private NpcAnimator _npcAnimator;
        private NpcItemVisual _npcItemVisual;
        private NpcStateMachine _stateMachine;

        public ToyData SelectedToy { get; set; }
        public bool HasItem { get; set; }
        public IShelfSlot TargetSlot { get; set; }
        public Transform Transform => transform;
        public NpcAnimator NpcAnimator => _npcAnimator;

        public event Action OnReadyToReturn;

        public void PlayInteractAnimation() => _npcAnimator?.PlayInteractAnimation();
        public void StopInteractAnimation() => _npcAnimator?.StopInteractAnimation();

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _npcAnimator = GetComponent<NpcAnimator>();
            _npcItemVisual = GetComponent<NpcItemVisual>();
            _agent.stoppingDistance = _stoppingDistance;
            _stateMachine = new NpcStateMachine();
        }

        private void Update()
        {
            _stateMachine.CurrentState?.Update(this);
            UpdateAnimator();
        }

        public void Initialize(NpcContext context, INpcState initialState)
        {
            HasItem = false;
            SelectedToy = null;
            TargetSlot = null;
            SetAgentRotationEnabled(true);
            _stateMachine.ChangeState(initialState, this);
        }

        public void ResetNpc()
        {
            _agent.ResetPath();
            HasItem = false;
            SelectedToy = null;
            TargetSlot = null;
            SetAgentRotationEnabled(true);
            HideItemVisual();
        }

        public void MoveTo(Vector3 destination)
        {
            if (!_agent.isOnNavMesh)
            {
                Debug.LogWarning($"NpcController: agent not on NavMesh. Object: {gameObject.name}");
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

        public void FaceDirection(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0f;

            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime);
        }

        public bool IsFacingTarget(Vector3 targetPosition, float angleThreshold = 10f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0f;

            if (direction == Vector3.zero) return true;

            return Vector3.Angle(transform.forward, direction) <= angleThreshold;
        }

        public void SetAgentRotationEnabled(bool enabled)
        {
            _agent.updateRotation = enabled;
        }

        public void ShowItemVisual() => _npcItemVisual?.Show();
        public void HideItemVisual() => _npcItemVisual?.Hide();

        public void NotifyReadyToReturn() => OnReadyToReturn?.Invoke();

        private void UpdateAnimator()
        {
            if (_npcAnimator == null) return;

            float speed = _agent.velocity.magnitude;
            _npcAnimator.SetSpeed(speed > 0.3f ? speed : 0f);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stoppingDistance < 0f) _stoppingDistance = 0f;
            if (_reachThreshold < 0f) _reachThreshold = 0f;
            if (_rotationSpeed < 0f) _rotationSpeed = 0f;
        }
#endif
    }
}
