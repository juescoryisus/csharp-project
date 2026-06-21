using System.Data;
using Microsoft.Data.SqlClient;
using MySqlConnector;

namespace WinFormsApp;

// ======================================================================
// Formulario CRUD sobre la tabla Clientes.
// Permite elegir el motor (SQL Server o MariaDB) y realizar:
//   - SELECT  (cargar en el DataGridView)
//   - INSERT  (agregar)
//   - UPDATE  (actualizar)
//   - DELETE  (eliminar)
// ======================================================================
public class MainForm : Form
{
    // --- Cadenas de conexion hacia los contenedores Docker ---
    private const string SqlServerConn =
        "Server=localhost,1433;Database=TiendaDB;User Id=sa;Password=Passw0rd!2024;TrustServerCertificate=True;";
    private const string MariaDbConn =
        "Server=localhost;Port=3306;Database=TiendaDB;User Id=root;Password=Passw0rd!2024;";

    // --- Controles ---
    private readonly ComboBox cmbMotor = new();
    private readonly DataGridView dgv = new();
    private readonly TextBox txtNombre = new();
    private readonly TextBox txtEmail = new();
    private readonly TextBox txtTelefono = new();
    private readonly TextBox txtCiudad = new();
    private readonly Button btnCargar = new();
    private readonly Button btnInsertar = new();
    private readonly Button btnActualizar = new();
    private readonly Button btnEliminar = new();
    private readonly Label lblEstado = new();

    private int _selectedId = -1;
    private bool EsSqlServer => cmbMotor.SelectedIndex == 0;

    public MainForm()
    {
        Text = "CRUD Clientes - SQL Server / MariaDB";
        Width = 900;
        Height = 600;
        StartPosition = FormStartPosition.CenterScreen;

        // --- Selector de motor ---
        var lblMotor = new Label { Text = "Motor:", Left = 20, Top = 20, Width = 50 };
        cmbMotor.Left = 75; cmbMotor.Top = 17; cmbMotor.Width = 150;
        cmbMotor.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMotor.Items.AddRange(new object[] { "SQL Server", "MariaDB" });
        cmbMotor.SelectedIndex = 0;

        btnCargar.Text = "SELECT (Cargar)";
        btnCargar.Left = 240; btnCargar.Top = 16; btnCargar.Width = 130;
        btnCargar.Click += (_, _) => CargarDatos();

        // --- DataGridView ---
        dgv.Left = 20; dgv.Top = 60; dgv.Width = 840; dgv.Height = 300;
        dgv.ReadOnly = true;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.MultiSelect = false;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.SelectionChanged += Dgv_SelectionChanged;

        // --- Campos de edicion ---
        int top = 380;
        AddField("Nombre:", txtNombre, 20, top);
        AddField("Email:", txtEmail, 300, top);
        AddField("Telefono:", txtTelefono, 20, top + 40);
        AddField("Ciudad:", txtCiudad, 300, top + 40);

        // --- Botones CRUD ---
        btnInsertar.Text = "INSERT";
        btnInsertar.Left = 600; btnInsertar.Top = top; btnInsertar.Width = 110;
        btnInsertar.Click += (_, _) => Insertar();

        btnActualizar.Text = "UPDATE";
        btnActualizar.Left = 720; btnActualizar.Top = top; btnActualizar.Width = 110;
        btnActualizar.Click += (_, _) => Actualizar();

        btnEliminar.Text = "DELETE";
        btnEliminar.Left = 600; btnEliminar.Top = top + 40; btnEliminar.Width = 110;
        btnEliminar.Click += (_, _) => Eliminar();

        lblEstado.Left = 20; lblEstado.Top = top + 90; lblEstado.Width = 840;
        lblEstado.ForeColor = Color.DarkBlue;

        Controls.AddRange(new Control[]
        {
            lblMotor, cmbMotor, btnCargar, dgv,
            btnInsertar, btnActualizar, btnEliminar, lblEstado
        });

        Load += (_, _) => CargarDatos();
    }

    private void AddField(string etiqueta, TextBox txt, int left, int top)
    {
        var lbl = new Label { Text = etiqueta, Left = left, Top = top + 3, Width = 70 };
        txt.Left = left + 75; txt.Top = top; txt.Width = 180;
        Controls.Add(lbl);
        Controls.Add(txt);
    }

    // -------------------- SELECT --------------------
    private void CargarDatos()
    {
        try
        {
            var dt = new DataTable();
            const string sql = "SELECT ClienteId, Nombre, Email, Telefono, Ciudad FROM Clientes ORDER BY ClienteId;";

            if (EsSqlServer)
            {
                using var conn = new SqlConnection(SqlServerConn);
                using var da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            else
            {
                using var conn = new MySqlConnection(MariaDbConn);
                using var da = new MySqlDataAdapter(sql, conn);
                da.Fill(dt);
            }

            dgv.DataSource = dt;
            lblEstado.Text = $"SELECT ejecutado en {cmbMotor.Text}: {dt.Rows.Count} registros.";
        }
        catch (Exception ex)
        {
            MostrarError(ex);
        }
    }

    private void Dgv_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgv.CurrentRow?.Cells["ClienteId"].Value is null) return;
        _selectedId = Convert.ToInt32(dgv.CurrentRow.Cells["ClienteId"].Value);
        txtNombre.Text = dgv.CurrentRow.Cells["Nombre"].Value?.ToString() ?? "";
        txtEmail.Text = dgv.CurrentRow.Cells["Email"].Value?.ToString() ?? "";
        txtTelefono.Text = dgv.CurrentRow.Cells["Telefono"].Value?.ToString() ?? "";
        txtCiudad.Text = dgv.CurrentRow.Cells["Ciudad"].Value?.ToString() ?? "";
    }

    // -------------------- INSERT --------------------
    private void Insertar()
    {
        try
        {
            const string sql = @"INSERT INTO Clientes (Nombre, Email, Telefono, Ciudad, FechaRegistro)
                                 VALUES (@nombre, @email, @telefono, @ciudad, @fecha);";
            int filas = EjecutarComando(sql, agregarFecha: true);
            lblEstado.Text = $"INSERT: {filas} registro agregado.";
            CargarDatos();
        }
        catch (Exception ex)
        {
            MostrarError(ex);
        }
    }

    // -------------------- UPDATE --------------------
    private void Actualizar()
    {
        if (_selectedId < 0) { lblEstado.Text = "Seleccione un registro para actualizar."; return; }
        try
        {
            const string sql = @"UPDATE Clientes
                                 SET Nombre = @nombre, Email = @email,
                                     Telefono = @telefono, Ciudad = @ciudad
                                 WHERE ClienteId = @id;";
            int filas = EjecutarComando(sql, agregarId: true);
            lblEstado.Text = $"UPDATE: {filas} registro actualizado.";
            CargarDatos();
        }
        catch (Exception ex)
        {
            MostrarError(ex);
        }
    }

    // -------------------- DELETE --------------------
    private void Eliminar()
    {
        if (_selectedId < 0) { lblEstado.Text = "Seleccione un registro para eliminar."; return; }
        if (MessageBox.Show("Eliminar el registro seleccionado?", "Confirmar",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        try
        {
            const string sql = "DELETE FROM Clientes WHERE ClienteId = @id;";
            int filas;
            if (EsSqlServer)
            {
                using var conn = new SqlConnection(SqlServerConn);
                conn.Open();
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", _selectedId);
                filas = cmd.ExecuteNonQuery();
            }
            else
            {
                using var conn = new MySqlConnection(MariaDbConn);
                conn.Open();
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", _selectedId);
                filas = cmd.ExecuteNonQuery();
            }
            lblEstado.Text = $"DELETE: {filas} registro eliminado.";
            _selectedId = -1;
            CargarDatos();
        }
        catch (Exception ex)
        {
            MostrarError(ex);
        }
    }

    // Helper para INSERT / UPDATE con los mismos parametros.
    private int EjecutarComando(string sql, bool agregarFecha = false, bool agregarId = false)
    {
        if (EsSqlServer)
        {
            using var conn = new SqlConnection(SqlServerConn);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text);
            cmd.Parameters.AddWithValue("@ciudad", txtCiudad.Text);
            if (agregarFecha) cmd.Parameters.AddWithValue("@fecha", DateTime.Today);
            if (agregarId) cmd.Parameters.AddWithValue("@id", _selectedId);
            return cmd.ExecuteNonQuery();
        }
        else
        {
            using var conn = new MySqlConnection(MariaDbConn);
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@telefono", txtTelefono.Text);
            cmd.Parameters.AddWithValue("@ciudad", txtCiudad.Text);
            if (agregarFecha) cmd.Parameters.AddWithValue("@fecha", DateTime.Today);
            if (agregarId) cmd.Parameters.AddWithValue("@id", _selectedId);
            return cmd.ExecuteNonQuery();
        }
    }

    private void MostrarError(Exception ex)
    {
        lblEstado.ForeColor = Color.DarkRed;
        lblEstado.Text = $"Error: {ex.Message}";
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
