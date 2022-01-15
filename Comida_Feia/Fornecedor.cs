using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ComidaFeia
{
    public partial class Fornecedor : Form
    {
        public Fornecedor()
        {
            InitializeComponent();
            Estado();
            TipoP();
            Listagem();
            textBox1.Text = Global_Var.UID_G;
        }

        #region Preencher Combobox

        /// <summary>
        /// Função que coloca os tipos de produtos numa combobox
        /// </summary>
        private void TipoP()
        {
            string querytype = "Select descTProd from Tipo_Prod";
            try
            {
                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(querytype, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                this.comboBox1.Items.Add(reader["descTProd"]);
                            }
                            reader.Close();
                        }
                        con.Close();
                    }
                }
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Erro interno da aplicação.\nErro: " + err.Message + "\n\nSe problema persistir informe administrador de sistema.", "Erro da aplicação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /// <summary>
        /// Função que coloca os estados dos produtos numa combobox
        /// </summary>
        private void Estado()
        {
            string querytype = "Select descEst from Estado";
            try
            {
                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(querytype, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                this.comboBox2.Items.Add(reader["descEst"]);
                            }
                            reader.Close();
                        }
                        con.Close();
                    }
                }
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Erro interno da aplicação.\nErro: " + err.Message + "\n\nSe problema persistir informe administrador de sistema.", "Erro da aplicação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        #endregion

        #region Botões

        /// <summary>
        /// Botão cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox2.Clear();
            textBox3.Clear();
        }

        /// <summary>
        /// Botão Sair
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Botão Guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Guardar();
        }
        #endregion

        #region Table
        private void Listagem()
        {
            try
            {
                string query = "SELECT DID, descDoa, quantidade, descTProd, descEst " +
                    "FROM Doacao " +
                    "JOIN Doa_Prod ON Doacao.DID=Doa_Prod.DoacaoDID " +
                    "JOIN Tipo_Prod ON Tipo_Prod.TPID=Doa_Prod.ProdutoTipo_ProdTPID " +
                    "JOIN Estado ON Estado.EID=Doa_Prod.ProdutoEstadoEID " +
                    "WHERE FornecedorUtilizadorUID='" + Global_Var.UID_G + "'";

                DataTable dTable = new DataTable();
                DataRow row = null;
                dTable.Rows.Clear();

                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        string[] cols =
                        {
                                "DID",
                                "Nome",
                                "Quantidade",
                                "Tipo Produto",
                                "Estado"
                        };

                        for (int i = 0; i < cols.Length; i++)
                        {
                            dTable.Columns.Add(cols[i]);
                        }

                        // se existir resultados preenche a tabela
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                row = dTable.NewRow();
                                row[0] = reader["DID"].ToString();
                                row[1] = reader["descDoa"].ToString();
                                row[2] = reader["quantidade"].ToString();
                                row[3] = reader["descTProd"].ToString();
                                row[4] = reader["descEst"].ToString();
                                dTable.Rows.Add(row);
                            }
                            reader.Close();
                            this.dataGridView1.DataSource = dTable;

                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        #endregion

        #region ComboBox Selection

        /// <summary>
        /// Guardar o ID do tipo de Produto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "Select TPID FROM Tipo_Prod  WHERE descTProd= '" + comboBox1.SelectedItem + "'";
            try
            {
                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Global_Var.TP1_G = (reader["TPID"]).ToString();
                            }
                            reader.Close();
                        }
                        con.Close();
                    }
                }
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Erro interno da aplicação.\nErro: " + err.Message + "\n\nSe problema persistir informe administrador de sistema.", "Erro da aplicação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        /// <summary>
        /// Guardar o ID do estado do produto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "Select EID FROM Estado WHERE descEst= '" + comboBox2.SelectedItem + "'";
            try
            {
                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Global_Var.E1_G = (reader["EID"]).ToString();
                            }
                            reader.Close();
                        }
                        con.Close();
                    }
                }
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Erro interno da aplicação.\nErro: " + err.Message + "\n\nSe problema persistir informe administrador de sistema.", "Erro da aplicação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        #endregion

        #region Funções

        /// <summary>
        /// Função que irá contar o número de produtos existentes na base de dados
        /// </summary>
        /// <returns></returns>
        private int ContarPro()
        {
            int total = 0;
            string query = "Select count(PID) as total from Produto";

            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            total = Convert.ToInt32(reader["total"]);
                        }
                        reader.Close();
                    }
                    con.Close();
                }
            }
            return total;
        }

        /// <summary>
        /// Função que irá contar o número de doações existentes para atribuir um ID ao DID
        /// </summary>
        private int ContarD()
        {
            int total = 0;
            string query = "Select count(DID) as total from Doacao";

            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            total = Convert.ToInt32(reader["total"]);
                        }
                        reader.Close();
                    }
                    con.Close();
                }
            }
            return total;
        }

        /// <summary>
        /// Função que guarda as doações e cria novos produtos caso não existam
        /// </summary>
        private void Guardar()
        {
            int DID, PID, n_quantidade;
            string UID, tipo, estado, nome, quantidade, forma;
            string p = "|";

            DID = ContarD() + 1;
            PID = ContarPro() + 1;

            UID = textBox1.Text;
            tipo = comboBox1.Text;
            estado = comboBox2.Text;
            nome = textBox2.Text;
            quantidade = textBox3.Text;
            forma = "Normal";

            Existe();
            n_quantidade = Convert.ToInt32(quantidade) + Convert.ToInt32(Global_Var.Q_G);

            string campos_Doacao = "DID,descDoa,Doacao.quantidade,FornecedorUtilizadorUID";
            string valores_Doacao = DID + p + nome + p + quantidade + p + UID;

            // Caso não exista o produto na base de dados
            string campos_Doa_Prod = "DoacaoDID,DoacaoFornecedorUtilizadorUID,ProdutoPID,ProdutoTipo_ProdTPID,ProdutoEstadoEID";
            string valores_Doa_Prod = DID + p + UID + p + PID + p + Global_Var.TP1_G + p + Global_Var.E1_G;

            //Se o produto não existir teremos de criar um novo
            string campos_Produto = "PID,descPro,forma_pro,Produto.quantidade,Tipo_ProdTPID,EstadoEID";
            string valores_Produto = PID + p + nome + p + forma + p + quantidade + p + Global_Var.TP1_G + p + Global_Var.E1_G;

            // Valores a atualizar nas quantidades
            string campos_PrA = "quantidade";
            string valores_PrA = n_quantidade.ToString();

            //Caso o produto já exista
            string valores_Doa_Prod2 = DID + p + UID + p + Global_Var.PID_G + p + Global_Var.TP_G + p + Global_Var.E_G;


            //Inserir nas Doações
            try
            {
                //Se existir
                if (Existe().ToString() == "1")
                {
                    //Se o produto existe, apenas adiciona a doacao e no doa_Prod e tem de fazer um update no produto existente, aumentando a quantidade 
                    SQL.Inserir(SQL.tb_doacao, campos_Doacao, valores_Doacao);
                    SQL.Inserir(SQL.tb_doa_prod, campos_Doa_Prod, valores_Doa_Prod2);

                    SQL.Actualizar(SQL.tb_produto, campos_PrA, valores_PrA, Global_Var.PID_G);
                    Listagem();

                    MessageBox.Show("A sua doação foi realizada!", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Se o produto não existir
                else
                {
                    SQL.Inserir(SQL.tb_produto, campos_Produto, valores_Produto);
                    SQL.Inserir(SQL.tb_doacao, campos_Doacao, valores_Doacao);
                    SQL.Inserir(SQL.tb_doa_prod, campos_Doa_Prod, valores_Doa_Prod);
                    Listagem();

                    MessageBox.Show("A sua doação foi realizada!", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        #endregion


        /// <summary>
        /// Verificar existencia de um produto
        /// </summary>
        private int Existe()
        {
            string query = "Select PID, EID, TPID, descPro, quantidade, count(*) as total FROM Produto " +
                "JOIN Tipo_Prod on Tipo_Prod.TPID=Produto.Tipo_ProdTPID " +
                "JOIN Estado on Estado.EID=Produto.EstadoEID " +
                "WHERE descTProd='" + this.comboBox1.SelectedItem + "' AND descEst= '" + comboBox2.SelectedItem + "' AND descPro='" + textBox2.Text + "' " +
                "GROUP BY PID, EID, TPID, descPro, quantidade";

            int prod_existe = 0;
            int resp = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                prod_existe = Convert.ToInt32(reader["total"].ToString());
                                Global_Var.PID_G = (reader["PID"]).ToString();
                                Global_Var.TP_G = (reader["TPID"]).ToString();
                                Global_Var.E_G = (reader["EID"]).ToString();
                                Global_Var.Q_G = (reader["quantidade"]).ToString();
                            }
                        }
                        else
                        {
                            prod_existe = 0;
                        }
                        reader.Close();
                        con.Close();
                    }
                }
                // Se o produto existe
                if (prod_existe > 0)
                {
                    resp = 1;

                }
                // Se o produto não existe
                else
                {
                    resp = 0;
                }
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Erro interno da aplicação.\nErro: " + err.Message + "\n\nSe problema persistir informe administrador de sistema.", "Erro da aplicação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return resp;
        }

        private void Fornecedor_Load(object sender, EventArgs e)
        {

        }
    }
}
