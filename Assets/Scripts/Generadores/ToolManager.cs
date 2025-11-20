using UnityEngine;

public enum ModoHerramienta { Obstaculo, Comida }

public class ToolManager : MonoBehaviour
{
    public ModoHerramienta modoActual = ModoHerramienta.Obstaculo;

    public CrearObstaculos crearObstaculos;
    public CrearComidaEnClick crearComida;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            modoActual = ModoHerramienta.Obstaculo;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            modoActual = ModoHerramienta.Comida;

        // Activar solo lo que corresponde
        crearObstaculos.enabled = (modoActual == ModoHerramienta.Obstaculo);
        crearComida.enabled = (modoActual == ModoHerramienta.Comida);
    }
}
