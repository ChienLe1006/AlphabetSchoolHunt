using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FieldOfViewParameter
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 7f;
    [SerializeField] private int rayCount = 50;

    public FieldOfViewParameter()
    {
    }

    public FieldOfViewParameter(LayerMask obstacleLayer, float fov, float viewDistance, int rayCount)
    {
        this.obstacleLayer = obstacleLayer;
        this.fov = fov;
        this.viewDistance = viewDistance;
        this.rayCount = rayCount;
    }

    public LayerMask ObstacleLayer { get => obstacleLayer; }
    public float Fov { get => fov; }
    public float ViewDistance { get => viewDistance; }
    public int RayCount { get => rayCount; }
}

public class FieldOfView 
{
    private readonly Character character;

    private MeshFilter meshFilter;
    private LayerMask obstacleLayer;

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    private float trianglesAngle;
    private float trianglesAngleAttack;

    private float timeToAttack;

    private Transform transform => meshFilter.transform;

    public float FovAngle => character.CharacterProperty.FieldOfViewParameter.Fov;
    public float ViewDistance => character.CharacterProperty.FieldOfViewParameter.ViewDistance;
    public int RayCount => character.CharacterProperty.FieldOfViewParameter.RayCount;
    public LayerMask ObstacleLayer => character.CharacterProperty.FieldOfViewParameter.ObstacleLayer;
    private float PlayerScale => character.transform.localScale.x;

    public FieldOfView(FieldOfViewParameter parameter, Character character, MeshFilter meshFilter)
    {
        this.character = character;
        this.meshFilter = meshFilter;

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        vertices = new Vector3[RayCount + 1];
        uv = new Vector2[vertices.Length];
        triangles = new int[RayCount * 3];
    }

    public void DrawFieldOfViewAndAttack()
    {
        meshFilter.gameObject.SetActive(true);

        trianglesAngle = FovAngle / (RayCount - 1);
        float angle = 90 + FovAngle / 2;
        int triangleIndex = 0;

        vertices[0] = Vector3.zero;

        for (int vertexIndex = 1; vertexIndex < vertices.Length; vertexIndex++)
        {
            Vector3 vertex = vertices[0] + VectorUlti.GetVectorFromAngle(angle) * ViewDistance * PlayerScale;
            vertices[vertexIndex] = vertex;
            bool isObstructed = Physics.Raycast(transform.position, transform.TransformDirection(vertex.normalized), out var obstructor, ViewDistance * PlayerScale, ObstacleLayer);
            
            if (isObstructed)
            {
                Character character = obstructor.collider.GetComponent<Character>();
                if (character != null && character.CanHurt) Attack();
                else character.LockAlert.Init();
            }

            triangles[triangleIndex] = 0;
            triangles[++triangleIndex] = vertexIndex - 1;
            triangles[++triangleIndex] = vertexIndex;
            triangleIndex++;

            angle -= trianglesAngle;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    private List<Character> characters = new List<Character>();
    public void Attack()
    {
        characters.Clear();
        character.PlaySkillVFX();

        trianglesAngleAttack = FovAngle / (RayCount - 1);
        float angle = 90 + FovAngle / 2;
        vertices[0] = Vector3.zero;

        for (int vertexIndex = 1; vertexIndex < vertices.Length; vertexIndex++)
        {
            Vector3 vertex = vertices[0] + VectorUlti.GetVectorFromAngle(angle) * ViewDistance * PlayerScale;            
            bool hit = Physics.Raycast(transform.position, transform.TransformDirection(vertex.normalized), out RaycastHit obstructor, ViewDistance * PlayerScale, ObstacleLayer);

            if (hit)
            {
                Character character = obstructor.collider.GetComponent<Character>();
                if (character != null && !characters.Contains(character) && character.CanHurt)
                {
                    CharacterTakeDamage(character);
                }
            }           

            angle -= trianglesAngleAttack;
        }       
    }

    private void CharacterTakeDamage(Character characterInView)
    {
        characterInView.HP -= character.AttackDamage;
        characterInView.HP = Mathf.Clamp(characterInView.HP, 0, characterInView.MaxHP);
        characters.Add(characterInView);
        
        if (characterInView.HP <= 0)
        {
            character.Kill(characterInView, character.transform);
            character.StopSkillVFX();
        }
    }

    public void DeleteFieldOfView()
    {
        meshFilter.gameObject.SetActive(false);
    }
}
