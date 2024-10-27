using MySQLite;

namespace SQLiteTest
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        readonly static string DatabaseFileName 
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "db.db");
        
        MySQLite.SQLiteConnection conn;

        private void CreateDatabase(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(DatabaseFileName))
                {
                    SetInfoLabel("Deleting existing database...");
                    File.Delete(DatabaseFileName);
                }

                var options = new MySQLite.SQLiteConnectionString(DatabaseFileName,
                    storeDateTimeAsTicks: true,
                    key: "password",
                    postKeyAction: db => db.Execute("PRAGMA cipher_compatibility = 3;")
                );

                conn = new SQLiteConnection(options);
                SetLabelAfterAction($"Created DB successfully.", true);
            }
            catch (Exception ex)
            {
                SetLabelAfterAction($"Failed to create DB. Exception:{Environment.NewLine}'{ex}'", false);
            }
        }

        private void OpenDatabase(object sender, EventArgs e)
        {
            try
            {
                var options = new SQLiteConnectionString(DatabaseFileName,
                    storeDateTimeAsTicks: true,
                    key: "password",
                    postKeyAction: c =>  c.Execute("PRAGMA cipher_compatibility = 3")
                );

                conn = new SQLiteConnection(options);
                SetLabelAfterAction($"Opened DB 'successfully'.", true);
            }
            catch (Exception ex)
            {
                SetLabelAfterAction($"Failed to open DB. Exception:{Environment.NewLine}'{ex}'", false);
            }
        }

        private void TryOpenDatabaseWithoutPassword(object sender, EventArgs e)
        {
            try
            {
                var options = new SQLiteConnectionString(DatabaseFileName,
                    storeDateTimeAsTicks: true);

                conn.Close();
                conn = null;
                conn = new SQLiteConnection(options);
            }
            catch (Exception ex)
            {
                SetLabelAfterAction($"Failed to open DB without password. Exception:{Environment.NewLine}'{ex}'", false);
            }
        }
        private void CreateDatabaseRecord(object sender, EventArgs e)
        {
            try
            {
                conn.CreateTable(typeof(MyDatabaseRecord));
                conn.Insert(new MyDatabaseRecord 
                { 
                    Value = DateTime.UtcNow.ToString()
                });

                SetLabelAfterAction($"Added record successfully. Total record count is {conn.Table<MyDatabaseRecord>().Count()}", true);
            }
            catch (Exception ex)
            {
                SetLabelAfterAction($"Failed to add record to DB. Exception:{Environment.NewLine}'{ex}'", false);
            }
        }

        private void SetInfoLabel(string message)
        {
            InfoLabel.Text = message;
        }

        private void SetLabelAfterAction(string message, bool success)
        {
            InfoLabel.Text = "";
            SuccessLabel.Text = "";
            ExceptionLabel.Text = "";

            var labelToSet = success ? SuccessLabel : ExceptionLabel;

            labelToSet.Text = message;
        }
    }

    public class MyDatabaseRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PrimaryKey { get; set; }

        public string? Value { get; set; }
    }
}
