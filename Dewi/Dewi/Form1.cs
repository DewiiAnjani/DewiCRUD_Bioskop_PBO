using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Dewi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class DatabaseHelpers
        {
            string connString = "Host=localhost;Username=postgres;Password=dewi2493;Database=Bioskop";
            NpgsqlConnection conn;

            public void connect()
            {
                if (conn == null)
                {
                    conn = new NpgsqlConnection(connString);
                }
                conn.Open();
            }

            public DataTable getData(string sql)
            {
                DataTable table = new DataTable();
                connect();
                try
                {
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn);
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    conn.Close();
                }
                return table;
            }

            public void exc(String sql)
            {
                connect();
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch
                {

                }
                finally
                {
                    conn.Close();
                }
            }
        }
        DatabaseHelpers db = new DatabaseHelpers();

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = "Aksi";
            getDataFilm();
        }

        private void getDataFilm()
        {
            string sql = "SELECT * FROM film";
            dataGridView1.DataSource = db.getData(sql);
            
            dataGridView1.Columns["id_film"].HeaderText = "ID Film";
            dataGridView1.Columns["judul"].HeaderText = "Judul";
            dataGridView1.Columns["genre"].HeaderText = "Genre";
            dataGridView1.Columns["negara"].HeaderText = "Negara";
            dataGridView1.Columns["tanggal"].HeaderText = "Tanggal";
            dataGridView1.Columns["Edit"].DisplayIndex = 6;
            dataGridView1.Columns["Delete"].DisplayIndex = 6;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "INSERT FILM", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string sql = $"INSERT INTO film(id_film, judul, genre, negara, tanggal) VALUES ({textBox1.Text},'{textBox2.Text}','{comboBox1.SelectedItem}','{textBox4.Text}','{dateTimePicker1.Text}')";
                MessageBox.Show("Berhasil!");
                db.exc(sql);
                getDataFilm();
                button3.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Gagal!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "UPDATE FILM", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string sql = $"update film set judul = '{textBox2.Text}', genre = '{comboBox1.SelectedItem}', negara = '{textBox4.Text}', tanggal = '{dateTimePicker1.Text}' where id_film = {textBox1.Text}";
                MessageBox.Show("Berhasil!");
                db.exc(sql);
                getDataFilm();
                button3.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Gagal!");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                button1.Enabled = false;
                textBox1.Enabled = false;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["id_film"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["judul"].Value.ToString();
                comboBox1.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["genre"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["negara"].Value.ToString();
                dateTimePicker1.Text = dataGridView1.Rows[e.RowIndex].Cells["tanggal"].Value.ToString();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                var id_film = dataGridView1.Rows[e.RowIndex].Cells["id_film"].Value.ToString();
                string sql = $"delete from film where id_film = {id_film}";

                DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "DELETE FILM", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show("Berhasil!");
                    db.exc(sql);
                    getDataFilm();
                    button3.PerformClick();
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Gagal!");
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            button1.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedItem = "Aksi";
            textBox4.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToString();
        }
    }
}
