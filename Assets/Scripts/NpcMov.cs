using UnityEngine;

public class NpcMov : MonoBehaviour
{
    public float velocidad = 2f;
    public float radio = 2f;          // radio del círculo
    public float distancia = 3f;      // distancia del círculo al frente del NPC
    public float cambioDireccion = 30f;
    public int Limitex = 10, Limitey = 10;// cuánto cambia el ángulo en cada frame

    private float angulo = 0f;

    void Update()
    {

        if (transform.position.x >= Limitex || transform.position.x <= -Limitex ||  transform.position.y >= Limitey || transform.position.y <= -Limitey)
        {
            angulo +=90f;
        }
        else
        {
            // Elegir un pequeño cambio aleatorio en el ángulo
            angulo += Random.Range(-cambioDireccion, cambioDireccion) * Time.deltaTime;
        }

        // Calcular el desplazamiento del punto objetivo en el círculo
        Vector2 circulo = new Vector2(Mathf.Cos(angulo), Mathf.Sin(angulo)) * radio;

        // Calcular la posición hacia la que moverse
        Vector2 objetivo = (Vector2)transform.position + (Vector2)transform.up * distancia + circulo;

        // Mover al NPC suavemente hacia el objetivo
        Vector2 direccion = (objetivo - (Vector2)transform.position).normalized;
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

        // Girar el NPC hacia la dirección actual
        float anguloRot = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, anguloRot);
 
    }
}
