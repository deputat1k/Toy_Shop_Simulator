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
        [SerializeField] private float _rotationSpeed = 540f;
        [SerializeField] private float _minMoveDirectionMagnitude = 0.1f;

        private NavMeshAgent _agent;
        private NpcAnimator _npcAnimator;
        private NpcItemVisual _npcItemVisual;
        private NpcStateMachine _stateMachine;

        // true  -> a state has taken manual control of facing (e.g. looking at a shelf/counter)
        // false -> body automatically faces movement direction every frame
        private bool _manualFacingActive;

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

            // Rotation is fully handled by FaceDirection() below.
            // NavMeshAgent's built-in rotation fought with manual facing
            // during sharp turns — this caused the visible sliding/drift.
            _agent.updateRotation = false;

            _stateMachine = new NpcStateMachine();
        }

        private void Update()
        {
            _stateMachine.CurrentState?.Update(this);

            if (!_manualFacingActive)
                FaceMovementDirection();

            UpdateAnimator();
        }

        // Rotates the body toward the agent's steering direction every frame.
        // Replaces NavMeshAgent's internal rotation with one controllable, consistent system.
        private void FaceMovementDirection()
        {
            Vector3 desired = _agent.desiredVelocity;
            desired.y = 0f;

            if (desired.magnitude < _minMoveDirectionMagnitude) return;

            FaceDirection(transform.position + desired);
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

        // Same external contract as before — states don't need any changes.
        // true  -> auto-face movement direction takes over
        // false -> state controls facing manually via FaceDirection()
        public void SetAgentRotationEnabled(bool enabled)
        {
            _manualFacingActive = !enabled;
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