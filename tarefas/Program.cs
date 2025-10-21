// See https://aka.ms/new-console-template for more information
Console.WriteLine("Seja bem vindo!");

Console.WriteLine("Gastos mensais");
var tarefa01 = new Tarefa();
tarefa01.Nome = "Contas basicas";
tarefa01.Descricao  = "Pagar agua, luz, internet e aluguel";
tarefa01.DataCriacao = DateTime.Now; 
tarefa01.Status = 1;
tarefa01.DataExecucao = null;

Console.WriteLine("Gastos mensais preenchidos");

Console.WriteLine("Inserindo gastos mensais no banco de dados");

var operacoes = new Operacoes();
int idInserido = operacoes.Criar(tarefa01);

Console.WriteLine($"Gastos mensais inseridos no banco de dados com sucesso. Id: {idInserido}");