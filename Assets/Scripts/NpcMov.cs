using System.Collections;
using System.Collections.Generic;
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

    // COOL-DOWN entre hormigas por ID
    private Dictionary<int, float> ultimoEncuentroConHormiga = new Dictionary<int, float>();
    private float tiempoCooldown = 1.5f; // segundos que deben esperar para reencontrarse

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

        //Encuentro con hormigas
        //DetectarHormiga();
        DetectarEncuentroConHormiga();

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

    bool EsComidaGrande(TipoComida tipo)
    {
        switch (tipo)
        {
            case TipoComida.Carne:
            case TipoComida.Insecto:
            case TipoComida.Semilla:
                return true;  // Comidas grandes
        }
        return false; // Las demás son pequeñas
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

        //// Decidir si la hormiga se come la comida o la lleva al hormiguero
        //float probabilidadTransportar = 0.7f; // 70% transporte / 30% consumir

        //float decision = Random.value;

        //if (decision < probabilidadTransportar)
        //{
        //    // TRANSPORTAR AL HORMIGUERO
        //    llevandoComida = true;
        //    destinoHormiguero = Hormiguero.PuntoHormiguero.position;
        //    Debug.Log("La hormiga decidió transportar la comida al hormiguero.");
        //}
        //else
        //{
        //    // COMER EN EL LUGAR
        //    inf = datos.GetInfo();   // sin "InformacionHormiga"
        //    inf.vidaActual = Mathf.Min(100, inf.vidaActual + 25); // más recarga si come in situ
        //    datos.SetInfo(inf);

        //    Debug.Log("La hormiga decidió COMER la comida en el lugar.");
        //}

        // Si la comida es grande → siempre transportar
        if (EsComidaGrande(info.Tipo))
        {
            llevandoComida = true;
            destinoHormiguero = Hormiguero.PuntoHormiguero.position;
            Debug.Log("Comida grande encontrada. La hormiga la lleva al hormiguero.");
        }
        else
        {
            // Comida pequeña → decisión probabilística
            float decision = Random.value;
            float probabilidadTransportar = 0.3f; // 30% transporta / 70% comer en el sitio

            if (decision < probabilidadTransportar)
            {
                llevandoComida = true;
                destinoHormiguero = Hormiguero.PuntoHormiguero.position;
                Debug.Log("Comida pequeña, pero la hormiga decidió transportarla.");
            }
            else
            {
                // COMER EN EL SITIO
                InformacionHormiga inf2 = datos.GetInfo();
                inf2.vidaActual = Mathf.Min(100, inf2.vidaActual + 25);
                datos.SetInfo(inf2);

                Debug.Log("Comida pequeña. La hormiga decidió comerla en el lugar.");
            }
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

    void DetectarHormiga()
    {
        float radioEncuentro = 0.5f; // Distancia muy cercana
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radioEncuentro);

        foreach (var h in hits)
        {
            // Evitar detectarse a sí misma
            if (h.gameObject == this.gameObject)
                continue;

            // Si es otra hormiga
            DatosHormiga otraHormiga = h.GetComponent<DatosHormiga>();
            if (otraHormiga != null)
            {
                StartCoroutine(InteraccionHormiga());
                return;
            }
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

    System.Collections.IEnumerator InteraccionHormiga()
    {
        // Pequeña pausa como si se olieran
        float pausa = Random.Range(0.2f, 0.4f);
        float nuevaDireccion = Random.Range(45f, 135f);

        // Detener movimiento
        float velocidadOriginal = velocidad;
        velocidad = 0;

        yield return new WaitForSeconds(pausa);

        // Cambiar ángulo (hormiga gira)
        angulo += nuevaDireccion * Mathf.Deg2Rad;

        // Restaurar velocidad
        velocidad = velocidadOriginal;
    }

    bool PuedeInteractuar(int idHormiga)
    {
        if (!ultimoEncuentroConHormiga.ContainsKey(idHormiga))
            return true;

        return Time.time - ultimoEncuentroConHormiga[idHormiga] >= tiempoCooldown;
    }

    void RegistrarEncuentro(int idHormiga)
    {
        ultimoEncuentroConHormiga[idHormiga] = Time.time;
    }

    void DetectarEncuentroConHormiga()
    {
        float radioEncuentro = 0.6f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radioEncuentro);

        foreach (var h in hits)
        {
            if (h.gameObject == this.gameObject)
                continue;

            DatosHormiga otro = h.GetComponent<DatosHormiga>();
            if (otro == null) continue;

            int idOtro = otro.GetInfo().ID;

            // Si no puedo interactuar todavía
            if (!PuedeInteractuar(idOtro))
                continue;

            RegistrarEncuentro(idOtro);

            IntercambiarInformacion(otro);

            // Pausa corta como si se olieran
            StartCoroutine(PausaEncuentro());

            SepararseDeOtraHormiga(h.transform);
        }
    }

    IEnumerator PausaEncuentro()
    {
        float pausa = Random.Range(0.2f, 0.4f);

        float velOriginal = velocidad;
        velocidad = 0;

        yield return new WaitForSeconds(pausa);

        velocidad = velOriginal;
    }

    void IntercambiarInformacion(DatosHormiga otraHormiga)
    {
        InformacionHormiga miInfo = datos.GetInfo();
        InformacionHormiga infoOtra = otraHormiga.GetInfo();

        Debug.Log($"🐜 Hormiga {miInfo.ID} se encontró con Hormiga {infoOtra.ID}");

        // 1. Compartir rumor de comida (si una lleva comida, la otra aprende)
        if (llevandoComida)
        {
            Debug.Log($"🐜 Hormiga {miInfo.ID} informa de comida a hormiga {infoOtra.ID}");
            // Aquí puedes guardar en la otra hormiga posición de comida
        }

        // 2. Compartir estado de vida (trofalaxis ligera)
        if (miInfo.vidaActual < 40 && infoOtra.vidaActual > 60)
        {
            float intercambio = 5f;
            miInfo.vidaActual += intercambio;
            infoOtra.vidaActual -= intercambio;

            datos.SetInfo(miInfo);
            otraHormiga.SetInfo(infoOtra);

            Debug.Log($"🤝 Trofalaxis: {infoOtra.ID} da vida a {miInfo.ID}");
        }

        // 3. Advertencia de peligro (si alguna olió una feromona de peligro)
        // Puedes agregar una variable booleana "vioPeligro"
        // Y la compartes aquí

        // 4. Compartir rol
        if (miInfo.rolActual != infoOtra.rolActual)
        {
            Debug.Log($"📡 {miInfo.ID} y {infoOtra.ID} comparten rol: {miInfo.rolActual} ↔ {infoOtra.rolActual}");
            // Se podrían reasignar roles dinámicamente
        }
    }

    void SepararseDeOtraHormiga(Transform otra)
    {
        Vector2 direccion = (transform.position - otra.position).normalized;
        float fuerza = 1.2f;

        transform.Translate(direccion * fuerza * Time.deltaTime, Space.World);
    }

}
