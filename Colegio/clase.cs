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
    public partial class clase : Form
    {
        public String conexion;
        public System.Data.IsolationLevel lectura;
        public System.Data.IsolationLevel escritura;
        public clase()
        {
            conexion = "DATA SOURCE = " + Properties.Settings.Default.nombre_db + "; PASSWORD=" + Properties.Settings.Default.contrasenia_db + "; USER ID=" + Properties.Settings.Default.usuario_db + ";";
            InitializeComponent();
            lectura = IsolationLevel.ReadCommitted;
            escritura = IsolationLevel.ReadCommitted;
        }

        private void clase_Load(object sender, EventArgs e)
        {
            cargar_datos();
            cargar_alumnos();
            cargar_grado();
        }

        public void cargar_alumnos()
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("prueba_select", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(lectura);
                comando.Transaction = transaction;
                /*try
                {*/

                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("registros", OracleType.Cursor).Direction = ParameterDirection.Output;
                    OracleDataAdapter adaptador = new OracleDataAdapter();
                    adaptador.SelectCommand = comando;
                    transaction.Commit();
                    DataTable tabla1 = new DataTable();
                    adaptador.Fill(tabla1);
                    comboBox1.DataSource = tabla1;
                    comboBox1.DisplayMember = "nombre";
                    comboBox1.ValueMember = "idalumno";
                /*}
                catch (Exception ex)
                {
                    MessageBox.Show("algo salio mal");
                    transaction.Rollback();
                }*/
            }
        }
        public void cargar_grado()
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
                    DataTable tabla1 = new DataTable();
                    adaptador.Fill(tabla1);
                    comboBox2.DataSource = tabla1;
                    comboBox2.DisplayMember = "nombre";
                    comboBox2.ValueMember = "idgrado";
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
                OracleCommand comando = new OracleCommand("clase_select", connection);
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

        private void eliminar_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("clase_delete", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(escritura);
                comando.Transaction = transaction;
                OracleConnection ora = new OracleConnection(conexion);
                try
                {

                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("pid_clase", OracleType.Number).Value = Convert.ToInt32(id.Text);
                    comando.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("elemento eliminado correctamente");

                }
                catch (Exception)
                {
                    MessageBox.Show("Algo fallo");
                    transaction.Rollback();
                }
            }
            cargar_datos();
        }

        private void agregar_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("clase_crear", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(escritura);
                comando.Transaction = transaction;
                OracleConnection ora = new OracleConnection(conexion);
                try
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("cod", OracleType.Number).Value = Convert.ToInt32(id.Text);
                    comando.Parameters.Add("seccionn", OracleType.VarChar).Value = seccion.Text;
                    comando.Parameters.Add("grado", OracleType.Number).Value = Convert.ToInt32(comboBox2.SelectedValue.ToString());
                    comando.Parameters.Add("alumno", OracleType.Number).Value = Convert.ToInt32(comboBox1.SelectedValue.ToString());
                    comando.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("Profesor creado correctamente");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    MessageBox.Show("Algo fallo");
                }

            }
            cargar_datos();
        }

        private void editar_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(conexion))
            {
                connection.Open();
                OracleCommand comando = new OracleCommand("clase_actualizar", connection);
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(escritura);
                comando.Transaction = transaction;
                try
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add("cod", OracleType.Number).Value = Convert.ToInt32(id.Text);
                    comando.Parameters.Add("seccionn", OracleType.VarChar).Value = seccion.Text;
                    comando.Parameters.Add("alumno", OracleType.VarChar).Value = Convert.ToInt32(comboBox1.SelectedValue.ToString());
                    comando.Parameters.Add("grado", OracleType.Number).Value = Convert.ToInt32(comboBox2.SelectedValue.ToString());

                    comando.ExecuteNonQuery();
                    transaction.Commit();
                    MessageBox.Show("Profesor modificado correctamente");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    MessageBox.Show("Algo fallo");
                }
            }
            cargar_datos();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //gets a collection that contains all the rows
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                id.Text = row.Cells[0].Value.ToString();
                seccion.Text = row.Cells[3].Value.ToString();
                comboBox1.SelectedIndex = comboBox1.FindStringExact(row.Cells[1].Value.ToString());
                comboBox2.SelectedIndex = comboBox2.FindStringExact(row.Cells[2].Value.ToString());

            }
        }
    }
}
