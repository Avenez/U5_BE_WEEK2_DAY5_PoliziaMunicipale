using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;

public class TipoViolazione
{
    [ScaffoldColumn(false)]
    public int IdViolazione { get; set; }
    public string Descrizione { get; set; }

    public bool Contestabile { get; set; }

    // Costruttori
    public TipoViolazione() { }

    public TipoViolazione(int idViolazione, string descrizione)
    {
        IdViolazione = idViolazione;
        Descrizione = descrizione;
    }

    public TipoViolazione(int idViolazione, string descrizione, bool contestabile)
    {
        IdViolazione = idViolazione;
        Descrizione = descrizione;
        Contestabile = contestabile;
    }



    // Metodo per inserire un nuovo record nella tabella che prende un TipoViolazione e lo inserisce nel DB
    public static void InserisciNuovoTipoViolazione(TipoViolazione nuovoTipoViolazione)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
        SqlConnection conn = new SqlConnection(connectionString);

        try
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO TIPO_VIOLAZIONE (descrizione, contestabile) VALUES (@Descrizione, @Contestabile)", conn);
            cmd.Parameters.AddWithValue("@Descrizione", nuovoTipoViolazione.Descrizione);
            cmd.Parameters.AddWithValue("@Contestabile", nuovoTipoViolazione.Contestabile);

            cmd.ExecuteNonQuery();

            Console.WriteLine("Inserimento avvenuto con Successo");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            conn.Close();
        }
    }

    //Metodo che seleziona tutte le violazioni presenti sul DB
    //Crea una lista di violazioni e la restituisce
    public static List<TipoViolazione> GetTipoViolazioni()
    {
        List<TipoViolazione> tipoViolazioni = new List<TipoViolazione>();

        string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM TIPO_VIOLAZIONE";

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                TipoViolazione tipoViolazione = new TipoViolazione
                {
                    IdViolazione = Convert.ToInt32(reader["idviolazione"]),
                    Descrizione = reader["descrizione"].ToString(),
                    Contestabile = reader.GetBoolean(reader.GetOrdinal("Contestabile"))
                };
                tipoViolazioni.Add(tipoViolazione);
            }

            reader.Close();
        }

        return tipoViolazioni;
    }

}
