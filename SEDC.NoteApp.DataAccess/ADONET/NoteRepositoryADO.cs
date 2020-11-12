using SEDC.NoteApp.DataModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SEDC.NoteApp.DataAccess.ADONET
{
    public class NoteRepositoryADO : IRepository<NoteDTO>
    {
        private readonly string _connectionString;
        public NoteRepositoryADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(NoteDTO entity)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = $@"INSERT INTO Notes (Text, Color, Tag, UserId) 
VALUES(@noteText, @noteColor, @noteTag, @noteUserId)";
            cmd.Parameters.AddWithValue("@noteText", $"{entity.Text}");
            cmd.Parameters.AddWithValue("@noteColor", $"{entity.Color}");
            cmd.Parameters.AddWithValue("@noteTag", entity.Tag);
            cmd.Parameters.AddWithValue("@noteUserId", entity.UserId);

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public void Delete(NoteDTO entity)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = $"DELETE FROM Notes WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", entity.Id);
            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public IEnumerable<NoteDTO> GetAll()
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = connection;

            cmd.CommandText = "SELECT * FROM Notes";

            SqlDataReader dr = cmd.ExecuteReader();

            List<NoteDTO> notes = new List<NoteDTO>();

            while (dr.Read())
            {
               
                notes.Add(new NoteDTO()
                {
                    Id = (int)dr["Id"],
                    Text = (string)dr["Text"],
                    Color = (string)dr["Color"],
                    Tag = (int)dr["Tag"],
                    UserId = (int)dr["UserId"]
                });
            }

            connection.Close();

            return notes;
        }

        public NoteDTO GetById(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = $"SELECT * FROM Notes WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            SqlDataReader dr = cmd.ExecuteReader();
            NoteDTO note = new NoteDTO();

            while (dr.Read())
            {
                note = new NoteDTO
                {
                    Id = dr.GetFieldValue<int>(0),
                    Text = dr.GetFieldValue<string>(1),
                    Color = dr.GetFieldValue<string>(2),
                    Tag = dr.GetFieldValue<int>(3),
                    UserId = dr.GetFieldValue<int>(4)
                };
            }
            connection.Close();
            return note;
        }

        public void Update(NoteDTO update)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = $@"UPDATE Notes SET Text = @noteText, Color= @noteColor, Tag = @noteTag, UserId= @noteUserId
WHERE Id = @noteId";
            cmd.Parameters.AddWithValue("@noteText", update.Text);
            cmd.Parameters.AddWithValue("@noteColor", update.Color);
            cmd.Parameters.AddWithValue("@noteTag", update.Tag);
            cmd.Parameters.AddWithValue("@noteUserId", update.UserId);
            cmd.Parameters.AddWithValue("@noteId", update.Id);
            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}
