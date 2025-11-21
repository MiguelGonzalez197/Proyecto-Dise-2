using UnityEngine;

public abstract class BaseDatos<TDatos, TUI> : MonoBehaviour
    where TUI : BaseUI<TDatos>
{
    [SerializeField] 
    protected TDatos informacion;
    private TUI UI;

    private void Start()
    {
        UI = FindFirstObjectByType<TUI>();

        if (UI == null)
            Debug.LogError($"❌ No hay UI {typeof(TUI).Name} en la escena.");
    }

    protected virtual void OnMouseDown()
    {
        if (UI == null)
        {
            Debug.LogWarning($"No se encontró un {typeof(TUI).Name} en escena");
            return;
        }
        if (UI is UI_InfomacionHormiga uiHormiga && this is DatosHormiga datosHormiga)
            uiHormiga.SeleccionarHormiga(datosHormiga);
        if (UI is UI_InfomacionHormona uiHormona && this is DatosHormonas datosHormona)
            uiHormona.SeleccionarHormona(datosHormona);

        UI.MostrarInformacion(informacion);

        //PRUEBAAAAAA
        Debug.Log("SE HIZO CLICK EN: " + gameObject.name);
        // Solo hormigas tienen ID
        if (informacion is InformacionHormiga infoHormiga)
        {
            Debug.Log("INFO ENCONTRADA: " + infoHormiga.ID);
        }
        else
        {
            Debug.Log("INFO ENCONTRADA: (sin ID)");
        }
        Debug.Log("UI ENCONTRADA: " + UI);


    }
    public TDatos GetInfo() => informacion;
    public void SetInfo(TDatos nuevaInfo) => informacion = nuevaInfo;
}
