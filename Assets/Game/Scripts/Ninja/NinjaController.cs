using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class NinjaController : MonoBehaviour, IAnimationCompleted
{

    public GameObject FirePrefab;
	public float speed = 5.0f;
	public float movementThreshold = 0.5f;
    public float projSpeed = 5f;

    private Vector2 inputDirection;
	private Rigidbody2D _rigidbody2D;
	private Animator _animator;
    private bool attack = false;

	private void Start()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

    public void AnimationCompleted(int shortHashName)
    {
        attack = false;
    }

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !attack)
        {
            attack = true;
            _animator.SetTrigger("Attack");
            _rigidbody2D.velocity = new Vector2();
            if (FirePrefab != null)
            {
                //TODO: Instatiate the fire bullet in facing position and add the velocity... none of this code is working by now
                //GameObject fireInstance = Instantiate(FirePrefab, transform.position, transform.rotation);

                ////fireInstance.GetComponent<Rigidbody2D>().velocity = transform.forward * projSpeed;

                //fireInstance.GetComponent<Rigidbody2D>().AddForce(transform.forward * projSpeed);
                //fireInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x * projSpeed, transform.position.y * projSpeed);
            }
        }
        else if(!attack)
        {
            inputDirection.x = Input.GetAxis("Horizontal");
            inputDirection.y = Input.GetAxis("Vertical");

            if (inputDirection.magnitude > movementThreshold)
            {
                _rigidbody2D.velocity = new Vector2(inputDirection.x * speed, inputDirection.y * speed);

                // Set the input values on the animator
                _animator.SetFloat("inputX", inputDirection.x);
                _animator.SetFloat("inputY", inputDirection.y);
                _animator.SetBool("isWalking", true);
            }
            else
            {
                _rigidbody2D.velocity = new Vector2();
                _animator.SetBool("isWalking", false);
            }
        }

    }
}
