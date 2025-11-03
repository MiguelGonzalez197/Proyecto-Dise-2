using UnityEngine;

public abstract class BaseDatos<TDatos, TUI> : MonoBehaviour
    where TUI : BaseUI<TDatos>
{
    [SerializeField] 
    private TDatos informacion;
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

        UI.MostrarInformacion(informacion);
        
    }
}
