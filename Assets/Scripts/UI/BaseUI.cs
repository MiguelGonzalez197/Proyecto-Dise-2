using UnityEngine;

public abstract class BaseUI<T> : MonoBehaviour
{
    public virtual void MostrarInformacion(T datos)
    {
        
        Debug.Log("Existe Canva");
    }

    protected virtual bool ExisteCanva()
    {
        return false;
    }
}
