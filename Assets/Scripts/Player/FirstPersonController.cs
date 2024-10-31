using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 4.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	public float SprintSpeed = 6.0f;
	[Tooltip("Rotation speed of the character")]
	public float RotationSpeed = 1.0f;
	[Tooltip("Acceleration and deceleration")]
	public float SpeedChangeRate = 10.0f;

	[Space(10)]
	[Tooltip("The height the player can jump")]
	public float JumpHeight = 1.2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float Gravity = -15.0f;

	[Space(10)]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	public float JumpTimeout = 0.1f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	public float FallTimeout = 0.15f;

	[Space(10)]
	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	public bool Grounded = true;
	[Tooltip("Useful for rough ground")]
	public float GroundedOffset = -0.14f;
	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	public float GroundedRadius = 0.5f;
	[Tooltip("What layers the character uses as ground")]
	public LayerMask GroundLayers;

	[Space(10)]
	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 90.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -90.0f;

	[Space(10)]

	[Header("Sounds")]
    [SerializeField] private AK.Wwise.Event footstepsEvent;
	private float footstepsTimer = 0f;	
	[SerializeField] private float footstepsInterval = 0.5f; // Adjust interval for timing between steps

	[SerializeField] private AK.Wwise.Event jumpEvent;
    [SerializeField] private AK.Wwise.Event landEvent;
	private bool wasGroundedLastFrame;
	private bool hasJumped = false; 


	// cinemachine
	private float _cinemachineTargetPitch;
	public Camera playerCamera; 

	// player
	private float _speed;
	private float _rotationVelocity;
	private float _verticalVelocity;
	private float _terminalVelocity = 53.0f;

	// timeout deltatime
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;

	private CharacterController _controller;
	//private StarterAssetsInputs _input;


	private const float _threshold = 0.01f;
	
	private bool isRightClickPressed = false;
	
	private Camera _mainCamera;

	public RawImage crosshairImage; 
	

	private void Awake()
	{
		// get a reference to our main camera
		if (_mainCamera == null)
		{
			_mainCamera = Camera.main;
		}
	}

	private void Start()
	{
		_controller = GetComponent<CharacterController>();
		//_input = GetComponent<StarterAssetsInputs>();

		// reset our timeouts on start
		_jumpTimeoutDelta = JumpTimeout;
		_fallTimeoutDelta = FallTimeout;


	}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();

			Cursor.lockState = CursorLockMode.Locked;
        	Cursor.visible = false;


			QuitGame();
		}
		
           
        

	

		private void LateUpdate()
		{
			
				CameraRotation();
			
		}

		public void QuitGame()
		{
			if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
    	}

		private float landingCooldown = 0.6f;
		private float lastLandTime; // Variable to store the last time the player landed

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
			
			 Debug.Log($"Grounded: {Grounded}, wasGroundedLastFrame: {wasGroundedLastFrame}, hasJumped: {hasJumped}");

			if (Grounded && !wasGroundedLastFrame && Time.time > lastLandTime + landingCooldown)
    		{
				landEvent.Post(gameObject);
				Debug.Log("Landed Sound");
				hasJumped = false; 
				lastLandTime = Time.time;
			}
			wasGroundedLastFrame = Grounded;
		}

		private void CameraRotation()
		{
			 // if there is an input
			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = -Input.GetAxis("Mouse Y");

			if (Mathf.Abs(mouseX) > _threshold || Mathf.Abs(mouseY) > _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime for mouse input
				float deltaTimeMultiplier = 1.0f;
					
				_cinemachineTargetPitch += mouseY * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = mouseX * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);

				
			}
					
		}
			
		

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// if there is no input, set the target speed to 0
			Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
			if (moveInput == Vector3.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = moveInput.normalized;

			if (inputDirection != Vector3.zero)
			{
				
				Vector3 forward = _mainCamera.transform.forward;
				Vector3 right = _mainCamera.transform.right;

				
				forward.y = 0f;
				right.y = 0f;
				forward.Normalize();
				right.Normalize();

				
				inputDirection = forward * moveInput.z + right * moveInput.x;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			if (Grounded && currentHorizontalSpeed > 0f)
			{
				footstepsTimer += Time.deltaTime;
				if (footstepsTimer >= footstepsInterval)
				{
					footstepsEvent.Post(gameObject); 
					Debug.Log("Play the footsteps sound");
					// Play the footsteps sound
					footstepsTimer = 0f; // Reset timer
				}
			}
			

		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				

				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (Input.GetButtonDown("Jump") && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					 jumpEvent.Post(gameObject); 
					 Debug.Log("jump Sound  " );
					hasJumped = true; 
				
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
			//wasGroundedLastFrame = Grounded; 
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
