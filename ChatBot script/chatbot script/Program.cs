using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static async Task Main(string[] args)
    {
        // Retrieve the connection string from configuration
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString;
        int delay = GetDelay(connectionString);

        // Retrieve messages from the database
        List<string>[] mensagens = await RetrieveMessagesFromDatabase(connectionString);
        string imagem_URL = await RetrieveImagemUrl(connectionString);

        if (mensagens == null)
        {
            Console.WriteLine("Failed to retrieve messages from the database.");
            return;
        }

        List<string> bloco1 = mensagens[0];
        List<string> bloco2 = mensagens[1];
        List<string> bloco3 = mensagens[2];
        List<string> bloco4 = mensagens[3];
        List<string> bloco5 = mensagens[4];
        List<string> bloco6 = mensagens[5];
        List<string> bloco7 = mensagens[6];

        // SQL query to select the first 100 rows
        string query = "SELECT * FROM robo where telefonenozap is true";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var telefone = reader["telefone"];
                            var titulo = reader["titulo"].ToString().Replace("\n", "").Replace("\r", "");

                            var (mensagem, combinacao, imagemUrl) = ComporMensagem(bloco1, bloco2, bloco3, bloco4, bloco5, bloco6, bloco7, titulo, imagem_URL);
                            Console.WriteLine(mensagem);
                            Console.WriteLine(combinacao);

                            
                            var client = new HttpClient();

                            var requestData = new
                            {
                                canal = "",
                                mensagem = mensagem,
                                acao = "alerta",
                                tipo = "",
                                nome = titulo,
                                email = "",
                                telefone = telefone,
                                midia = string.IsNullOrEmpty(imagemUrl) ? 1 : 2,
                                url = imagemUrl,
                                imagemName = string.IsNullOrEmpty(imagemUrl) ? null : Path.GetFileName(imagemUrl),
                            };
                            
                            var url = ConfigurationManager.AppSettings["url"].ToString();
                            var jsonContent = JsonConvert.SerializeObject(requestData);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            try
                            {
                                var response = await client.PostAsync(url, content);
                                Console.WriteLine(await response.Content.ReadAsStringAsync());
                                if (response.IsSuccessStatusCode)
                                {
                                    var responseContent = await response.Content.ReadAsStringAsync();
                                    Console.WriteLine("Success! Response: " + responseContent);
                                }
                                else
                                {
                                    Console.WriteLine("Error: " + response.StatusCode);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Exception: " + ex.Message);
                            }

                            string nome = requestData.nome;
                            string NumeroTelefone = requestData.telefone.ToString();
                            string CombinacaoEnviada = combinacao.ToString();
                            string query2 = "INSERT INTO chatbot (Nome, Numero_Telefone, Combinacao_Enviada) VALUES (@Nome, @NumeroTelefone, @CombinacaoEnviada)";

                            using (MySqlConnection connection2 = new MySqlConnection(connectionString))
                            {
                                MySqlCommand command2 = new MySqlCommand(query2, connection2);
                                command2.Parameters.AddWithValue("@Nome", nome);
                                command2.Parameters.AddWithValue("@NumeroTelefone", NumeroTelefone);
                                command2.Parameters.AddWithValue("@CombinacaoEnviada", CombinacaoEnviada);
                                connection2.Open();
                                command2.ExecuteNonQuery();
                                connection2.Close();
                            }
                            Console.WriteLine("Dados inseridos com sucesso!");
                            await Task.Delay(delay);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                await Task.Delay(10000);
            }
        }
    }
    static int GetDelay(string connectionString)
    {
        string query = "SELECT delay FROM robo_delay LIMIT 1";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    int delay = result != null ? Convert.ToInt32(result) : 0;
                    return delay;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving delay: " + ex.Message);
                return 5000; // Default delay if there's an error
            }
        }
    }
    static async Task<string> RetrieveImagemUrl(string connectionString)
    {
        string query = "Select url FROM mensagens WHERE bloco = 7";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string imagemUrl = reader.GetString("url");
                            return imagemUrl;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving imagem URL: " + ex.Message);
                return null;
            }
        }
        return null;
    }
        static async Task<List<string>[]> RetrieveMessagesFromDatabase(string connectionString)
    {
        List<string>[] messages = new List<string>[7];
        for (int i = 0; i < 7; i++)
        {
            messages[i] = new List<string>();
        }

        string query = "SELECT bloco, mensagem FROM mensagens ORDER BY bloco, id";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int bloco = reader.GetInt32("bloco");
                            string mensagem = reader.GetString("mensagem");

                            if (bloco >= 1 && bloco <= 7)
                            {
                                messages[bloco - 1].Add(mensagem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving messages: " + ex.Message);
                return null;
            }
        }

        return messages;
    }

    static (string mensagem, string combinacao, string imagemUrl) ComporMensagem(List<string> bloco1, List<string> bloco2, List<string> bloco3, List<string> bloco4, List<string> bloco5, List<string> bloco6, List<string> bloco7, string titulo, string imagemUrl)
    {
        Random random = new Random();

        // Seleciona um item aleatório de cada bloco e armazena o índice
        int index1 = random.Next(bloco1.Count);
        int index2 = random.Next(bloco2.Count);
        int index3 = random.Next(bloco3.Count);
        int index4 = random.Next(bloco4.Count);
        int index5 = random.Next(bloco5.Count);
        int index6 = random.Next(bloco6.Count);
        int index7 = random.Next(bloco7.Count);

        string parte1 = bloco1[index1];
        string parte2 = bloco2[index2];
        string parte3 = bloco3[index3];
        string parte4 = bloco4[index4];
        string parte5 = bloco5[index5];
        string parte6 = bloco6[index6];
        string parte7 = bloco7[index7];

        
        

        // Concatena os blocos em uma única mensagem
        string mensagem = $"{parte1.Replace("[titulo]", titulo)} {parte2} {parte3} {parte4} {parte5} {parte6} {parte7}";
        string combinacao = $"{index1 + 1}{index2 + 1}{index3 + 1}{index4 + 1}{index5 + 1}{index6 + 1}{index7 + 1}";
        return (mensagem, combinacao, imagemUrl);
    }
}

