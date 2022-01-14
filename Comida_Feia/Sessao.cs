namespace Comida_Feia
{
    public static class Global_Var
    {

        private static string uid_g, pid_g, tp_prod_g, est_g, quantidade_g, tp1_g, e1_g, cpid_g, mid_g;

        /// <summary>
        /// User ID global
        /// </summary>
        public static string UID_G
        {
            get { return Global_Var.uid_g; }

            set { Global_Var.uid_g = value; }
        }

        /// <summary>
        /// Produto ID
        /// </summary>
        public static string PID_G
        {
            get { return Global_Var.pid_g; }

            set { Global_Var.pid_g = value; }
        }

        /// <summary>
        /// Tipo Produto
        /// </summary>
        public static string TP_G
        {
            get { return Global_Var.tp_prod_g; }

            set { Global_Var.tp_prod_g = value; }
        }

        /// <summary>
        /// Estado
        /// </summary>
        public static string E_G
        {
            get { return Global_Var.est_g; }

            set { Global_Var.est_g = value; }
        }

        /// <summary>
        /// Quantidade
        /// </summary>
        public static string Q_G
        {
            get { return Global_Var.quantidade_g; }

            set { Global_Var.quantidade_g = value; }
        }

        /// <summary>
        /// Tipo Produto para doar
        /// </summary>
        public static string TP1_G
        {
            get { return Global_Var.tp1_g; }

            set { Global_Var.tp1_g = value; }
        }

        /// <summary>
        /// Estado para doar
        /// </summary>
        public static string E1_G
        {
            get { return Global_Var.e1_g; }

            set { Global_Var.e1_g = value; }
        }

        /// <summary>
        ///  CPID
        /// </summary>
        public static string CP_G
        {
            get { return Global_Var.cpid_g; }

            set { Global_Var.cpid_g = value; }
        }

        /// <summary>
        ///  MID
        /// </summary>
        public static string MID_G
        {
            get { return Global_Var.mid_g; }

            set { Global_Var.mid_g = value; }
        }

    }
}
