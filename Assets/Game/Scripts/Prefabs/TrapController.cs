using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{

    private Animator _animator;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        //_animator.SetBool("ActiveFire", true);

    }
   

    // Update is called once per frame
    void Update()
    {
       
    }
    public void ActiveTrap()
    {
        isActive = true;
        _animator.SetBool("ActiveFire", true);
    }

    public void DesactivateTrap()
    {
        isActive = false;
        _animator.SetBool("ActiveFire", false);
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isActive)
        {

            Destroy(collision.gameObject);
            GameManager.Instance.StartGame();
        }

    }
}
