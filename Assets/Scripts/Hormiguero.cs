using UnityEngine;

public class Hormiguero : MonoBehaviour
{
    public static Transform PuntoHormiguero;

    private void Awake()
    {
        PuntoHormiguero = this.transform; // Punto central del hormiguero
    }
}
