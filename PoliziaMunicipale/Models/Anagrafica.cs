using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoliziaMunicipale.Models
{
    public class Anagrafica
    {
        [ScaffoldColumn(false)]
        public int IdAnagrafica { get; set; }

        [Required]
        [Display(Name = "Cognome")]
        public string Cognome { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Indirizzo")]
        public string Indirizzo { get; set; }

        [Required]
        [Display(Name = "Città")]
        public string Citta { get; set; }

        [Required]
        
        [Display(Name = "CAP")]
        public string CAP { get; set; }

        [Required]
        [StringLength(16)]
        [Display(Name = "Codice Fiscale")]
        public string CodiceFiscale { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Numero Punti Attuali")]
        public int Punti { get; set; }

        [ScaffoldColumn(false)]
        public string fullIdAngrafica => $"{IdAnagrafica} {Nome} {Cognome} {CodiceFiscale}";



        
        public Anagrafica() { }

      
        public Anagrafica(int idAnagrafica, string cognome, string nome, string indirizzo, string citta, string cap, string codiceFiscale, int punti)
        {
            IdAnagrafica = idAnagrafica;
            Cognome = cognome;
            Nome = nome;
            Indirizzo = indirizzo;
            Citta = citta;
            CAP = cap;
            CodiceFiscale = codiceFiscale;
            Punti = punti;
        }

        //Metodo per l'inserimento sul DB di una nuova Anagrafica
        public static void InserisciNuovaAnagrafica(Anagrafica nuovaAnagrafica)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ANAGRAFICA (Cognome, Nome, Indirizzo, Citta, CAP, Cod_Fisc, Punti) VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @CAP, @CodiceFiscale, @Punti)", conn);
                cmd.Parameters.AddWithValue("@Cognome", nuovaAnagrafica.Cognome);
                cmd.Parameters.AddWithValue("@Nome", nuovaAnagrafica.Nome);
                cmd.Parameters.AddWithValue("@Indirizzo", nuovaAnagrafica.Indirizzo);
                cmd.Parameters.AddWithValue("@Citta", nuovaAnagrafica.Citta);
                cmd.Parameters.AddWithValue("@CAP", nuovaAnagrafica.CAP);
                cmd.Parameters.AddWithValue("@CodiceFiscale", nuovaAnagrafica.CodiceFiscale);
                cmd.Parameters.AddWithValue("@Punti", nuovaAnagrafica.Punti);

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

        //metodo che sottrae i punti di un verbale alla persona indicata tramite id quando si crea un verbale
        //Non elimina punti sotto lo zero. Quando i punti non bastano porta questi a zero
        public static void SottraiPunti(int idAnagrafica, int puntiDaSottrarre)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();  
                    SqlCommand cmd = new SqlCommand("UPDATE ANAGRAFICA SET Punti = CASE WHEN Punti >= @PuntiDaSottrarre THEN Punti - @PuntiDaSottrarre ELSE 0 END WHERE IdAnagrafica = @IdAnagrafica", conn);
                    cmd.Parameters.AddWithValue("@PuntiDaSottrarre", puntiDaSottrarre);
                    cmd.Parameters.AddWithValue("@IdAnagrafica", idAnagrafica);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine($"Sottrazione di {puntiDaSottrarre} punti avvenuta con successo per l'anagrafica con ID {idAnagrafica}.");
                    else
                        Console.WriteLine($"L'anagrafica con ID {idAnagrafica} non è stata trovata.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Metodo che crea una lista di tutte le angrafiche presenti sul DB e la restituisce
        public static List<Anagrafica> GetAllAnagrafiche()
        {
            List<Anagrafica> anagrafiche = new List<Anagrafica>();

            string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM ANAGRAFICA", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Anagrafica anagrafica = new Anagrafica
                        {
                            IdAnagrafica = Convert.ToInt32(reader["IdAnagrafica"]),
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Indirizzo = reader["Indirizzo"].ToString(),
                            Citta = reader["Citta"].ToString(),
                            CAP = reader["CAP"].ToString(),
                            CodiceFiscale = reader["Cod_Fisc"].ToString(),
                            Punti = Convert.ToInt32(reader["Punti"])
                        };
                        anagrafiche.Add(anagrafica);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                }
                finally
                {
                    conn.Close();
                }
            }

            return anagrafiche;
        }
    }

}