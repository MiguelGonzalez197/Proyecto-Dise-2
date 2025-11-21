using UnityEngine;

public enum ModoHerramienta { Libre, Obstaculo, Comida }

public class ToolManager : MonoBehaviour
{
    public ModoHerramienta modoActual = ModoHerramienta.Libre;

    public CrearObstaculos crearObstaculos;
    public CrearComidaEnClick crearComida;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            modoActual = ModoHerramienta.Libre;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            modoActual = ModoHerramienta.Obstaculo;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            modoActual = ModoHerramienta.Comida;

        // Activar solo lo que corresponde
        crearObstaculos.enabled = (modoActual == ModoHerramienta.Obstaculo);
        crearComida.enabled = (modoActual == ModoHerramienta.Comida);
    }
}
