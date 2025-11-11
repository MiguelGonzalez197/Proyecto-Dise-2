using UnityEngine;

public abstract class BaseDatos<TDatos, TUI> : MonoBehaviour
    where TUI : BaseUI<TDatos>
{
    [SerializeField] 
    protected TDatos informacion;
    private TUI UI;

    private void Awake()
    {
        UI = FindFirstObjectByType<TUI>();
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
        
    }
    public TDatos GetInfo() => informacion;
    public void SetInfo(TDatos nuevaInfo) => informacion = nuevaInfo;
}
