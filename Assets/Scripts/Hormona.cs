using UnityEngine;

public class Hormona : MonoBehaviour
{
    public InformacionHormonas datos;
    private SpriteRenderer sr;

    private DatosHormonas datosHormonas;  // Referencia a DatosHormonas

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        datosHormonas = GetComponent<DatosHormonas>();
    }

    private void Update()
    {
        datos.TiempoEvaporacion -= Time.deltaTime;

        // Sincronizar el dato con DatosHormonas para que UI pueda actualizarse
        if (datosHormonas != null)
        {
            datosHormonas.SetInfo(datos);
        }

        // Desvanecer visualmente
        float alpha = Mathf.Clamp01(datos.TiempoEvaporacion / 10f);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        // Destruir al evaporarse
        if (datos.TiempoEvaporacion <= 0)
            Destroy(gameObject);
    }
    // resto igual
}
