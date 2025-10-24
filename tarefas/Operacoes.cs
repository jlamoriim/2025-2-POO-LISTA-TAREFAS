using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class Operacoes
{
    private string connectionString =
        @"server=phpmyadmin.uni9.marize.us;User ID=user_poo;password=S3nh4!F0rt3;database=user_poo;";

    public int Criar(Tarefa tarefa)
    {
        using (var conexao = new MySqlConnection(connectionString))
        {
            conexao.Open();
            // Atenção: o SELECT LAST_INSERT_ID() deve ser separado ou usar ExecuteScalar apenas na insert sem ; no meio.
            string sql = @"INSERT INTO tarefa (nome, descricao, dataCriacao, status, dataExecucao) 
                           VALUES (@nome, @descricao, @dataCriacao, @status, @dataExecucao);
                           SELECT LAST_INSERT_ID();";

            // Alternativa melhor: execute insert e depois execute "SELECT LAST_INSERT_ID()" em outro comando.

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@nome", tarefa.Nome);
                cmd.Parameters.AddWithValue("@descricao", tarefa.Descricao);
                cmd.Parameters.AddWithValue("@dataCriacao", tarefa.DataCriacao);
                cmd.Parameters.AddWithValue("@status", tarefa.Status);

                if (tarefa.DataExecucao.HasValue)
                    cmd.Parameters.AddWithValue("@dataExecucao", tarefa.DataExecucao.Value);
                else
                    cmd.Parameters.AddWithValue("@dataExecucao", DBNull.Value);

                // ExecuteScalar com múltiplas queries não funciona bem. Melhor fazer em duas etapas:
                cmd.ExecuteNonQuery();

                // Agora buscar o último ID inserido:
                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                cmd.Parameters.Clear(); // limpar parâmetros para a nova query

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    public Tarefa Buscar(int id)
    {
        using var conexao = new MySqlConnection(connectionString);
        conexao.Open();

        string sql = "SELECT id, nome, descricao, dataCriacao, dataExecucao, status FROM tarefa WHERE id = @id";

        using var cmd = new MySqlCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Tarefa
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Descricao = reader.GetString("descricao"),
                DataCriacao = reader.GetDateTime("dataCriacao"),
                DataExecucao = reader.IsDBNull(reader.GetOrdinal("dataExecucao"))
                                ? (DateTime?)null
                                : reader.GetDateTime("dataExecucao"),
                Status = reader.GetInt32("status")
            };
        }

        return null; // se não encontrar
    }

    public IList<Tarefa> Listar()
    {
        var lista = new List<Tarefa>();

        using var conexao = new MySqlConnection(connectionString);
        conexao.Open();

        string sql = "SELECT id, nome, descricao, dataCriacao, dataExecucao, status FROM tarefa";

        using var cmd = new MySqlCommand(sql, conexao);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var tarefa = new Tarefa
            {
                Id = reader.GetInt32("id"),
                Nome = reader.GetString("nome"),
                Descricao = reader.GetString("descricao"),
                DataCriacao = reader.GetDateTime("dataCriacao"),
                DataExecucao = reader.IsDBNull(reader.GetOrdinal("dataExecucao"))
                                ? (DateTime?)null
                                : reader.GetDateTime("dataExecucao"),
                Status = reader.GetInt32("status")
            };
            lista.Add(tarefa);
        }

        return lista;
    }

    public void Alterar(Tarefa tarefa)
    {
        using var conexao = new MySqlConnection(connectionString);
        conexao.Open();

        string sql = @"UPDATE tarefa 
                       SET nome = @nome, descricao = @descricao, dataCriacao = @dataCriacao, 
                           status = @status, dataExecucao = @dataExecucao
                       WHERE id = @id";

        using var cmd = new MySqlCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@nome", tarefa.Nome);
        cmd.Parameters.AddWithValue("@descricao", tarefa.Descricao);
        cmd.Parameters.AddWithValue("@dataCriacao", tarefa.DataCriacao);
        cmd.Parameters.AddWithValue("@status", tarefa.Status);

        if (tarefa.DataExecucao.HasValue)
            cmd.Parameters.AddWithValue("@dataExecucao", tarefa.DataExecucao.Value);
        else
            cmd.Parameters.AddWithValue("@dataExecucao", DBNull.Value);

        cmd.Parameters.AddWithValue("@id", tarefa.Id);

        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conexao = new MySqlConnection(connectionString);
        conexao.Open();

        string sql = "DELETE FROM tarefa WHERE id = @id";

        using var cmd = new MySqlCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }
}