using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
	[Header("Private fields")]
    private PlayerInput inputAction;
	private Rigidbody rig;
	private PlayerInputAction playerInputAction;
	private bool isLeftBlock = false;
	private BlockItem blockItem;
	private Vector3 originalPos;

	[Header("Locomotion")]
	public float currentMovementSpeed;
	public float standardMovementSpeed = 4f;
	public float blockMovementSpeed = 2f;
	Vector2 inputVector;

	[Header("Dash")]
	public float dashDistance = 1.5f;
	private float dashTime = .5f;
	//Ease??

	[Header("Attack")]
	private Animator anim;


	[Header("SerializedFields")]
	[SerializeField] private Transform rightHandPos;
	[SerializeField] private Transform leftHandPos;
	[SerializeField] private Transform enemy;
	[SerializeField] private GameObject dashParticle;
	[SerializeField] private Collider swordCol;
	

	private void Awake() {
		inputAction= GetComponent<PlayerInput>();
		rig = GetComponent<Rigidbody>();
		blockItem= GetComponentInChildren<BlockItem>();
		anim = GetComponent<Animator>();	

		originalPos = blockItem.transform.localPosition;

		playerInputAction = new PlayerInputAction();
		playerInputAction.InputActions.Enable();

		playerInputAction.InputActions.BlockLeft.performed += BlockLeft_performed;
		playerInputAction.InputActions.BlockRight.performed += BlockRight_performed;

		playerInputAction.InputActions.BlockLeft.canceled += BlockLeft_canceled;
		playerInputAction.InputActions.BlockRight.canceled += BlockRight_canceled;

		playerInputAction.InputActions.Dash.performed += Dash_performed;

		playerInputAction.InputActions.LightAttack.performed += LightAttack_performed;

	}

	private void LightAttack_performed(InputAction.CallbackContext obj) {
		LightAttack();
	}

	private void Dash_performed(InputAction.CallbackContext obj) {
		if (inputVector == Vector2.zero) return;
		Dash_Standard();
	}

	private void Start() {
		currentMovementSpeed= standardMovementSpeed;
	}

	private void BlockRight_canceled(InputAction.CallbackContext obj) {
		Unblock();
	}

	private void BlockLeft_canceled(InputAction.CallbackContext obj) {
		Unblock();
	}

	private void BlockRight_performed(InputAction.CallbackContext obj) {
		isLeftBlock = false;
		Block(isLeftBlock);
	}

	private void BlockLeft_performed(InputAction.CallbackContext obj) {
		isLeftBlock = true;
		Block(isLeftBlock);
	}

	private void Block(bool isLeft) {
		blockItem.itemCol.enabled = true;
		blockItem.transform.localPosition = !isLeft ? rightHandPos.localPosition : leftHandPos.localPosition;
		DebugUI.Instance.SetText("Blocking, (right=)" + !isLeft);

		currentMovementSpeed = blockMovementSpeed;
	}

	private void FixedUpdate() {
		inputVector = playerInputAction.InputActions.Movement.ReadValue<Vector2>();
		inputVector.Normalize();
		float speed = 20f; //I wouldnt modify this one
		rig.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed *currentMovementSpeed, ForceMode.Force);
	}

	private void Unblock() {
		blockItem.transform.localPosition = originalPos;
		blockItem.itemCol.enabled = false;
		DebugUI.Instance.SetText("Not blocking");

		currentMovementSpeed = standardMovementSpeed;

	}

	private void Update() {
		transform.LookAt(enemy);
	}

	private void Dash_Standard() {
		rig.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * 10, ForceMode.Impulse);
		DebugUI.Instance.SetText("Dashed...");
		Instantiate(dashParticle, transform.position, new Quaternion(-180,0,0,0));
	}

	private void LightAttack() {
		anim.Play("LightAttack");

		DebugUI.Instance.SetText("Light attacked...");
	}

	public void CheckContact() {
		Debug.Log(swordCol);
	}



}
