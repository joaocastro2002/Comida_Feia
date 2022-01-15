using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ComidaFeia
{
    public partial class Cliente : Form
    {
        public Cliente()
        {
            InitializeComponent();
            TipoP();
            Estado();
            Anos();
            Mes();
            Listagem();
            //textBox1.Text = Global_Var.UID_G;

        }

        #region Pedido

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
                                this.comboBox3.Items.Add(reader["descTProd"]);
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
        private void Estado(int val = 0)
        {
            string querytype = "Select distinct(descEst) from Estado " +
           "JOIN Produto on Produto.EstadoEID=Estado.EID " +
           "JOIN Tipo_Prod on Tipo_Prod.TPID=Produto.Tipo_ProdTPID ";

            if (val != 0)
            {
                querytype += "Where descTProd= '" + comboBox3.SelectedItem + "'"; ;
            }
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
                                this.comboBox4.Items.Add(reader["descEst"]);
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
        /// Função que coloca os produtos numa combobox do respetivo tipo
        /// </summary>
        private void Produtos(int val = 0)
        {

            //Buscar o nome do produto
            string querytype = "Select distinct(descPro) from Produto JOIN Tipo_Prod ON Tipo_Prod.TPID=Produto.Tipo_ProdTPID JOIN Estado on Produto.EstadoEID=Estado.EID";

            if (val != 0)
            {
                querytype += " WHERE descTProd='" + this.comboBox3.SelectedItem + "' AND descEst= '" + comboBox4.SelectedItem + "'";
            }
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
                                this.comboBox5.Items.Add(reader["descPro"]);
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

        // Feito
        #region Listar Pedidos

        /// <summary>
        /// Adicionar anos à combobox
        /// </summary>
        public void Anos()
        {
            comboBox1.Items.Add("2022");
        }


        /// <summary>
        /// Adicionar os meses à combobox
        /// </summary>
        public void Mes()
        {
            comboBox2.Items.Add("1");
            comboBox2.Items.Add("2");
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("4");
            comboBox2.Items.Add("5");
            comboBox2.Items.Add("6");
            comboBox2.Items.Add("7");
            comboBox2.Items.Add("8");
            comboBox2.Items.Add("9");
            comboBox2.Items.Add("10");
            comboBox2.Items.Add("11");
            comboBox2.Items.Add("12");
        }


        /// <summary>
        /// Botão que lista todos os pedidos efetuados pelo user no respetivo ano e mes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registar_Click(object sender, EventArgs e)
        {
            Tabela();
        }

        private void Tabela()
        {
            try
            {
                string query = "SELECT Pedido.quantidade , data,  descPro,descTProd, descEst, PeID FROM Pedido_Prod " +
                "JOIN Pedido ON Pedido.PeID=Pedido_Prod.PedidoPeID " +
                "JOIN Produto ON Produto.PID=Pedido_Prod.ProdutoPID " +
                "JOIN Estado ON Estado.EID=Pedido_Prod.ProdutoEstadoEID JOIN Tipo_Prod ON Tipo_Prod.TPID=Pedido_Prod.ProdutoTipo_ProdTPID" +
                    " WHERE YEAR(data)= '" + comboBox1.Text + "' and MONTH(data)= '" + comboBox2.Text + "' and Pedido.ClienteUtilizadorUID='" + textBox1.Text + "'";

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
                                "ID Pedido",
                                "Quantidade",
                                "Data",
                                "Nome",
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
                                row[0] = reader["PeID"].ToString();
                                row[1] = reader["quantidade"].ToString();
                                row[2] = reader["data"].ToString();
                                row[3] = reader["descPro"].ToString();
                                row[4] = reader["descTProd"].ToString();
                                row[5] = reader["descEst"].ToString();

                                dTable.Rows.Add(row);
                            }
                            reader.Close();
                            this.dataGridView2.DataSource = dTable;

                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
        #endregion // Feito

        //Feito
        #region Produtos Disponiveis
        private void Listagem()
        {
            try
            {
                string query = "SELECT PID, descPro, forma_Pro, quantidade, descTProd, descEst FROM Produto " +
                               "JOIN Tipo_Prod on Produto.Tipo_ProdTPID=Tipo_Prod.TPID " +
                               "JOIN Estado on Estado.EID=Produto.EstadoEID";

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
                                "PID",
                                "Nome",
                                "Produção",
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
                                row[0] = reader["PID"].ToString();
                                row[1] = reader["descPro"].ToString();
                                row[2] = reader["forma_pro"].ToString();
                                row[3] = reader["quantidade"].ToString();
                                row[4] = reader["descTProd"].ToString();
                                row[5] = reader["descEst"].ToString();

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
        #endregion region


        /// <summary>
        /// Botão Cancelar, apaga os dados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            textBox8.Clear();
        }


        /// <summary>
        /// Botão sair
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sair_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Apresentar os Produtos, conforme o seu tipo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox5.Text = "";
            this.comboBox4.Text = "";
            this.comboBox5.Items.Clear();
            this.comboBox4.Items.Clear();
            this.Produtos(1);
            this.Estado(1);
        }


        /// <summary>
        /// Apresentar os produtos conforme o estado 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox5.Text = "";
            this.comboBox5.Items.Clear();
            this.Produtos(1);
        }


        /// <summary>
        /// Botão para cancelar o pedido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "Delete from Pedido_Prod where PedidoPeID = '" + textBox2.Text + "'";
                string query2 = "Delete from Pedido where PeID = '" + textBox2.Text + "'";

                //Eliminar a query 1
                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        int eliminado = cmd.ExecuteNonQuery();
                        string message = String.Empty;
                        con.Close();
                    }
                }

                //Eliminar a query 2
                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    using (SqlCommand cmd = new SqlCommand(query2, con))
                    {
                        con.Open();
                        int eliminado = cmd.ExecuteNonQuery();
                        string message = String.Empty;
                        con.Close();
                    }
                }
                MessageBox.Show("O pedido foi cancelado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


        /// <summary>
        /// Botão confirmar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Guardar();
        }


        /// <summary>
        /// Contar o numero de pedidos na tabela pedidos
        /// </summary>
        /// <returns></returns>
        private int Count()
        {
            int total = 0;
            string queryPed = "Select count(PeID) as total from Pedido";

            using (SqlConnection con = new SqlConnection(conexao.comando))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(queryPed, con))
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
        /// Função que guarda os dados na base de dados
        /// </summary>
        private void Guardar()
        {
            int PeID, n_quantidade;
            string UID, tipo, estado, nome, quantidade, data;
            string p = "|";

            PeID = Count() + 1;

            UID = textBox1.Text;
            tipo = comboBox3.Text;
            estado = comboBox4.Text;
            nome = comboBox5.Text;
            data = maskedTextBox1.Text;
            quantidade = textBox8.Text;

            Existe();
            n_quantidade = Convert.ToInt32(Global_Var.Q_G) - Convert.ToInt32(quantidade);

            string campos_Pedido = "quantidade,ClienteUtilizadorUID,PeID,data";
            string valores_Pedido = quantidade + p + UID + p + PeID + p + data;

            string campos_Pedido_Prod = "PedidoClienteUtilizadorUID,PedidoPeID,ProdutoPID,ProdutoTipo_ProdTPID,ProdutoEstadoEID";
            string valores_Pedido_Prod = UID + p + PeID + p + Global_Var.PID_G + p + Global_Var.TP_G + p + Global_Var.E_G;

            // Valores a atualizar nas quantidades
            string campos_PrA = "quantidade";
            string valores_PrA = n_quantidade.ToString();

            //Inserir no Pedido
            try
            {
                if (Int32.Parse(quantidade) <= Int32.Parse(Global_Var.Q_G))
                {
                    SQL.Inserir(SQL.tb_pedido, campos_Pedido, valores_Pedido);
                    SQL.Actualizar(SQL.tb_produto, campos_PrA, valores_PrA, Global_Var.PID_G);
                    Listagem();

                    MessageBox.Show("Dados gravados com sucesso!", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Erro! Não está disponivel essa quantidade!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            //Inserir no Pedido_Prod
            try
            {
                if (SQL.Inserir(SQL.tb_ped_prod, campos_Pedido_Prod, valores_Pedido_Prod))
                {
                    //MessageBox.Show("Dados gravados com sucesso!", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Erro!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }


        /// <summary>
        /// Guardar o PID do produto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Busca o PID do produto
            string query = "Select PID, EID, TPID, quantidade FROM Produto JOIN Tipo_Prod on Tipo_Prod.TPID=Produto.Tipo_ProdTPID JOIN Estado on Estado.EID=Produto.EstadoEID WHERE descTProd='" + this.comboBox3.SelectedItem + "' AND descEst= '" + comboBox4.SelectedItem + "' AND descPro= '" + comboBox5.SelectedItem + "'";
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
                                Global_Var.PID_G = (reader["PID"]).ToString();
                                Global_Var.TP_G = (reader["TPID"]).ToString();
                                Global_Var.E_G = (reader["EID"]).ToString();
                                Global_Var.Q_G = (reader["quantidade"]).ToString();
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
        /// Verificar existencia de um produto
        /// </summary>
        private int Existe()
        {
            string query = "Select PID, EID, TPID, descPro, quantidade, count(*) as total FROM Produto " +
                "JOIN Tipo_Prod on Tipo_Prod.TPID=Produto.Tipo_ProdTPID " +
                "JOIN Estado on Estado.EID=Produto.EstadoEID " +
                "WHERE descTProd='" + this.comboBox3.SelectedItem + "' AND descEst= '" + comboBox4.SelectedItem + "' AND descPro='" + comboBox5.SelectedItem + "' " +
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

        private void Cliente_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
