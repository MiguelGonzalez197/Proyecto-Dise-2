using UnityEngine;


public class GeneradorComida : MonoBehaviour
{
    [Header("Configuración General")]
    [SerializeField] private GameObject[] prefabsComida; // Array de prefabs posibles
    [SerializeField] private int cantidadComida = 2;
    [SerializeField] private Vector2 areaGeneracion = new Vector2(10, 10);

    [Header("Configuración de Atributos Aleatorios")]
    [SerializeField] private Vector2 rangoTiempoDesaparicion = new Vector2(5f, 20f);

    [Header("Debug")]
    [SerializeField] private bool mostrarArea = true;
    [SerializeField] private Color colorArea = new Color(0, 1, 0, 0.2f);

    private void Start()
    {
        GenerarComida();
    }

    public void GenerarComida()
    {
        if (prefabsComida == null || prefabsComida.Length == 0)
        {
            Debug.LogWarning("No hay prefabs de comida asignados al generador.");
            return;
        }

        for (int i = 0; i < cantidadComida; i++)
        {
            // Elegir un prefab aleatorio
            GameObject prefab = prefabsComida[Random.Range(0, prefabsComida.Length)];

            Vector3 pos = new Vector3(
                transform.position.x + Random.Range(-areaGeneracion.x / 2, areaGeneracion.x / 2),
                transform.position.y + Random.Range(-areaGeneracion.y / 2, areaGeneracion.y / 2),
                transform.position.z
            );


            // Instanciar
            GameObject comidaObj = Instantiate(prefab, pos, Quaternion.identity);

            // Obtener su script DatosComida
            DatosComida datos = comidaObj.GetComponent<DatosComida>();
            if (datos != null)
            {
                // Crear información aleatoria
                InformacionComida info = new InformacionComida
                {
                    Tipo = ObtenerTipoAleatorio(),
                    TiempoEnDesaparecer = Random.Range(rangoTiempoDesaparicion.x, rangoTiempoDesaparicion.y)
                };

                // Aplicar
                datos.SetInfo(info);

                // Opcional: destruir tras cierto tiempo
                Destroy(comidaObj, info.TiempoEnDesaparecer);
            }
            else
            {
                Debug.LogWarning($"El prefab {prefab.name} no tiene componente DatosComida.");
            }
        }
    }

    private TipoComida ObtenerTipoAleatorio()
    {
        int inicio = 1;
        int fin = System.Enum.GetValues(typeof(TipoComida)).Length;
        return (TipoComida)Random.Range(inicio, fin);
    }

    private void OnDrawGizmosSelected()
    {
        if (mostrarArea)
        {
            Gizmos.color = colorArea;
            Gizmos.DrawCube(transform.position, new Vector3(areaGeneracion.x, areaGeneracion.y, 0.1f));
        }
    }

}
