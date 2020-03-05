using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Colegio
{
    public partial class Grado : Form
    {
        public String conexion;
        public System.Data.IsolationLevel lectura;
        public System.Data.IsolationLevel escritura;
        public Grado()
        {
            conexion = "DATA SOURCE = " + Properties.Settings.Default.nombre_db + "; PASSWORD=" + Properties.Settings.Default.contrasenia_db + "; USER ID=" + Properties.Settings.Default.usuario_db + ";";
            InitializeComponent();
            lectura = IsolationLevel.ReadCommitted;
            escritura = IsolationLevel.ReadCommitted;
        }

        private void Grado_Load(object sender, EventArgs e)
        {
            cargar_profesores();
            cargar_datos();
        }

        public void cargar_profesores()
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("profesor_select", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(lectura);
                comando.Transaction = transaction;
                try
                {

                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("registros", OracleType.Cursor).Direction = ParameterDirection.Output;
                    OracleDataAdapter adaptador = new OracleDataAdapter();
                    adaptador.SelectCommand = comando;
                    transaction.Commit();
                    DataTable tabla1 = new DataTable();
                    adaptador.Fill(tabla1);
                    comboBox1.DataSource = tabla1;
                    comboBox1.DisplayMember = "nombre";
                    comboBox1.ValueMember = "idprofesor";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("algo salio mal");
                    transaction.Rollback();
                }
            }
        }
        public void cargar_datos()
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("grado_select", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(lectura);
                comando.Transaction = transaction;
                try
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("registros", OracleType.Cursor).Direction = ParameterDirection.Output;
                    OracleDataAdapter adaptador = new OracleDataAdapter();
                    adaptador.SelectCommand = comando;
                    transaction.Commit();
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    dataGridView1.DataSource = tabla;

                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    MessageBox.Show("algo salio mal");
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //gets a collection that contains all the rows
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                id.Text = row.Cells[0].Value.ToString();
                nombre.Text = row.Cells[1].Value.ToString();
                comboBox1.SelectedIndex = comboBox1.FindStringExact(row.Cells[2].Value.ToString());

            }
        }

        private void eliminar_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("grado_delete", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(escritura);
                comando.Transaction = transaction;
                OracleConnection ora = new OracleConnection(conexion);
                try
                {

                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("pid_grado", OracleType.Number).Value = Convert.ToInt32(id.Text);
                    comando.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("Grado eliminado correctamente");

                }
                catch (Exception)
                {
                    MessageBox.Show("Algo fallo");
                    transaction.Rollback();
                }
            }
            cargar_datos();
        }

        private void editar_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("grado_actualizar", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(escritura);
                comando.Transaction = transaction;
                try
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("cod", OracleType.Number).Value = Convert.ToInt32(id.Text);
                    comando.Parameters.Add("namee", OracleType.VarChar).Value = nombre.Text;
                    comando.Parameters.Add("profesor", OracleType.Number).Value = Convert.ToInt32(comboBox1.SelectedValue.ToString());

                    comando.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("grado modificado correctamente");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    MessageBox.Show("Algo fallo");
                }
            }
            cargar_datos();
        }

        private void agregar_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("grado_crear", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(escritura);
                comando.Transaction = transaction;
                OracleConnection ora = new OracleConnection(conexion);
                try
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("cod", OracleType.Number).Value = Convert.ToInt32(id.Text);
                    comando.Parameters.Add("namee", OracleType.VarChar).Value = nombre.Text;
                    comando.Parameters.Add("profesor", OracleType.Number).Value = Convert.ToInt32(comboBox1.SelectedValue.ToString());
                    comando.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("grado creado correctamente");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    MessageBox.Show("Algo fallo");
                }

            }
            cargar_datos();
        }

        private void nombre_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
