using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AoECircle : MonoBehaviour
{
    [SerializeField] private float explosionRange = 5f; 
    [SerializeField] private int segments = 36;

    private LineRenderer lineRenderer;
    private LayerMask obstacleLayer;


    void Start()
    {
        obstacleLayer = LayerMask.GetMask("Default", "Player", "Obstacle");
        lineRenderer = GetComponent<LineRenderer>();
        // DrawCircle(explosionRange);

    }

    void Update()  {
        if(this.gameObject.GetComponent<EnemyStateController>().GetCurrentState() is BossSpecialState){
            lineRenderer.enabled = true;
            DrawAoEWithObstacles(this.gameObject.transform.position, explosionRange, obstacleLayer);
        } else {
            lineRenderer.enabled = false;
        }
    }

    private void DrawCircle(float radius)
    {
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;

        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
    }

    void DrawAoEWithObstacles(Vector3 center, float radius, LayerMask obstacleLayer)
    {
        int segments = 100;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;

        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

            RaycastHit hit;
            Vector3 worldPoint;

            if (Physics.Raycast(center, direction, out hit, radius, obstacleLayer))
            {
                worldPoint = hit.point;
            }
            else
            {
                worldPoint = center + direction * radius;
            }

            lineRenderer.SetPosition(i, worldPoint);
        }
    }
}
