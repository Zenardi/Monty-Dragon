using System;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;
        [SerializeField] [Range(.1f, 1f)] float animatorFowardCap = 1f;


        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.03f;

        [Header("Movement")]
        [SerializeField] float stopDistance = 1.0f;
        [SerializeField] float moveSpeedMultiplier = 0.7f;
        [SerializeField] float animationSpeedMultiplier = 1.7f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreashold = 1f;

        [Header("Nav Mesh Agent")]
        [SerializeField] float navMeshAgentStreetingSpeed = 1.0f;
        [SerializeField] float navMeshAgentStoppingDistance = 1.3f;

        NavMeshAgent navMeshAgent;
        Animator animator;
        Rigidbody myRigidbody;
        float turnAmount;
        float m_ForwardAmount;
        bool isAlive = true;

        private void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;

            var capsuleColider = gameObject.AddComponent<CapsuleCollider>();
            capsuleColider.center = colliderCenter;
            capsuleColider.radius = colliderRadius;
            capsuleColider.height = colliderHeight;

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentStreetingSpeed;
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = false;
            navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;
            navMeshAgent.autoBraking = false;
        }


        private void Update()
        {
            if(!navMeshAgent.isOnNavMesh)
            {
                Debug.LogError(gameObject.name + " not a nav mesh");
            }

            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }


        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = myRigidbody.velocity.y;
                myRigidbody.velocity = velocity;
            }
        }

        internal float GetAnimSpeedMultiplier()
        {
            return animator.speed;
        }

        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        private void Move(Vector3 movement)
        {
            SetFowardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        private void SetFowardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction.
            if (movement.magnitude > moveThreashold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            m_ForwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", m_ForwardAmount * animatorFowardCap, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        public void Kill()
        {
            isAlive = false;
        }
    }
}