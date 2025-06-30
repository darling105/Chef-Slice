using BzKovSoft.ObjectSlicerSamples;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float stunTime;

    private Slicer slicer;

    private void Start()
    {
        slicer = FindAnyObjectByType<Slicer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BzKnife knife))
        {
            slicer.Stun(stunTime);
            SoundManager.Instance.PlayBlockChopSFX();
        }
    }
}
