using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	//config params
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] Vector2 screenPadding = new Vector2(0,0);
	[SerializeField] bool allowVerticalMovement = true;
	[SerializeField] GameObject projectile;
	[SerializeField] float projectileSpeed = 10f;
	[SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] float projectileXOffset = 0.18f;
	private float xMin, xMax, yMin, yMax;
	Coroutine firingCoroutine;
	//State
	
	//cached references
	SpriteRenderer sprite;

	// Start is called before the first frame update
	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		SetUpMoveBoundaries();
	}
	// Update is called once per frame
	void Update()
	{
		Move();
		Fire();
	}
	private void SetUpMoveBoundaries() {
		Camera gameCamera = Camera.main;
		var min = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0));
		var max = gameCamera.ViewportToWorldPoint(new Vector3(1,1,0));
		xMin = min.x + sprite.bounds.extents.x + screenPadding.x;
		yMin = min.y + sprite.bounds.extents.y + screenPadding.y;
		xMax = max.x - sprite.bounds.extents.x - screenPadding.x;
		yMax = max.y - sprite.bounds.extents.y - screenPadding.y;
	}

	private void Move() {
		var newXPos = transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		// Clamp
		newXPos = Mathf.Clamp(newXPos, xMin, xMax);
		var newYPos = transform.position.y;
		if (allowVerticalMovement) {
			newYPos += Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
			// Clamp
			newYPos = Mathf.Clamp(newYPos, yMin, yMax);
		}

		transform.position = new Vector2(newXPos, newYPos);
	}
	/* private void Rotate() {
		var mouse_pos = Input.mousePosition;
		mouse_pos.z = -20;
		var object_pos = Camera.main.WorldToScreenPoint(transform.position);
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;
		var angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	} */
	private void Fire() {
		if (Input.GetButtonDown("Fire1")) {
			firingCoroutine = StartCoroutine(RepeatFire());
		}
		if (Input.GetButtonUp("Fire1")) {
			StopCoroutine(firingCoroutine);
		}
	}
	IEnumerator RepeatFire() {
		while (true)
		{
            var projectileSpawnPoint = new Vector3(transform.position.x + projectileXOffset, transform.position.y, transform.position.z);
			var proj = Instantiate(projectile, projectileSpawnPoint, Quaternion.identity);
			proj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
			yield return new WaitForSeconds(projectileFiringPeriod);
		}
	}
}
