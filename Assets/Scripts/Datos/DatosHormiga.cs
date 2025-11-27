using UnityEngine;

public class DatosHormiga : BaseDatos<InformacionHormiga, UI_InfomacionHormiga>
{
    // Variable estática compartida por todas las hormigas
    private static int ultimoID = 0;

    private void Awake()
    {
        // Generar SIEMPRE un nuevo ID único
        ultimoID++;

        InformacionHormiga info = informacion;
        info.ID = ultimoID;
        informacion = info;
    }


    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        Debug.Log("Hormiga seleccionada: ejecutando lógica adicional...");
    }
}
