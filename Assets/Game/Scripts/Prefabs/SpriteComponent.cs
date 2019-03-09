using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteComponent : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite sprite;
    void Start()
    {
        gameObject.AddComponent<SpriteRenderer>().sprite = sprite;
    }

}
