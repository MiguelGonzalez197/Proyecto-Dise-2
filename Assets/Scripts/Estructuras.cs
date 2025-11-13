
[System.Serializable]
public struct InformacionHormiga
{
    public int ID;
    public float vidaActual;
    public EstadosSalud estadoActual;
    public Rol rolActual;
}

[System.Serializable]
public class InformacionHormonas
{
    public float TiempoEvaporacion;
    public TipoHormonas Tipo;
}

[System.Serializable]
public struct InformacionComida
{
    public float TiempoEnDesaparecer;
    public TipoComida Tipo;
}