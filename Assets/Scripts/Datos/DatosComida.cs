using UnityEngine;

public class DatosComida : BaseDatos<InformacionComida, UI_InformacionComida>
{
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        Debug.Log("Comida seleccionada: ejecutando lógica adicional...");
    }

    private void Update()
    {
        // Si el tiempo es infinito (-1), no desaparece
        if (informacion.TiempoEnDesaparecer < 0)
            return; // Comida infinita

        informacion.TiempoEnDesaparecer -= Time.deltaTime;

        if (informacion.TiempoEnDesaparecer <= 0)
            Destroy(gameObject);
        // Ya no reducimos el tiempo
        //informacion.TiempoEnDesaparecer -= Time.deltaTime; 
    }
}
