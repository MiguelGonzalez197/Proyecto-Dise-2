using UnityEngine;

public class GeneradorHormona : MonoBehaviour
{
    public GameObject prefabHormona; // Asignar en inspector
    public float intervalo = 2f;     // Tiempo entre hormonas
    public TipoHormonas tipoHormona = TipoHormonas.Comida;

    private float contador = 0f;

    void Update()
    {
        contador += Time.deltaTime;

        if (contador >= intervalo)
        {
            contador = 0f;

            GenerarHormona();
        }
    }

    void GenerarHormona()
    {
        InformacionHormonas datos = new InformacionHormonas
        {
            TiempoEvaporacion = 10, // segundos
            Tipo = tipoHormona
        };
        GameObject h = Instantiate(prefabHormona, transform.position, Quaternion.identity);

        Hormona compHormona = h.GetComponent<Hormona>();
        DatosHormonas compDatos = h.GetComponent<DatosHormonas>();

        compHormona.datos = datos;
        compDatos.SetInfo(datos);
    }
}
