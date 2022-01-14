using System;
using System.Linq;
using System.Data.SqlClient;

namespace Comida_Feia
{
    class SQL
    {

        /// < TABELA DA BASE DE DADOS >
        /// Tabelas da base de dados
        /// </summary>
        public static string tb_utilizador = "Utilizador";
        public static string tb_tpuser = "Tipo_Utilizador";
        public static string tb_contacto = "Contacto";
        public static string tb_tpcont = "Tipo_Contacto";
        public static string tb_cp = "CP";
        public static string tb_morada = "Morada";
        public static string tb_fornecedor = "Fornecedor";
        public static string tb_cliente = "Cliente";
        public static string tb_produto = "Produto";
        public static string tb_pedido = "Pedido";
        public static string tb_doacao = "Doacao";
        public static string tb_ped_prod = "Pedido_Prod";
        public static string tb_doa_prod = "Doa_Prod";
        public static string tb_tpPr = "Tipo_Prod";
        public static string tb_estado = "Estado";


        /// < UPDATE >
        /// Atualização dos dados
        /// @ tabela , "campo1, campo2" , "val1 | val2", "id";
        /// </summary>
        /// <param name="tabela"> Tabela escolhida para editar dados </param>
        /// <param name="campos"> Campos da tabela selecionada </param>
        /// <param name="valores"> Valores atribuidos </param>
        /// <param name="id"> Id escolhido para atualizar os dados </param>
        public static bool Actualizar(string tabela, string campos, string valores, string id)
        {
            string query = "UPDATE " + tabela + " SET ";

            string[] campo = campos.Split(',');
            string[] valor = valores.Split('|');

            int contagem = campo.Count();

            int f = 1;
            int i = 0;

            foreach (string cmp in campo)
            {
                query += cmp + "='" + valor[i] + "',";

                if (f == contagem)
                {
                    query = query.Substring(0, query.LastIndexOf(','));
                }
                f++;
                i++;
            }
            query += "  WHERE PID='" + id + "'";

            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    int actualizado = cmd.ExecuteNonQuery();
                    string message = String.Empty;
                    con.Close();

                    return (actualizado > 0) ? true : false;
                }
            }
        }

        /// < UPDATE PERSONALIZADO >
        /// Atualização dos dados personalizados
        /// @ tabela , "campo1, campo2" , "val1 | val2", "campoPersonalizado", "valorPersonalizado";
        /// </summary>
        /// <param name="tabela"> Tabela escolhida para fazer as alterações </param>
        /// <param name="campos"> Campos da tabela </param>
        /// <param name="valores"> Valores Atribuidos </param>
        /// <param name="personalizado"> Id selecionado </param>
        /// <example> id_jog <> id, então temos o atualizar personalizado </example>
        /// <param name="custom"> Valor do 'personalizado' </param>/>
        public static bool ActualizarPersonalizado(string tabela, string campos, string valores, string personalizado, string custom)
        {
            string query = "UPDATE " + tabela + " SET ";

            string[] campo = campos.Split(',');
            string[] valor = valores.Split('|');

            int contagem = campo.Count();

            int f = 1, i = 0;

            foreach (string cmp in campo)
            {
                query += cmp + "='" + valor[i] + "',";

                if (f == contagem) query = query.Substring(0, query.LastIndexOf(','));

                f++;
                i++;
            }

            query += "  WHERE " + personalizado + "='" + custom + "'";

            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    int actualizado = cmd.ExecuteNonQuery();
                    string message = String.Empty;
                    con.Close();

                    return (actualizado > 0) ? true : false;
                }
            }
        }

        /// < INSERIR >
        /// Inserir os dados
        /// @ tabela , "x,x" , "z|z" ;
        /// </summary>
        /// <param name="tabela"> Tabela a inserir novos dados</param>
        /// <param name="campos"> Campos da tabela </param>
        /// <param name="valores"> Valores atribuidos </param>
        public static bool Inserir(string tabela, string campos, string valores)
        {
            string resultado = String.Empty;

            string query = "INSERT INTO " + tabela + " (";

            string[] campo = campos.Split(',');

            string[] valor = valores.Split('|');

            int contagem = campo.Count(), f = 1, v = 1;

            foreach (string cmp in campo)
            {
                query += cmp + ",";

                if (f == contagem)
                {
                    query = query.Substring(0, query.LastIndexOf(','));
                }
                f++;
            }

            query += ") VALUES (";

            foreach (string val in valor)
            {
                query += "'" + val + "',";

                if (v == contagem)
                {
                    query = query.Substring(0, query.LastIndexOf(','));
                }
                v++;
            }
            query += ");";


            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    int inserido = cmd.ExecuteNonQuery();
                    string message = String.Empty;
                    con.Close();

                    return (inserido > 0) ? true : false;
                }
            }
        }

        /// < APAGAR >
        /// Apagar dados 
        /// @ tabela , id;
        ///</summary>
        ///<param name="id"> Id que pretende selecionar </param>
        ///<param name="tabela"> Tabela para eliminar o id </param>
        public static bool Apagar(string tabela, string id)
        {
            string query = "DELETE FROM " + tabela + " WHERE id='" + id + "'";

            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    /*cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);*/

                    int eliminado = cmd.ExecuteNonQuery();
                    string message = String.Empty;

                    con.Close();

                    return (eliminado > 0) ? true : false;
                }
            }
        }

        public static bool ApagarPersonalizado(string tabela, string campo, string id)
        {
            string query = "DELETE FROM " + tabela + " WHERE " + campo + "='" + id + "'";

            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    /*cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);*/

                    int eliminado = cmd.ExecuteNonQuery();
                    string message = String.Empty;

                    con.Close();

                    return (eliminado > 0) ? true : false;
                }
            }
        }

        /// < QUANT. REG.>
        /// Total Registos base dados
        /// </summary>
        public static int TotalRegistos(string tbl, bool cond = false, string conds = null)
        {
            int total = 0;

            string q = "SELECT COUNT(*) AS total FROM " + tbl + " ";

            q += (cond) ? conds : " ";

            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            total = Convert.ToInt32(reader["total"]);
                        }
                    }
                    else
                    {
                        total = 0;
                    }
                }
                con.Close();
            }
            return total;
        }

    }
}
