using AulaNova.Models;
using Microsoft.Data.Sqlite;
namespace AulaNova.Data;

public sealed class AlumnoRepository
{
    private readonly string connectionString;
    private readonly string schemaPath;
    public AlumnoRepository(IWebHostEnvironment env, IConfiguration config)
    {
        var path = Path.GetFullPath(config["Database:Path"] ?? "App_Data/aula-nova.db", env.ContentRootPath);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        connectionString = new SqliteConnectionStringBuilder { DataSource = path, ForeignKeys = true }.ToString();
        schemaPath = Path.Combine(env.ContentRootPath, "Database", "001_initial.sql");
    }
    private SqliteConnection Open() { var c = new SqliteConnection(connectionString); c.Open(); return c; }
    public void Initialize() { using var c = Open(); using var q = c.CreateCommand(); q.CommandText = File.ReadAllText(schemaPath); q.ExecuteNonQuery(); }
    public List<AlumnoModel> All()
    {
        using var c = Open(); using var q = c.CreateCommand();
        q.CommandText = "SELECT id,nombres,apellidos,documento,correo,telefono,carrera,ciclo,estado,observaciones,fecha_registro FROM alumnos ORDER BY id DESC";
        using var v = q.ExecuteReader(); var rows = new List<AlumnoModel>();
        while (v.Read()) rows.Add(new AlumnoModel { Id = v.GetInt32(0), Nombres = v.GetString(1), Apellidos = v.GetString(2), Documento = v.GetString(3), Correo = v.GetString(4), Telefono = v.IsDBNull(5) ? null : v.GetString(5), Carrera = v.GetString(6), Ciclo = v.GetInt32(7), Estado = v.GetString(8), Observaciones = v.IsDBNull(9) ? null : v.GetString(9), FechaRegistro = v.GetString(10) });
        return rows;
    }
    public AlumnoModel? Find(int id) => All().FirstOrDefault(a => a.Id == id);
    public bool Save(AlumnoModel a, bool update)
    {
        using var c = Open(); using var q = c.CreateCommand();
        q.CommandText = update ? "UPDATE alumnos SET nombres=$n,apellidos=$a,documento=$d,correo=$c,telefono=$t,carrera=$r,ciclo=$i,estado=$e,observaciones=$o WHERE id=$id" : "INSERT INTO alumnos(nombres,apellidos,documento,correo,telefono,carrera,ciclo,estado,observaciones) VALUES($n,$a,$d,$c,$t,$r,$i,$e,$o)";
        q.Parameters.AddWithValue("$id", a.Id);
        q.Parameters.AddWithValue("$n", a.Nombres.Trim()); q.Parameters.AddWithValue("$a", a.Apellidos.Trim());
        q.Parameters.AddWithValue("$d", a.Documento.Trim()); q.Parameters.AddWithValue("$c", a.Correo.Trim().ToLowerInvariant());
        q.Parameters.AddWithValue("$t", (object?)a.Telefono?.Trim() ?? DBNull.Value); q.Parameters.AddWithValue("$r", a.Carrera);
        q.Parameters.AddWithValue("$i", a.Ciclo); q.Parameters.AddWithValue("$e", a.Estado);
        q.Parameters.AddWithValue("$o", (object?)a.Observaciones?.Trim() ?? DBNull.Value);
        return q.ExecuteNonQuery() == 1;
    }
    public bool Delete(int id) { using var c = Open(); using var q = c.CreateCommand(); q.CommandText = "DELETE FROM alumnos WHERE id=$id"; q.Parameters.AddWithValue("$id", id); return q.ExecuteNonQuery() == 1; }
}
