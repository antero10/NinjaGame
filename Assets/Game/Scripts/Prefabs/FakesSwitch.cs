using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakesSwitch : SwitchController
{
   
    public GameObject floor;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    IEnumerator WaitForSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        foreach (TrapController trap in floor.transform.GetComponentsInChildren<TrapController>())
        {
            trap.DesactivateTrap();
        }
        isActive = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            foreach (TrapController trap in floor.transform.GetComponentsInChildren<TrapController>())
            {
                trap.ActiveTrap();
            }

            StartCoroutine(WaitForSeconds(5));
        }

    }

    private new void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }
}
