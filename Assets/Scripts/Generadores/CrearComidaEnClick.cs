using UnityEngine;

public class CrearComidaEnClick : MonoBehaviour
{
    public GameObject[] prefabsComida;

    void Update()
    {
        // Crear comida con clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            CrearComida();
        }

        // Eliminar comida con clic derecho
        if (Input.GetMouseButtonDown(1))
        {
            EliminarComida();
        }
    }

    void CrearComida()
    {
        if (prefabsComida == null || prefabsComida.Length == 0)
        {
            Debug.LogWarning("No hay prefabs de comida asignados en CrearComidaEnClick.");
            return;
        }

        // Posición del mouse → mundo
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        // Elegir un prefab aleatorio
        GameObject prefab = prefabsComida[Random.Range(0, prefabsComida.Length)];

        // Instanciar comida
        GameObject comida = Instantiate(prefab, pos, Quaternion.identity);

        // Asignar información interna (igual a tu generador normal)
        DatosComida datos = comida.GetComponent<DatosComida>();

        if (datos != null)
        {
            InformacionComida info = new InformacionComida
            {
                Tipo = ObtenerTipoAleatorio(),
                TiempoEnDesaparecer = Random.Range(5f, 20f)
            };

            datos.SetInfo(info);

            // Destruir luego del tiempo asignado
            Destroy(comida, info.TiempoEnDesaparecer);
        }
    }

    void EliminarComida()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Raycast para detectar comida bajo el cursor
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

        if (hit.collider != null)
        {
            // Solo eliminar si lo que golpeamos tiene DatosComida
            if (hit.collider.GetComponent<DatosComida>() != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private TipoComida ObtenerTipoAleatorio()
    {
        int inicio = 1;
        int fin = System.Enum.GetValues(typeof(TipoComida)).Length;
        return (TipoComida)Random.Range(inicio, fin);
    }
}
