using System.Data;
using Microsoft.Data.SqlClient;
using MVCPracticaFinalCRUD.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVCPracticaFinalCRUD.Repositories
{
    public class RepositoryDoctor
    {
        private DataTable tablaDoctores;
        SqlConnection cn;
        SqlCommand com;
        public RepositoryDoctor()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from DOCTOR";
            SqlDataAdapter adDoc = new SqlDataAdapter(sql, connectionString);
            this.tablaDoctores = new DataTable();
            adDoc.Fill(this.tablaDoctores);

            //para consultas adonet
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable() select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor();

                doc.IdHospital = row.Field<int>("HOSPITAL_COD");
                doc.IdDoctor = row.Field<int>("DOCTOR_NO");
                doc.Apellido = row.Field<string>("APELLIDO");
                doc.Especialidad = row.Field<string>("ESPECIALIDAD");
                doc.Salario = row.Field<int>("SALARIO");
              
                doctores.Add(doc);
            }
            return doctores;
        }

        public Doctor DetalleDoctor(int IdDoctor)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable() where datos.Field<int>("DOCTOR_NO") == IdDoctor select datos;
            var row = consulta.First();
            Doctor doc = new Doctor
            {
                IdHospital = row.Field<int>("HOSPITAL_COD"),
                IdDoctor = row.Field<int>("DOCTOR_NO"),
                Apellido = row.Field<string>("APELLIDO"),
                Especialidad = row.Field<string>("ESPECIALIDAD"),
                Salario = row.Field<int>("SALARIO"),
            };
            return doc;
        }

        public void Delete(int IdDoctor)
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            string sql = "delete from DOCTOR where DOCTOR_NO=@IdDoctor";

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            this.com.Parameters.AddWithValue("@IdDoctor", IdDoctor);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public async Task CreateDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad, int salario)
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            string sql = "insert into DOCTOR values(@idHospital, @idDoctor, @apellido, @especialidad, @salario)";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.Parameters.AddWithValue("@idDoctor", idDoctor);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public List<string> GetEspecialidades()
        {
            var consulta = (from datos in this.tablaDoctores.AsEnumerable() select datos.Field<string>("ESPECIALIDAD")).Distinct();
            return consulta.ToList();
        }

        public List<Doctor> GetDoctorEspecialidad(string especialidad)
        {
            var consulta = from datos in this.tablaDoctores.AsEnumerable() where datos.Field<string>("ESPECIALIDAD") == especialidad select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doctor = new Doctor
                {
                    IdHospital = row.Field<int>("HOSPITAL_COD"),
                    IdDoctor = row.Field<int>("DOCTOR_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Especialidad = row.Field<string>("ESPECIALIDAD"),
                    Salario = row.Field<int>("SALARIO"),
                };
                doctores.Add(doctor);
            }
            return doctores;
        }

        public async Task UpdateDoctorAsync(int idHospital, int idDoctor, string apellido, string especialidad, int salario)
        {
            string sql = "update DOCTOR set HOSPITAL_COD=@idHospital, DOCTOR_NO=@idDoctor, APELLIDO=@apellido, ESPECIALIDAD=@especialidad, SALARIO=@salario where DOCTOR_NO=@idDoctor";
            this.com.Parameters.AddWithValue("@idDoctor", idDoctor);
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
    
}
