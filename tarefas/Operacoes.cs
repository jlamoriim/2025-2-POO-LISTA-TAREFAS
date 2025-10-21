using MySql.Data.MySqlClient;

public class Operacoes
{
    private string connectionString = 
    @"server=phpmyadmin.uni9.marize.us;User ID=user_poo;password=S3nh4!F0rt3;database=user_poo;";
    public int Criar(Tarefa tarefa)
    {
        using(var conexao = new MySqlConnection(connectionString))
        {
            conexao.Open();
            string sql = @"INSERT INTO tarefa (nome, descricao, dataCriacao, status, dataExecucao) 
                           VALUES (@nome, @descricao, @dataCriacao, @status, @dataExecucao);
                           SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@nome", tarefa.Nome);
                cmd.Parameters.AddWithValue("@descricao", tarefa.Descricao);
                cmd.Parameters.AddWithValue("@dataCriacao", tarefa.DataCriacao);
                cmd.Parameters.AddWithValue("@status", tarefa.Status);
                cmd.Parameters.AddWithValue("@dataExecucao", tarefa.DataExecucao);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    public Tarefa Buscar(int id)
    {
        return null;
    }

    public IList<Tarefa> Listar()
    {
        using (var conexao = new MySqlConnection(connectionString))
    }
    var sql = "SELECT id, nome, descricao, dataCriacao, dataExecucao, status FROM 'tarefas'";
    conexao.Open();

    using (var cmd = new MaySqlCommand(sql, conexao))
    using (var reader = cmd.ExecuteReader())
    {
        while (reader.Read())
        {
            var tarefa = new tarefa 
            {
                id = reader.GetInt32("id"),
                Nome = reader.GetString("Nome"),
                Descricao = reader.GetString("Descricao"),
                DataCriacao = reader.GetDateTime("DataCriacao"),
                DataExecucao = reader.IsDBNull(reader.GetOrdinal("DataExecucao"))
                           ? (DataTime?) null
                           : reader.GetDateTime("DataExecucao"),
                Status = reader.GetInt32("status")
            };

        tarefas.Add(tarefa);
        }
        return Array.Empty<Tarefa>();
    }

    public void Alterar(Tarefa tarefa)
    {

    }

    public void Excluir(int id)
    {

    }
}