using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeChecker : MonoBehaviour
{
    public List<GameObject> inRangeTicks;
    public GameObject explode;
    private ObjectPooler objectPooler;
    private int explodeInd;

    private void Start()
    {
        objectPooler = ObjectPooler.SharedInstance;
        explodeInd = objectPooler.AddObject(explode);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Tick")) inRangeTicks.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tick")) inRangeTicks.Remove(collision.gameObject);
    }

    public void RemoveTick()
    {
        if(inRangeTicks.Count > 0)
        {
            GameObject explosion = objectPooler.GetPooledObject(explodeInd, inRangeTicks[0].transform);
            //explosion.transform.SetParent(transform.parent.parent.parent);
            explosion.transform.position = inRangeTicks[0].transform.position;
            explosion.GetComponent<ParticleSystem>().Play();
            inRangeTicks[0].SetActive(false);
        }
    }

}
