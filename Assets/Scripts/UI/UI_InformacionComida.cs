using TMPro;
using UnityEngine;

public class UI_InformacionComida : BaseUI<InformacionComida>
{
    private DatosComida comidaSeleccionada;

    [SerializeField]
    private TextMeshProUGUI textoTiempo;

    [SerializeField]
    private TextMeshProUGUI textoTipo;

    public override void MostrarInformacion(InformacionComida datos)
    {
        if (!ExisteCanva()) return;
        textoTiempo.text = "Tiempo en desaparecer: " + datos.TiempoEnDesaparecer + "s";
        textoTipo.text = "Tipo de comida: " + (datos.Tipo).ToString();
    }


    protected override bool ExisteCanva()
    {
        return textoTiempo != null && textoTipo != null;
    }
}
