Processar();

static void Processar()
{
    Grafo grafo = ObterGrafo();

    var caminho = ObterCaminho();
    Console.WriteLine($"O caminho é: {caminho}");

    VerificarCaminho(grafo, caminho);
}

static string ObterCaminho()
{
    Console.WriteLine("Informe o caminho separando os vertices por virgula: ");
    var caminho = Console.ReadLine();

    if (string.IsNullOrEmpty(caminho))
        throw new Exception("Caminho inválido.");
    
    return caminho.ToUpper();
}

static Grafo ObterGrafo()
{
    var verticeA = new Vertice("A");
    var verticeB = new Vertice("B");
    var verticeC = new Vertice("C");
    var verticeD = new Vertice("D");
    var verticeE = new Vertice("E");
    var verticeF = new Vertice("F");

    var relacoes = new List<Aresta>
    {
        new Aresta("1", verticeB, verticeA),
        new Aresta("2", verticeB, verticeC),
        new Aresta("3", verticeD, verticeF),
        new Aresta("4", verticeE, verticeF),
        new Aresta("5", verticeA, verticeE),
        new Aresta("6", verticeA, verticeC),
        new Aresta("7", verticeC, verticeF),
        new Aresta("8", verticeC, verticeB)
    };

    var grafo = new Grafo(direcionado: true, relacoes);
    grafo.PopularAdjacencias();
    
    return grafo;
}

static void VerificarCaminho(Grafo grafo, string caminho)
{
    var listaVertices = caminho.Split(",");
    var pontoInicial = listaVertices.First();

    var pontoInicialExiste = grafo.Relacoes.Exists(a => a.Vertice.Id == pontoInicial || 
                                                        a.VerticeAdjacente.Id == pontoInicial);
    if (!pontoInicialExiste)
        throw new Exception("Caminho não existe");

    var caminhoExiste = false;

    for (int i = 0; i < listaVertices.Length; i++)
    {
        if (i <= listaVertices.Length - 2)
            caminhoExiste = grafo.Relacoes.Exists(a => a.Vertice.Id == listaVertices[i] &&
                                                       a.VerticeAdjacente.Id == listaVertices[i + 1]);
        else
            caminhoExiste = grafo.Relacoes.Exists(a => a.Vertice.Id == listaVertices[i - 1] &&
                                                       a.VerticeAdjacente.Id == listaVertices[i]);

        if (caminhoExiste is false)
            throw new Exception("Caminho não Existe");
    }

    Console.WriteLine("Caminho Existe");
}

public class Grafo
{
    public Grafo(bool direcionado, List<Aresta> relacoes)
    {
        Direcionado = direcionado;
        Relacoes = relacoes;
    }

    public bool Direcionado { get; set; }
    public List<Aresta> Relacoes { get; set; }

    public void PopularAdjacencias()
    {
        foreach(var aresta in Relacoes)
        {
            if (Direcionado)
                aresta.Vertice.AdicionarVerticeAdjacente(aresta.VerticeAdjacente);
            else
            {
                aresta.Vertice.AdicionarVerticeAdjacente(aresta.VerticeAdjacente);
                aresta.VerticeAdjacente.AdicionarVerticeAdjacente(aresta.Vertice);
            }
        }
    }
}

public class Aresta
{
    public Aresta(dynamic id, Vertice vertice, Vertice verticeAdjacente)
    {
        Id = id;
        Vertice = vertice;
        VerticeAdjacente = verticeAdjacente;
    }

    public dynamic Id { get; set; }
    public Vertice Vertice { get; set; }
    public Vertice VerticeAdjacente { get; set; }
}

public class Vertice
{
    public Vertice(dynamic id)
    {
        Id = id;
        Adjacentes = new List<string>();
    }

    public dynamic Id { get; set; }
    public List<string> Adjacentes { get; set; }

    public void AdicionarVerticeAdjacente(Vertice vertice)
        => Adjacentes.Add(vertice.Id);
}
