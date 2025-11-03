using UnityEngine;

public class DatosHormonas : BaseDatos<InformacionHormonas, UI_InfomacionHormona>
{
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        Debug.Log("Hormona seleccionada: ejecutando lógica adicional...");
    }
}