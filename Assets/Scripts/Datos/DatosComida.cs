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
        informacion.TiempoEnDesaparecer -= Time.deltaTime; 
    }
}
