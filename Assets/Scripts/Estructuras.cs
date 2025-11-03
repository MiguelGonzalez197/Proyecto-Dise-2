
[System.Serializable]
public struct InformacionHormiga
{
    public int ID;
    public float vidaActual;
    public EstadosSalud estadoActual;
    public Rol rolActual;
}

[System.Serializable]
public struct InformacionHormonas
{
    public int TiempoEvaporacion;
    public TipoHormonas Tipo;
}