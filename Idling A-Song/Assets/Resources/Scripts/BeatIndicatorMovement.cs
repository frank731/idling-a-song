using UnityEngine;
using UnityEngine.UI;

public class BeatIndicatorMovement : MonoBehaviour { 

    public float beatTime;
    public Transform beatMarker;
    public Image image;
    private Coroutine coroutine;
    private void Start()
    {
        coroutine = StartCoroutine(KinematicFunctions.MoveObjectAudioSynced(transform, transform.localPosition, beatMarker.localPosition, beatTime, Conducter.Instance));
        //StartCoroutine(KinematicFunctions.MoveObject(transform, transform.localPosition, beatMarker.localPosition, beatTime));
    }
    private void OnEnable()
    {
        //StartCoroutine(KinematicFunctions.MoveObject(transform, transform.localPosition, beatMarker.localPosition, beatTime));
        coroutine = StartCoroutine(KinematicFunctions.MoveObjectAudioSynced(transform, transform.localPosition, beatMarker.localPosition, beatTime, Conducter.Instance));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("End"))
        {
            image.enabled = false;
            Invoke("TurnOff", 0.1f);
            //StopCoroutine(coroutine);
            //gameObject.SetActive(false);
            //Debug.Log(conducter.songPos);
        }
    }
    private void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
