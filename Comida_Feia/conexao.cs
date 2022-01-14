

namespace Comida_Feia
{
    static class conexao
    {
        //Alterar os valores
        static string baseDados = "ComidaFeia";
        static string servidor = "LAPTOP-CE4FD5R5";
        // static string servidor = "LAPTOP-CE4FD5R5\\MSSQLSERVER01";
        static string utilizador = "sergio";
        static string password = "1234567";

        //public static string comando = "SERVER=" + servidor + ";" + "DATABASE=" + baseDados + ";" + "UID=" + utilizador + ";" + "PASSWORD=" + password + ";";

        public static string comando = @"Data Source=LAPTOP-CE4FD5R5;Initial Catalog=ComidaFeia;User ID=sergio;Password=1234567";


        /*public static string comando = "Data Source=ServerName;" +
          "Initial Catalog="+ baseDados + ";" +
          "User id="+ utilizador + ";" +
          "Password="+password+";";*/
    }
}
