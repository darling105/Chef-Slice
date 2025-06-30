using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed;  // Base speed (renamed from 'speed')

    private void OnTriggerStay(Collider other)
    {
        other.transform.position = Vector3.MoveTowards(
            other.transform.position,
            endPoint.position,
            speed * Time.deltaTime
        );
    }
}
