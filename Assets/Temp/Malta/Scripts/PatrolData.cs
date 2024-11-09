using UnityEngine;

[CreateAssetMenu(fileName = "NewPatrolData", menuName = "Patrol/PatrolData")]
public class PatrolData : ScriptableObject
{
    public Vector3[] patrolPositions; // Agora estamos armazenando posi��es ao inv�s de Transforms
}
