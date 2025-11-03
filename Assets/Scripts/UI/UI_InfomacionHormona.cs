using TMPro;
using UnityEngine;

public class UI_InfomacionHormona : BaseUI<InformacionHormonas>
{
    [SerializeField]
    private TextMeshProUGUI textoTiempo;

    [SerializeField]
    private TextMeshProUGUI textoTipo;

    public override void MostrarInformacion(InformacionHormonas datos)
    {
        if (!ExisteCanva()) return;
        textoTiempo.text = "Tiempo en evaporar: " + (datos.TiempoEvaporacion).ToString() + "s";
        textoTipo.text = "Tipo de hormona: " + (datos.Tipo).ToString();
    }

    protected override bool ExisteCanva()
    {
        return textoTiempo != null && textoTipo != null;
    }

}