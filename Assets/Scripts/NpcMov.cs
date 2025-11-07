using UnityEngine;

public class NpcMov : MonoBehaviour
{
    public float velocidad = 2f;
    public float radio = 2f;          // radio del círculo
    public float distancia = 3f;      // distancia del círculo al frente del NPC
    public float cambioDireccion = 30f;
    public int Limitex = 10, Limitey = 10;// cuánto cambia el ángulo en cada frame

    private float angulo = 0f;
    private DatosHormiga datos;
    private SpriteRenderer sr; // Para cambiar el color

    void Awake()
    {
        datos = GetComponent<DatosHormiga>(); // Obtiene la referencia al script de datos
        sr = GetComponent<SpriteRenderer>();  // Obtener SpriteRenderer para cambiar color
    }

    void Update()
    {
        InformacionHormiga info = datos.GetInfo();

        // Si la hormiga está muerta, no se mueve y cambia color
        if (info.vidaActual <= 0)
        {
            // Cambiar color a gris (puedes cambiar el color que quieras)
            if (sr != null)
            {
                sr.color = Color.gray;
            }
            return; // Salir del update para que no se mueva
        }
        else
        {
            // Si la hormiga está viva, color normal (blanco)
            if (sr != null)
            {
                sr.color = Color.white;
            }
        }

        // Movimiento normal
        if (transform.position.x >= Limitex || transform.position.x <= -Limitex || transform.position.y >= Limitey || transform.position.y <= -Limitey)
        {
            angulo += 90f;
        }
        else
        {
            angulo += Random.Range(-cambioDireccion, cambioDireccion) * Time.deltaTime;
        }

        Vector2 circulo = new Vector2(Mathf.Cos(angulo), Mathf.Sin(angulo)) * radio;
        Vector2 objetivo = (Vector2)transform.position + (Vector2)transform.up * distancia + circulo;
        Vector2 direccion = (objetivo - (Vector2)transform.position).normalized;
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
        float anguloRot = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, anguloRot);

        ActualizarEstadoHormiga();
    }

    void ActualizarEstadoHormiga()
    {
        InformacionHormiga info = datos.GetInfo();

        info.vidaActual -= 0.5f * Time.deltaTime;

        if (info.vidaActual > 70) info.estadoActual = EstadosSalud.Saludable;
        else if (info.vidaActual > 40) info.estadoActual = EstadosSalud.Herida;
        else if (info.vidaActual > 0) info.estadoActual = EstadosSalud.Critico;
        else info.estadoActual = EstadosSalud.Muerta;

        datos.SetInfo(info);
    }
}
