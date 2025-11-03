using TMPro;
using UnityEngine;

public class UI_InfomacionHormiga : BaseUI<InformacionHormiga>
{

    [SerializeField]
    private TextMeshProUGUI textoID;

    [SerializeField]
    private TextMeshProUGUI textoVida;

    [SerializeField]
    private TextMeshProUGUI textoEstadoActual;

    [SerializeField]
    private TextMeshProUGUI textoOcupacionActual;

    public override void MostrarInformacion(InformacionHormiga datos)
    {
        if (!ExisteCanva()) return;
        textoID.text = "ID: " + (datos.ID).ToString();
        textoVida.text = "Vida: " + (datos.vidaActual).ToString();
        textoEstadoActual.text = "Estado: " + (datos.estadoActual).ToString();
        textoOcupacionActual.text = "Rol: " + (datos.rolActual).ToString();
    }

    protected override bool ExisteCanva()
    { 
        return textoID != null && textoVida != null && textoEstadoActual != null && textoOcupacionActual != null;
    }
}
