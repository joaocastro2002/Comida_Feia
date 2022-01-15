using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ComidaFeia
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DadosComb();
            Esconder();
        }

        /// <summary>
        /// Esconder a palavra passe
        /// </summary>
        private void Esconder()
        {
            this.textBox2.UseSystemPasswordChar = false;
            this.textBox2.PasswordChar = '*';
        }


        /// <summary>
        /// Função que coloca os tipos dd utilizador numa combobox
        /// </summary>
        private void DadosComb()
        {
            string querytype = "Select descTP from Tipo_Utilizador";
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
                                this.comboBox1.Items.Add(reader["descTP"]);
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
        /// Botão de entrar com a conta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void entrar_Click(object sender, EventArgs e)
        {
            int user_existe = 0;
            string querylogin = "SELECT UID, count(*) as total FROM Utilizador " +
                "JOIN Tipo_Utilizador on Utilizador.Tipo_UtilizadorTUID=Tipo_Utilizador.TUID " +
                "WHERE UID= '" + textBox1.Text + "' And password= '" + textBox2.Text + "' And descTP= '" + comboBox1.Text + "' GROUP BY UID";

            Global_Var.UID_G = textBox1.Text;


            try
            {
                using (SqlConnection con = new SqlConnection(conexao.comando))
                {
                    using (SqlCommand cmd = new SqlCommand(querylogin, con))
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                user_existe = Convert.ToInt32(reader["total"].ToString());
                            }
                        }
                        else
                        {
                            user_existe = 0;
                        }
                        reader.Close();
                        con.Close();
                    }
                }

                if (user_existe > 0)
                {
                    MessageBox.Show("Login efetuado com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    if (comboBox1.Text == "Cliente")
                    {
                        //Abre o form dos clientes
                        var Cliente = new Cliente();
                        Cliente.Show();
                    }
                    else
                    {
                        //Abre o form dos fornecedores
                        var Fornecedor = new Fornecedor();
                        Fornecedor.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Os dados introduzidos estão Incorretos", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Erro interno da aplicação.\nErro: " + err.Message + "\n\nSe problema persistir informe administrador de sistema.", "Erro da aplicação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Botão iterativo para registar uma nova conta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registar_Click(object sender, EventArgs e)
        {
            var Registo = new Registo();
            Registo.Show();
        }

        /// <summary>
        /// Botão para sair da aplicação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
