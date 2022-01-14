using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Comida_Feia
{
    public partial class Registo : Form
    {
        
        public Registo()
        {
            InitializeComponent();
            Tipo();
            TipoC();
            textBox1.Text = NovoUser().ToString();
        }
        

        
        #region ID automático
        /// <summary>
        /// Função que irá contar o número de produtos existentes na base de dados
        /// </summary>
        /// <returns></returns>
        private int NovoUser()
        {
            int total = 0;
            string query = "Select UID from Utilizador";

            
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
                            total = Convert.ToInt32(reader["UID"]);
                        }
                        reader.Close();
                    }
                    con.Close();
                }
            }
            return total + 1;
            
        }
        
        #endregion
        

        #region Funções das caixas de texto

        /// <summary>
        /// Função que coloca os tipos de utilizador numa combobox
        /// </summary>
        private void Tipo()
        {
            string querytype = "Select * from Tipo_Utilizador";
            Dictionary<int, string> comboSource = new Dictionary<int, string>();
            comboSource.Add(0, "Selecionar Opção");
            /*
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
                                comboSource.Add((int)reader["TUID"], reader["descTP"].ToString());
                            }
                            reader.Close();
                        }
                        con.Close();

                        comboBox1.DataSource = new BindingSource(comboSource, null);
                        comboBox1.DisplayMember = "Value";
                        comboBox1.ValueMember = "Key";
                    }
                }
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Erro interno da aplicação.\nErro: " + err.Message + "\n\nSe problema persistir informe administrador de sistema.", "Erro da aplicação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            */

        }


        /// <summary>
        /// Ativar o nome quando for selecionado um tipo de utilizador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var TUID = this.comboBox1.SelectedValue.ToString();
            textBox4.Enabled = true;
        }


        /// <summary>
        /// Função que coloca os tipos de contacto numa combobox
        /// </summary>
        private void TipoC()
        {
            string querytype = "Select * from Tipo_Contacto";
            Dictionary<int, string> comboSource = new Dictionary<int, string>();
            comboSource.Add(0, "Selecionar Opção");

            /*
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
                                comboSource.Add((int)reader["TCID"], reader["descCont"].ToString());
                            }
                            reader.Close();
                        }
                        con.Close();
                        comboBox2.DataSource = new BindingSource(comboSource, null);
                        comboBox2.DisplayMember = "Value";
                        comboBox2.ValueMember = "Key";
                    }
                }
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show("Erro interno da aplicação.\nErro: " + err.Message + "\n\nSe problema persistir informe administrador de sistema.", "Erro da aplicação", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            */

        }

        #endregion


        /// <summary>
        /// Função que guarda a informação introduzida nas caixas de texto na base de dados
        /// </summary>
        private void Guardar()
        {
            String UID, password, tipo, nome, morada, localidade, tpCont, contacto, CPID;
            int CP1, MID;
            string p = "|";


            UID = textBox1.Text;
            password = textBox2.Text;
            tipo = comboBox1.SelectedValue.ToString();
            nome = textBox4.Text;
            morada = textBox5.Text;
            localidade = textBox6.Text;
            tpCont = comboBox2.SelectedValue.ToString();
            contacto = textBox7.Text;
            string descCli = "Instituição";
            //CPID = Global_Var.CP_G;

            // Verificar se o CPID existe através da localidade
            // Verificar se a rua da Morada existe
            
             //* Ordem de Guardar
             //* 
             //* 1- CP -> Feito
             //* 2- Morada -> Feito
             //* 3- Utilizador (passsword, TipoUtilizadorUID, MID) -> Feito
             //* 4- Cliente / Fornecedor -> Feito 
             //* 5- Contacto -> Feito
             //* 
             //* Função NovoUser vai criar o novo id de utilizador
             


            string campos_CP = "localidade";
            string valores_CP = localidade;

            string campos_Cliente = "nome,descCli,UtilizadorUID";
            string valores_Cliente = nome + p + descCli + p + UID;

            string campos_Fornecedor = "nome,UtilizadorUID";
            string valores_Fornecedor = nome + p + UID;

            string campos_Contacto = "numero,Tipo_ContactoTCID,UtilizadorUID";
            string valores_Contacto = contacto + p + tipo + p + UID;

            
            // Verificar se as Passwords são iguais
            if (textBox2.Text == textBox3.Text)
            {
                var x = comboBox1.SelectedValue.ToString();
                // Se for Cliente
                if (x == "1")
                {
                    MessageBox.Show("Cliente");
                    // Se for falso, não existe localidade
                    if (ExisteCP() == false)
                    {
                        // Criar nova localidade
                        SQL.Inserir(SQL.tb_cp, campos_CP, valores_CP);
                        // Como a localidade não existe, cria se a morada dessa localidade

                        CP1 = CP_Mid();
                        string campos_MIDS = "rua,CPCPID";
                        string valores_MIDS = morada + p + CP1.ToString();

                        SQL.Inserir(SQL.tb_morada, campos_MIDS, valores_MIDS);

                        // Inserir Utilizador com a nova morada do novo CP
                        MID = M_MID();
                        string campos_Utilizador = "password,Tipo_UtilizadorTUID,MoradaMID";
                        string valores_Utilizador = password + p + tipo + p + MID;
                        SQL.Inserir(SQL.tb_utilizador, campos_Utilizador, valores_Utilizador);

                        //Inserir dados do Cliente com nova morada e CPID
                        SQL.Inserir(SQL.tb_cliente, campos_Cliente, valores_Cliente);

                        //Inserir Contacto com nova morada e CPID
                        SQL.Inserir(SQL.tb_contacto, campos_Contacto, valores_Contacto);

                    }
                    else
                    {
                        // Já existe a localidade
                        // Pode não existir a morada
                        // Se não existir MID
                        if (ExisteMID() == false)
                        {
                            CP1 = CP_Mid();
                            string campos_MID = "rua,CPCPID";
                            string valores_MID = morada + p + CP1.ToString();

                            // Se não existe morada cria
                            SQL.Inserir(SQL.tb_morada, campos_MID, valores_MID);

                            // Inserir Utilizador com a nova morada
                            MID = M_MID();
                            string campos_Utilizador = "password,Tipo_UtilizadorTUID,MoradaMID";
                            string valores_Utilizador = password + p + tipo + p + MID;
                            SQL.Inserir(SQL.tb_utilizador, campos_Utilizador, valores_Utilizador);

                            //Inserir dados do Cliente com nova morada e CPID existente
                            SQL.Inserir(SQL.tb_cliente, campos_Cliente, valores_Cliente);

                            //Inserir Contacto com nova morada e CPID existente
                            SQL.Inserir(SQL.tb_contacto, campos_Contacto, valores_Contacto);

                        }
                        else
                        {
                            // Aqui já existe, não se cria morada
                            // Inserir utilizador com morada existente
                            MID = M_MID();
                            string campos_Utilizador = "password,Tipo_UtilizadorTUID,MoradaMID";
                            string valores_Utilizador = password + p + tipo + p + MID;
                            SQL.Inserir(SQL.tb_utilizador, campos_Utilizador, valores_Utilizador);

                            //Inserir dados do Cliente com morada e CPID existente
                            SQL.Inserir(SQL.tb_cliente, campos_Cliente, valores_Cliente);

                            //Inserir Contacto com morada e CPID existente
                            SQL.Inserir(SQL.tb_contacto, campos_Contacto, valores_Contacto);

                        }
                    }
                }
                // Se for Fornecedor
                else
                {
                    MessageBox.Show("Fornecedor");
                    // Se for falso, não existe localidade
                    if (ExisteCP() == false)
                    {
                        // Criar nova localidade
                        SQL.Inserir(SQL.tb_cp, campos_CP, valores_CP);
                        // Como a localidade não existe, cria se a morada dessa localidade

                        CP1 = CP_Mid();
                        string campos_MIDS = "rua,CPCPID";
                        string valores_MIDS = morada + p + CP1.ToString();

                        SQL.Inserir(SQL.tb_morada, campos_MIDS, valores_MIDS);

                        // Inserir Utilizador com a nova morada do novo CP
                        MID = M_MID();
                        string campos_Utilizador = "password,Tipo_UtilizadorTUID,MoradaMID";
                        string valores_Utilizador = password + p + tipo + p + MID;
                        SQL.Inserir(SQL.tb_utilizador, campos_Utilizador, valores_Utilizador);

                        //Inserir dados do Fornecedor com nova morada e CPID
                        SQL.Inserir(SQL.tb_fornecedor, campos_Fornecedor, valores_Fornecedor);

                        //Inserir Contacto com nova morada e CPID
                        SQL.Inserir(SQL.tb_contacto, campos_Contacto, valores_Contacto);

                    }
                    else
                    {
                        // Já existe a localidade
                        // Pode não existir a morada
                        // Se não existir MID
                        if (ExisteMID() == false)
                        {
                            CP1 = CP_Mid();
                            string campos_MID = "rua,CPCPID";
                            string valores_MID = morada + p + CP1.ToString();

                            // Se não existe morada cria
                            SQL.Inserir(SQL.tb_morada, campos_MID, valores_MID);

                            // Inserir Utilizador com a nova morada
                            MID = M_MID();
                            string campos_Utilizador = "password,Tipo_UtilizadorTUID,MoradaMID";
                            string valores_Utilizador = password + p + tipo + p + MID;
                            SQL.Inserir(SQL.tb_utilizador, campos_Utilizador, valores_Utilizador);

                            //Inserir dados do Fornecedor com nova morada e CPID existente
                            SQL.Inserir(SQL.tb_fornecedor, campos_Fornecedor, valores_Fornecedor);

                            //Inserir Contacto com nova morada e CPID existente
                            SQL.Inserir(SQL.tb_contacto, campos_Contacto, valores_Contacto);

                        }
                        else
                        {
                            // Aqui já existe, não se cria morada
                            // Inserir utilizador com morada existente
                            MID = M_MID();
                            string campos_Utilizador = "password,Tipo_UtilizadorTUID,MoradaMID";
                            string valores_Utilizador = password + p + tipo + p + MID;
                            SQL.Inserir(SQL.tb_utilizador, campos_Utilizador, valores_Utilizador);

                            //Inserir dados do Fornecedor com morada e CPID existente
                            SQL.Inserir(SQL.tb_fornecedor, campos_Fornecedor, valores_Fornecedor);

                            //Inserir Contacto com morada e CPID existente
                            SQL.Inserir(SQL.tb_contacto, campos_Contacto, valores_Contacto);

                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("As palavras passes não coicidem!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        #region Botões
        /// <summary>
        /// Botão cancelar, volta para o login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
           // var Form1 = new Form1();
            //Form1.Show();
        }


        /// <summary>
        /// Botão para registar as pessoas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        #endregion

        
        #region Funções Determinantes

        /// <summary>
        /// Função que determina se já existe um CP 
        /// correspondente à localidade
        /// </summary>
        /// <returns></returns>
        public bool ExisteCP()
        {
            string query = "SELECT CPID FROM CP WHERE localidade='" + textBox6.Text + "'";

            
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
                            Global_Var.CP_G = (reader["CPID"]).ToString();
                            return true;
                        }
                        reader.Close();
                    }
                    con.Close();

                }
            }
            return false;
        }

        /// <summary>
        /// Função que determina se já existe uma morada 
        /// correspondente à localidade
        /// </summary>
        /// <returns></returns>
        public bool ExisteMID()
        {
            string query = "SELECT MID FROM Morada WHERE rua='" + textBox5.Text + "'";

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
                            Global_Var.MID_G = (reader["MID"]).ToString();
                            return true;
                        }
                        reader.Close();
                    }
                    con.Close();
                }
            }
            return false;
        }


        /// <summary>
        /// Função que vai buscar o novo CPID da localidade introduzida
        /// </summary>
        /// <returns></returns>
        private int CP_Mid()
        {
            int CP = 0;
            string query = "Select CPID from CP where localidade='" + textBox6.Text + "'";

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
                            CP = Convert.ToInt32(reader["CPID"]);
                        }
                        reader.Close();
                    }
                    con.Close();
                }
            }
            return CP;           
        }


        /// <summary>
        /// Função que vai buscar o MID da morada introduzida
        /// </summary>
        /// <returns></returns>
        private int M_MID()
        {
            int MID = 0;
            string query = "Select MID from Morada where rua='" + textBox5.Text + "'";

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
                            MID = Convert.ToInt32(reader["MID"]);
                        }
                        reader.Close();
                    }
                    con.Close();
                }
            }
            return MID;
            
        }       
        #endregion
        
       
    }
}




