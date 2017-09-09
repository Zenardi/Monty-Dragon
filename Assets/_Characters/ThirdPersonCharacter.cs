using UnityEngine;

namespace RPG.Characters
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;
		
		Rigidbody myRigidbody;
		Animator animator;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;


		void Start()
		{
			animator = GetComponent<Animator>();
            myRigidbody = GetComponent<Rigidbody>();           
            animator.applyRootMotion = true;
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}


		public void Move(Vector3 move)
		{
			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			ApplyExtraTurnRotation();


			//ScaleCapsuleForCrouching(crouch);
			//PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}
        
		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}



    }
}
