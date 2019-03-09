using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite OnSwitch;
    public Sprite OffSwitch;
    public GameObject[] Doors;
    protected bool isActive = false;
    private AudioSource audioSource;

    public int minScore = 10;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.Space) && collision.gameObject.tag == "Player")
        {
            // It means the ninja will switch the sprite

            if (isActive)
            {
                GetComponent<SpriteRenderer>().sprite = OffSwitch;
            } 
            else if(GameManager.Instance.TotalCoins >= minScore)
            {
                GetComponent<SpriteRenderer>().sprite = OnSwitch;
                if (Doors.Length > 0)
                {
                    foreach (GameObject door in Doors)
                    {
                        Destroy(door);
                    }
                }
             
                if (audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                isActive = true;
            }


        }
    }
}
