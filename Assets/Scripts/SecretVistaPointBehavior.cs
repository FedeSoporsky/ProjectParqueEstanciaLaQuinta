using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SecretVistaPointBehavior : MonoBehaviour
{
    public CompletableItemPointBehavior pointBehavior;
    public Player player;
    public Image image;
    void Start()
    {
        StartCoroutine(CheckProximity());
    }

    IEnumerator CheckProximity()
    {
        yield return new WaitForSeconds(1); //This is for dealing with the race condition.
        while (!player.IsAtSamePosition(pointBehavior.geoPoint))
        {
            yield return new WaitForSeconds(1);
        }

        image.enabled = true;
    }
}