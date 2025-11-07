using TMPro;
using UnityEngine;

public class UI_InfomacionHormona : BaseUI<InformacionHormonas>
{
    private DatosHormonas hormonaSeleccionada;

    [SerializeField]
    private TextMeshProUGUI textoTiempo;

    [SerializeField]
    private TextMeshProUGUI textoTipo;

    public override void MostrarInformacion(InformacionHormonas datos)
    {
        if (!ExisteCanva()) return;
        int segundos = Mathf.CeilToInt(datos.TiempoEvaporacion);
        textoTiempo.text = "Tiempo en evaporar: " + segundos + "s";
        textoTipo.text = "Tipo de hormona: " + (datos.Tipo).ToString();
    }
    public void SeleccionarHormona(DatosHormonas h)
    {
        hormonaSeleccionada = h;
    }
    private void Update()
    {
        // ✅ Si hay hormona seleccionada, actualiza su UI en tiempo real
        if (hormonaSeleccionada != null)
        {
            MostrarInformacion(hormonaSeleccionada.GetInfo());

            // Si ya se destruyó o evaporó, limpiar la selección
            if (hormonaSeleccionada == null || hormonaSeleccionada.gameObject == null)
            {
                hormonaSeleccionada = null;
            }

        }
    }

    protected override bool ExisteCanva()
    {
        return textoTiempo != null && textoTipo != null;
    }

}