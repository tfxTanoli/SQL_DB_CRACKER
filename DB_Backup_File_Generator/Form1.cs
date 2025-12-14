namespace DB_Backup_File_Generator
{
    public partial class Form1 : Form
    {
        private TextBox txtConnectionString;

        public Form1()
        {
            InitializeComponent();
            SetupControls();
        }

        private void SetupControls()
        {
            // Form Styling
            this.Text = "DB Backup Generator";
            this.Size = new Size(600, 300);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "Database Connection Setup";
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.FromArgb(0, 51, 102); // Dark professional blue
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 20);
            this.Controls.Add(lblTitle);

            // Connection String Label
            Label lblConnectionString = new Label();
            lblConnectionString.Text = "Connection String:";
            lblConnectionString.AutoSize = true;
            lblConnectionString.Location = new Point(32, 70);
            lblConnectionString.ForeColor = Color.FromArgb(64, 64, 64);
            this.Controls.Add(lblConnectionString);

            // Connection String TextBox
            txtConnectionString = new TextBox();
            txtConnectionString.Location = new Point(35, 95);
            txtConnectionString.Width = 500;
            txtConnectionString.Font = new Font("Consolas", 10F); // Monospace for technical text
            this.Controls.Add(txtConnectionString);

            // Test Button
            Button btnTest = new Button();
            btnTest.Text = "Test Connection";
            btnTest.Size = new Size(160, 40);
            btnTest.Location = new Point(35, 140);
            btnTest.BackColor = Color.FromArgb(0, 102, 204);
            btnTest.ForeColor = Color.White;
            btnTest.FlatStyle = FlatStyle.Flat;
            btnTest.FlatAppearance.BorderSize = 0;
            btnTest.Cursor = Cursors.Hand;
            btnTest.Click += BtnTest_Click;
            this.Controls.Add(btnTest);

            // Generate Backup Button
            Button btnGenerateBackup = new Button();
            btnGenerateBackup.Text = "Generate .bak file";
            btnGenerateBackup.Size = new Size(160, 40);
            btnGenerateBackup.Location = new Point(210, 140);
            btnGenerateBackup.BackColor = Color.FromArgb(40, 167, 69); // Green color
            btnGenerateBackup.ForeColor = Color.White;
            btnGenerateBackup.FlatStyle = FlatStyle.Flat;
            btnGenerateBackup.FlatAppearance.BorderSize = 0;
            btnGenerateBackup.Cursor = Cursors.Hand;
            btnGenerateBackup.Click += BtnGenerateBackup_Click;
            this.Controls.Add(btnGenerateBackup);

            // Generate Schema Button
            Button btnGenerateSchema = new Button();
            btnGenerateSchema.Text = "Generate Schema";
            btnGenerateSchema.Size = new Size(160, 40);
            btnGenerateSchema.Location = new Point(385, 140);
            btnGenerateSchema.BackColor = Color.FromArgb(255, 140, 0); // Orange color
            btnGenerateSchema.ForeColor = Color.White;
            btnGenerateSchema.FlatStyle = FlatStyle.Flat;
            btnGenerateSchema.FlatAppearance.BorderSize = 0;
            btnGenerateSchema.Cursor = Cursors.Hand;
            btnGenerateSchema.Click += BtnGenerateSchema_Click;
            this.Controls.Add(btnGenerateSchema);

            // Generate Scripts Button
            Button btnGenerateScripts = new Button();
            btnGenerateScripts.Text = "Generate Scripts";
            btnGenerateScripts.Size = new Size(160, 40);
            btnGenerateScripts.Location = new Point(35, 200);
            btnGenerateScripts.BackColor = Color.FromArgb(102, 51, 153); // Purple color
            btnGenerateScripts.ForeColor = Color.White;
            btnGenerateScripts.FlatStyle = FlatStyle.Flat;
            btnGenerateScripts.FlatAppearance.BorderSize = 0;
            btnGenerateScripts.Cursor = Cursors.Hand;
            btnGenerateScripts.Click += BtnGenerateScripts_Click;
            this.Controls.Add(btnGenerateScripts);
        }

        private void BtnTest_Click(object? sender, EventArgs e)
        {
            try
            {
                string connectionString = txtConnectionString.Text;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    MessageBox.Show("Please enter a connection string.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Connection Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection Failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerateBackup_Click(object? sender, EventArgs e)
        {
            try
            {
                string connectionString = txtConnectionString.Text;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    MessageBox.Show("Please enter a connection string first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Microsoft.Data.SqlClient.SqlConnectionStringBuilder builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;

                if (string.IsNullOrEmpty(databaseName))
                {
                    MessageBox.Show("Connection string must specify a database (Initial Catalog).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string backupFolder = @"C:\Backup";
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                string backupFileName = $"{databaseName}.bak";
                string backupFilePath = Path.Combine(backupFolder, backupFileName);

                string query = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupFilePath}' WITH FORMAT, INIT, NAME = '{databaseName}-Full Database Backup';";

                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Backup completed successfully!\nFile saved to:\n{backupFilePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Backup Failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void BtnGenerateSchema_Click(object? sender, EventArgs e)
        {
            try
            {
                string connectionString = txtConnectionString.Text;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    MessageBox.Show("Please enter a connection string first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Microsoft.Data.SqlClient.SqlConnectionStringBuilder builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;

                if (string.IsNullOrEmpty(databaseName))
                {
                    MessageBox.Show("Connection string must specify a database (Initial Catalog).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string backupFolder = @"C:\Backup";
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                string schemaFileName = $"{databaseName}_Schema.txt";
                string schemaFilePath = Path.Combine(backupFolder, schemaFileName);

                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Query to get tables and columns
                    // Order by Table Name, then Ordinal Position
                    string query = @"
                        SELECT 
                            t.TABLE_SCHEMA,
                            t.TABLE_NAME,
                            c.COLUMN_NAME,
                            c.DATA_TYPE,
                            c.CHARACTER_MAXIMUM_LENGTH,
                            c.IS_NULLABLE
                        FROM 
                            INFORMATION_SCHEMA.TABLES t
                        INNER JOIN 
                            INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME AND t.TABLE_SCHEMA = c.TABLE_SCHEMA
                        WHERE 
                            t.TABLE_TYPE = 'BASE TABLE'
                        ORDER BY 
                            t.TABLE_SCHEMA, t.TABLE_NAME, c.ORDINAL_POSITION";

                    using (Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                    using (Microsoft.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                    {
                        using (StreamWriter writer = new StreamWriter(schemaFilePath))
                        {
                            writer.WriteLine($"Database Schema for: {databaseName}");
                            writer.WriteLine($"Generated on: {DateTime.Now}");
                            writer.WriteLine("--------------------------------------------------");
                            writer.WriteLine();

                            string currentTable = "";

                            while (reader.Read())
                            {
                                string schema = reader["TABLE_SCHEMA"].ToString() ?? "dbo";
                                string table = reader["TABLE_NAME"].ToString() ?? "Unknown";
                                string fullTableName = $"[{schema}].[{table}]";

                                if (fullTableName != currentTable)
                                {
                                    if (currentTable != "") writer.WriteLine(); // Spacing
                                    
                                    currentTable = fullTableName;
                                    writer.WriteLine($"TABLE: {fullTableName}");
                                }

                                string column = reader["COLUMN_NAME"].ToString() ?? "Unknown";
                                string type = reader["DATA_TYPE"].ToString() ?? "Unknown";
                                string length = reader["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? $"({reader["CHARACTER_MAXIMUM_LENGTH"]})" : "";
                                string nullable = reader["IS_NULLABLE"].ToString() == "YES" ? "NULL" : "NOT NULL";

                                writer.WriteLine($"   - {column} : {type}{length} [{nullable}]");
                            }
                        }
                    }
                }

                MessageBox.Show($"Schema generated successfully!\nFile saved to:\n{schemaFilePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Schema Generation Failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnGenerateScripts_Click(object? sender, EventArgs e)
        {
            try
            {
                string connectionString = txtConnectionString.Text;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    MessageBox.Show("Please enter a connection string first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Microsoft.Data.SqlClient.SqlConnectionStringBuilder builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                string databaseName = builder.InitialCatalog;

                if (string.IsNullOrEmpty(databaseName))
                {
                    MessageBox.Show("Connection string must specify a database (Initial Catalog).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string backupFolder = @"C:\Backup";
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                string scriptFileName = $"{databaseName}_Scripts.sql";
                string scriptFilePath = Path.Combine(backupFolder, scriptFileName);

                using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to get table list
                    string tableQuery = "SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_SCHEMA, TABLE_NAME";
                    
                    using (StreamWriter writer = new StreamWriter(scriptFilePath))
                    {
                        writer.WriteLine($"-- Database Scripts for: {databaseName}");
                        writer.WriteLine($"-- Generated on: {DateTime.Now}");
                        writer.WriteLine("--------------------------------------------------");
                        writer.WriteLine();

                        var tables = new List<(string Schema, string Name)>();
                        using (Microsoft.Data.SqlClient.SqlCommand tableCmd = new Microsoft.Data.SqlClient.SqlCommand(tableQuery, connection))
                        using (Microsoft.Data.SqlClient.SqlDataReader tableReader = tableCmd.ExecuteReader())
                        {
                            while (tableReader.Read())
                            {
                                tables.Add((tableReader["TABLE_SCHEMA"].ToString() ?? "dbo", tableReader["TABLE_NAME"].ToString() ?? "Unknown"));
                            }
                        }

                        foreach (var table in tables)
                        {
                            string fullTableName = $"[{table.Schema}].[{table.Name}]";
                            writer.WriteLine($"-- Table: {fullTableName}");
                            
                            // 1. CREATE TABLE Script
                            writer.WriteLine($"CREATE TABLE {fullTableName} (");

                            var columnNames = new List<string>();
                            string columnQuery = @"
                                SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE 
                                FROM INFORMATION_SCHEMA.COLUMNS 
                                WHERE TABLE_SCHEMA = @Schema AND TABLE_NAME = @Table 
                                ORDER BY ORDINAL_POSITION";

                            using (Microsoft.Data.SqlClient.SqlCommand colCmd = new Microsoft.Data.SqlClient.SqlCommand(columnQuery, connection))
                            {
                                colCmd.Parameters.AddWithValue("@Schema", table.Schema);
                                colCmd.Parameters.AddWithValue("@Table", table.Name);

                                var columns = new List<string>();
                                using (Microsoft.Data.SqlClient.SqlDataReader colReader = colCmd.ExecuteReader())
                                {
                                    while (colReader.Read())
                                    {
                                        string colName = colReader["COLUMN_NAME"].ToString() ?? "Unknown";
                                        columnNames.Add($"[{colName}]");

                                        string dataType = colReader["DATA_TYPE"].ToString() ?? "Unknown";
                                        string maxLen = colReader["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? $"({colReader["CHARACTER_MAXIMUM_LENGTH"]})" : "";
                                        
                                        // Handling types like nvarchar(-1) which is MAX
                                        if (maxLen == "(-1)") maxLen = "(MAX)";

                                        string nullable = colReader["IS_NULLABLE"].ToString() == "YES" ? "NULL" : "NOT NULL";
                                        
                                        columns.Add($"    [{colName}] {dataType}{maxLen} {nullable}");
                                    }
                                }
                                writer.WriteLine(string.Join(",\n", columns));
                            }
                            writer.WriteLine(");");
                            writer.WriteLine("GO");

                            // 2. INSERT DATA Scripts
                            writer.WriteLine($"-- Data for: {fullTableName}");

                            // Check Identity
                            bool hasIdentity = false;
                            string funcQuery = $"SELECT OBJECTPROPERTY(OBJECT_ID('{fullTableName}'), 'TableHasIdentity')";
                            using (Microsoft.Data.SqlClient.SqlCommand funcCmd = new Microsoft.Data.SqlClient.SqlCommand(funcQuery, connection))
                            {
                                var res = funcCmd.ExecuteScalar();
                                if (res != null && res.ToString() == "1") hasIdentity = true;
                            }

                            if (hasIdentity) writer.WriteLine($"SET IDENTITY_INSERT {fullTableName} ON;");

                            string selectQuery = $"SELECT * FROM {fullTableName}";
                            using (Microsoft.Data.SqlClient.SqlCommand dataCmd = new Microsoft.Data.SqlClient.SqlCommand(selectQuery, connection))
                            using (Microsoft.Data.SqlClient.SqlDataReader dataReader = dataCmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var values = new List<string>();
                                    for (int i = 0; i < dataReader.FieldCount; i++)
                                    {
                                        object val = dataReader.GetValue(i);
                                        if (val == DBNull.Value)
                                        {
                                            values.Add("NULL");
                                        }
                                        else if (val is bool b)
                                        {
                                            values.Add(b ? "1" : "0");
                                        }
                                        else if (val is byte[] bytes)
                                        {
                                            values.Add("0x" + BitConverter.ToString(bytes).Replace("-", ""));
                                        }
                                        else if (val is DateTime d)
                                        {
                                            values.Add($"'{d:yyyy-MM-dd HH:mm:ss.fff}'");
                                        }
                                        else
                                        {
                                            // Handle strings and others by escaping quotes
                                            values.Add($"'{val.ToString().Replace("'", "''")}'");
                                        }
                                    }

                                    string cols = string.Join(", ", columnNames);
                                    string vals = string.Join(", ", values);
                                    writer.WriteLine($"INSERT INTO {fullTableName} ({cols}) VALUES ({vals});");
                                }
                            }

                            if (hasIdentity) writer.WriteLine($"SET IDENTITY_INSERT {fullTableName} OFF;");
                            writer.WriteLine("GO");
                            writer.WriteLine();
                        }
                    }
                }

                MessageBox.Show($"Scripts (Structure + Data) generated successfully!\nFile saved to:\n{scriptFilePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Script Generation Failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
