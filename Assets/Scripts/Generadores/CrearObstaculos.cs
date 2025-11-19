using UnityEngine;

public class CrearObstaculos : MonoBehaviour
{
    public GameObject prefabObstaculo;

    private bool drawing = false;
    private Vector3 startPoint;
    private GameObject preview;

    void Update()
    {
        // Inicio del dibujo (clic izquierdo)
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 0;
            drawing = true;

            // Crear preview
            preview = Instantiate(prefabObstaculo, startPoint, Quaternion.identity);
        }

        // Mientras arrastras el mouse
        if (drawing && Input.GetMouseButton(0))
        {
            Vector3 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 0;

            Vector3 center = (startPoint + currentPoint) / 2f;
            Vector3 size = new Vector3(
                Mathf.Abs(startPoint.x - currentPoint.x),
                Mathf.Abs(startPoint.y - currentPoint.y),
                1
            );

            preview.transform.position = center;
            preview.transform.localScale = size;
        }

        // Sueltas el clic → obstáculo finalizado
        if (drawing && Input.GetMouseButtonUp(0))
        {
            drawing = false;
            preview = null;
        }

        // Clic derecho → eliminar obstáculo
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null)
                Destroy(hit.collider.gameObject);
        }
    }
}
