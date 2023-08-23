using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ProjectileStat", order = 4)]
public class ProjectileStatScriptableObject : ScriptableObject
{
    public int damage;
    public float speed;
    public GameObject projectilePrefab;
}