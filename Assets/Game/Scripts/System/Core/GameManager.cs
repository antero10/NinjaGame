using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Text CoinsCount;
    public GameObject NinjaPrefab;
    public GameObject StartingPoint;
    public int TotalCoins = 0;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        StartGame();

    }

    // Update is called once per frame
    void Update()
    {
        StartGame();

    }

    public void AddCoin(int value)
    {
        TotalCoins += value;
        CoinsCount.text = "Coins: " + TotalCoins;
    }

    public void StartGame()
    {
        if (GameObject.FindWithTag("Player") == null)
        {
            CoinsCount.text = "Coins: " + TotalCoins;
            Instantiate(NinjaPrefab, StartingPoint.transform.position, Quaternion.identity);
        }

    }
}
