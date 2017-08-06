using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeCell : MonoBehaviour, IEffect
{
    private Coroutine quaking;
    private HexCell cell;

    public void DoAt(HexCell cell, float dY, float duration, float delay = 0)
    {
        if (cell.GetComponentInChildren<QuakeCell>() == null)
        {
            // Add component, set its cell reference
            QuakeCell instance = Instantiate(gameObject, cell.transform).GetComponent<QuakeCell>();
            instance.cell = cell;

            // Start Coroutine with params
            instance.Do(dY, duration, delay);
        }
    }

    public void Do(float dY, float duration, float delay = 0)
    {
        if (quaking == null)
            StartCoroutine(DoQuake(dY, duration, delay));
    }

    public void Stop()
    {
        // Stop tweening, stop particles, stop quaking
        GetComponentInChildren<ParticleSystem>().Stop();
        iTween.Stop(cell.gameObject, "PunchPosition");
        quaking = null;

        // Get position from map, move to position
        cell.ResetPosition();
    }

    private void OnDestroy()
    {
        Stop();
    }

    IEnumerator DoQuake(float dY, float duration, float delay = 0)
    {
        // Wait for delay, then quake
        yield return new WaitForSeconds(delay);
        iTween.PunchPosition(cell.gameObject, Vector3.down * dY, duration);

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        var main = particleSystem.main;
        main.gravityModifier = -dY / 4;
        particleSystem.Play();

        // Wait for quake's duration to expire, then null ref to quake
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
