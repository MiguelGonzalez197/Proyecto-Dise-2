using UnityEngine;

public class DatosHormiga : BaseDatos<InformacionHormiga, UI_InfomacionHormiga>
{
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        Debug.Log("Hormiga seleccionada: ejecutando lógica adicional...");
    }
}
