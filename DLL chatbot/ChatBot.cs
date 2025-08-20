using MySql.Data.MySqlClient;
using System.Configuration;

namespace DLL_chatbot
{
    public class ChatBot
    {
        public string ProcessaMensagem(string telefone, string mensagem)
        {
            // String de conexão com o banco de dados
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ToString();

            // Query SQL para selecionar os telefones
            string query = "SELECT * FROM chatbot";
            string mensagemRetorno = string.Empty;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        Console.WriteLine("Falha ao conectar ao banco de dados.");
                        return "Erro ao conectar ao banco de dados.";
                    }

                    Console.WriteLine("Conexão com o banco de dados estabelecida.");

                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (telefone == reader["Numero_Telefone"].ToString())
                            {
                                string estado = reader["estado"].ToString();

                                Console.WriteLine($"Telefone encontrado: {telefone}, Estado: {estado}");

                                reader.Close(); // Close the reader before executing another query

                                if (string.IsNullOrEmpty(estado))
                                {
                                    AtualizarEstado(connection, telefone, "1");
                                    return BuscarMensagem(connection, "15");
                                }

                                if (estado == "1")
                                {
                                    if (mensagem == "1")
                                    {
                                        AtualizarEstado(connection, telefone, "3");
                                        return BuscarMensagem(connection, "6");
                                    }
                                    else if (mensagem == "2")
                                    {
                                        AtualizarEstado(connection, telefone, "2");
                                        return BuscarMensagem(connection, "7");
                                    }
                                    else
                                    {
                                        return BuscarMensagem(connection, "8");
                                    }
                                }

                                if (estado == "2")
                                {
                                    if (mensagem == "1")
                                    {
                                        AtualizarEstado(connection, telefone, "3");
                                        return BuscarMensagem(connection, "6");
                                    }
                                    else if (mensagem == "2")
                                    {
                                        AtualizarEstado(connection, telefone, "3");
                                        return BuscarMensagem(connection, "9");
                                    }
                                    else
                                    {
                                        return BuscarMensagem(connection, "8");
                                    }
                                }

                                if (estado == "3")
                                {
                                    switch (mensagem)
                                    {
                                        case "1": return BuscarMensagem(connection, "10");
                                        case "2": return BuscarMensagem(connection, "11");
                                        case "3": return BuscarMensagem(connection, "12");
                                        case "4": return BuscarMensagem(connection, "13");
                                        case "5": return BuscarMensagem(connection, "14");
                                        case "6":
                                            AtualizarEstado(connection, telefone, "3");
                                            return BuscarMensagem(connection, "6");
                                        default:
                                            return BuscarMensagem(connection, "8");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Erro de banco de dados: {ex.Message}\nStack Trace: {ex.StackTrace}");
                    return "Erro ao processar a mensagem devido a um problema no banco de dados.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro inesperado: {ex.Message}\nStack Trace: {ex.StackTrace}");
                    return "Erro inesperado ao processar a mensagem.";
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                        Console.WriteLine("Conexão com o banco de dados fechada.");
                    }
                }
            }

            Console.WriteLine("Nenhum telefone correspondente encontrado.");
            return mensagemRetorno;
        }

        private void AtualizarEstado(MySqlConnection connection, string telefone, string novoEstado)
        {
            string updateQuery = "UPDATE chatbot SET estado = @novoEstado WHERE Numero_Telefone = @telefone";
            try
            {
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@novoEstado", novoEstado);
                    command.Parameters.AddWithValue("@telefone", telefone);
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Estado atualizado para {novoEstado} para o telefone {telefone}.");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar estado: {ex.Message}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private string BuscarMensagem(MySqlConnection connection, string bloco)
        {
            string selectQuery = "SELECT mensagem FROM mensagens WHERE bloco = @bloco LIMIT 1";

            try
            {
                using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
                {
                    // Definir o parâmetro explicitamente
                    command.Parameters.Add("@bloco", MySqlDbType.VarChar).Value = bloco;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Ler o resultado
                        if (reader.Read())
                        {
                            // Obter valor diretamente como string
                            string mensagem = reader.IsDBNull(0) ? null : reader.GetString(0);

                            if (mensagem != null)
                            {
                                Console.WriteLine($"Mensagem encontrada para o bloco {bloco}: {mensagem}");
                                return mensagem;
                            }
                        }
                    }
                }

                // Caso nenhuma mensagem seja encontrada
                Console.WriteLine($"Nenhuma mensagem encontrada para o bloco {bloco}.");
                return null;
            }
            catch (MySqlException ex)
            {
                Console.Error.WriteLine($"Erro ao executar a consulta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro inesperado: {ex.Message}");
                throw;
            }


            Console.WriteLine($"Mensagem não encontrada para o bloco {bloco}.");
            return "Mensagem não encontrada.";
        }
    }
}
