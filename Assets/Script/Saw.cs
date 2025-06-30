using BzKovSoft.ObjectSlicerSamples;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private Slicer slicer;

    private void Awake()
    {
        slicer = FindAnyObjectByType<Slicer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BzKnife knife))
        {
            SoundManager.Instance.PlayBlockChopSFX();
            GameManager.Instance.LoseGame();
        }
    }
}
