using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stopDistance = 1.0f;
        [SerializeField] float moveSpeedMultiplier = 0.7f;

        ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
        
        Vector3 clickPoint;
        GameObject walkTarget;
        NavMeshAgent agent;
        Animator animator;
        Rigidbody myRigidbody;

        void Start()
        {
            CameraUI.CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraUI.CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            myRigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            walkTarget = new GameObject("walkTarget");

            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = true ;
            agent.updateRotation = false;
            agent.stoppingDistance = stopDistance;

            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        private void Update()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity);
            }
            else
            {
                character.Move(Vector3.zero);
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                agent.SetDestination(enemy.transform.position);
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
    }
}