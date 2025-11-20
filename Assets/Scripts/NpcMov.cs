using UnityEngine;

public class NpcMov : MonoBehaviour
{
    public float velocidad = 2f;
    public float radio = 2f;          // radio del círculo
    public float distancia = 3f;      // distancia del círculo al frente del NPC
    public float cambioDireccion = 30f;
    public int Limitex = 10, Limitey = 10;// cuánto cambia el ángulo en cada frame

    private bool llevandoComida = false;
    private Vector3 destinoHormiguero;
    private float tiempoDentro = 2f; // segundos dentro del hormiguero
    private bool dentroHormiguero = false;

    private float angulo = 0f;
    private DatosHormiga datos;
    private SpriteRenderer sr; // Para cambiar el color

    void Awake()
    {
        datos = GetComponent<DatosHormiga>(); // Obtiene la referencia al script de datos
        sr = GetComponent<SpriteRenderer>();  // Obtener SpriteRenderer para cambiar color
    }

    void Update()
    {
        if (dentroHormiguero)
        {
            return; // no se mueve mientras está dentro
        }

        if (llevandoComida)
        {
            VolverAlHormiguero();
            return;
        }

        // Nuevo: detectar comida primero
        DetectarComida();

        InformacionHormiga info = datos.GetInfo();

        // Si la hormiga está muerta, no se mueve y cambia color
        if (info.vidaActual <= 0)
        {
            // Cambiar color a gris (puedes cambiar el color que quieras)
            if (sr != null)
            {
                sr.color = Color.gray;
            }
            return; // Salir del update para que no se mueva
        }
        else
        {
            // Si la hormiga está viva, color normal (blanco)
            if (sr != null)
            {
                sr.color = Color.white;
            }
        }

        // Movimiento normal
        if (transform.position.x >= Limitex || transform.position.x <= -Limitex || transform.position.y >= Limitey || transform.position.y <= -Limitey)
        {
            angulo += 90f;
        }
        else
        {
            angulo += Random.Range(-cambioDireccion, cambioDireccion) * Time.deltaTime;
        }

        Vector2 circulo = new Vector2(Mathf.Cos(angulo), Mathf.Sin(angulo)) * radio;
        Vector2 objetivo = (Vector2)transform.position + (Vector2)transform.up * distancia + circulo;
        Vector2 direccion = (objetivo - (Vector2)transform.position).normalized;
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
        float anguloRot = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, anguloRot);

        ActualizarEstadoHormiga();
    }

    void ActualizarEstadoHormiga()
    {
        InformacionHormiga info = datos.GetInfo();

        info.vidaActual -= 0.5f * Time.deltaTime;

        if (info.vidaActual > 70) info.estadoActual = EstadosSalud.Saludable;
        else if (info.vidaActual > 40) info.estadoActual = EstadosSalud.Herida;
        else if (info.vidaActual > 0) info.estadoActual = EstadosSalud.Critico;
        else info.estadoActual = EstadosSalud.Muerta;

        datos.SetInfo(info);
    }

    void DetectarComida()
    {
        float radioPercepcion = 1.5f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radioPercepcion);

        foreach (var h in hits)
        {
            DatosComida comida = h.GetComponent<DatosComida>();

            if (comida != null)
            {
                // Se encontró comida
                MoverHaciaComida(comida.gameObject);
                return;
            }
        }
    }

    void MoverHaciaComida(GameObject comida)
    {
        Vector2 direction = (comida.transform.position - transform.position).normalized;
        transform.Translate(direction * velocidad * Time.deltaTime, Space.World);

        float anguloRot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, anguloRot);

        // Si está lo suficientemente cerca → comer
        if (Vector2.Distance(transform.position, comida.transform.position) < 0.3f)
        {
            ConsumirComida(comida);
        }
    }

    void ConsumirComida(GameObject comidaObj)
    {
        DatosComida datosComida = comidaObj.GetComponent<DatosComida>();

        if (datosComida == null) return;

        InformacionComida info = datosComida.GetInfo();

        // Recuperar vida
        InformacionHormiga inf = datos.GetInfo();
        inf.vidaActual = Mathf.Min(100, inf.vidaActual + 20);
        datos.SetInfo(inf);

        // Generar feromona según el tipo de comida
        CrearFeromonaPorComida(info.Tipo, comidaObj.transform.position);

        // Decidir si la hormiga se come la comida o la lleva al hormiguero
        float probabilidadTransportar = 0.7f; // 70% transporte / 30% consumir

        float decision = Random.value;

        if (decision < probabilidadTransportar)
        {
            // TRANSPORTAR AL HORMIGUERO
            llevandoComida = true;
            destinoHormiguero = Hormiguero.PuntoHormiguero.position;
            Debug.Log("La hormiga decidió transportar la comida al hormiguero.");
        }
        else
        {
            // COMER EN EL LUGAR
            inf = datos.GetInfo();   // sin "InformacionHormiga"
            inf.vidaActual = Mathf.Min(100, inf.vidaActual + 25); // más recarga si come in situ
            datos.SetInfo(inf);

            Debug.Log("La hormiga decidió COMER la comida en el lugar.");
        }

        // Eliminar comida
        Destroy(comidaObj);
    }

    void CrearFeromonaPorComida(TipoComida tipo, Vector3 pos)
    {
        TipoHormonas hormona = TipoHormonas.Normal;

        switch (tipo)
        {
            case TipoComida.Semilla:
            case TipoComida.Azucar:
            case TipoComida.Fruta:
            case TipoComida.Insecto:
            case TipoComida.Carne:
            case TipoComida.Hoja:
                hormona = TipoHormonas.Comida;
                break;

            //case TipoComida.Insecto:
            //case TipoComida.Carne:
            //    hormona = TipoHormonas.Peligro;
            //    break;

            //case TipoComida.Hoja:
            //    hormona = TipoHormonas.Normal;
            //    break;
        }

        // Crear objeto feromona mediante tu prefab
        GameObject prefab = Resources.Load<GameObject>("PreFab/PrefabHormona");
        GameObject fer = Instantiate(prefab, pos, Quaternion.identity);

        InformacionHormonas data = new InformacionHormonas
        {
            Tipo = hormona,
            TiempoEvaporacion = 10
        };

        fer.GetComponent<DatosHormonas>().SetInfo(data);
        fer.GetComponent<Hormona>().datos = data;
    }

    void VolverAlHormiguero()
    {
        Vector2 direction = (destinoHormiguero - transform.position).normalized;
        transform.Translate(direction * velocidad * Time.deltaTime, Space.World);

        float anguloRot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, anguloRot);

        // Si llega al hormiguero
        if (Vector2.Distance(transform.position, destinoHormiguero) < 0.4f)
        {
            StartCoroutine(EntrarAlHormiguero());
        }
    }
    System.Collections.IEnumerator EntrarAlHormiguero()
    {
        dentroHormiguero = true;

        // “Entrar”: hacer invisible
        if (sr != null)
            sr.enabled = false;

        yield return new WaitForSeconds(2f); // tiempo dentro

        // Salir
        dentroHormiguero = false;
        llevandoComida = false;

        if (sr != null)
            sr.enabled = true;

        // Al salir, la hormiga rota hacia afuera
        angulo = Random.Range(0f, 360f);
    }



}
