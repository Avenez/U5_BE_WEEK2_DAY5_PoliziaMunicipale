using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoliziaMunicipale.Models
{

    public class Verbale
    {
            [ScaffoldColumn(false)]
            public int IdVerbale { get; set; }

            [DisplayName("Data Violazione")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            [Required]
            public DateTime DataViolazione { get; set; }

            [DisplayName("Indirizzo Violazione")]
            [Required]
            public string IndirizzoViolazione { get; set; }

            [DisplayName("Nome Agente")]
            [Required]
            public string NominativoAgente { get; set; }

            [DisplayName("Data Trascrisione")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            [Required]
            public DateTime DataTrascrizioneVerbale { get; set; }

            [DisplayName("Importo")]

            [Required]
            public decimal Importo { get; set; }

            [DisplayName("Punti da Decurtare")]
            [Required]
            public int DecurtamentoPunti { get; set; }


            [ForeignKey("IdAnagrafica")]
            [DisplayName("Colpevole")]
            [Required]
            public int IdAnagrafica { get; set; }

            [ForeignKey("IdViolazione")]
            [DisplayName("Tipo di Violazione")]
            [Required]
            public int IdViolazione { get; set; }

            // Costruttori
            public Verbale() { }

            public Verbale(int idVerbale, DateTime dataViolazione, string indirizzoViolazione, string nominativoAgente, DateTime dataTrascrizioneVerbale, decimal importo, int decurtamentoPunti, int idAnagrafica, int idViolazione)
            {
                IdVerbale = idVerbale;
                DataViolazione = dataViolazione;
                IndirizzoViolazione = indirizzoViolazione;
                NominativoAgente = nominativoAgente;
                DataTrascrizioneVerbale = dataTrascrizioneVerbale;
                Importo = importo;
                DecurtamentoPunti = decurtamentoPunti;
                IdAnagrafica = idAnagrafica;
                IdViolazione = idViolazione;
            }

            // Metodo per inserire un nuovo record nella tabella DB con un parametro che è un Verbale
            public static void InserisciNuovoVerbale(Verbale nuovoVerbale)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
                SqlConnection conn = new SqlConnection(connectionString);

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO VERBALE (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, idanagrafica, idviolazione) VALUES (@DataViolazione, @IndirizzoViolazione, @NominativoAgente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @IdAnagrafica, @IdViolazione)", conn);
                    cmd.Parameters.AddWithValue("@DataViolazione", nuovoVerbale.DataViolazione);
                    cmd.Parameters.AddWithValue("@IndirizzoViolazione", nuovoVerbale.IndirizzoViolazione);
                    cmd.Parameters.AddWithValue("@NominativoAgente", nuovoVerbale.NominativoAgente);
                    cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", nuovoVerbale.DataTrascrizioneVerbale);
                    cmd.Parameters.AddWithValue("@Importo", nuovoVerbale.Importo);
                    cmd.Parameters.AddWithValue("@DecurtamentoPunti", nuovoVerbale.DecurtamentoPunti);
                    cmd.Parameters.AddWithValue("@IdAnagrafica", nuovoVerbale.IdAnagrafica);
                    cmd.Parameters.AddWithValue("@IdViolazione", nuovoVerbale.IdViolazione);

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


            //Metodo Relativo alla richiesta di selezione di verbali sopra un certo importo.
            //Il metodo prende un parametro in modo da poter filtrare non solo per 400 ma in funzione dell'inporto desiderato per essere maggiormente dinamico
            //Crea una lista di verbali e la restituisce
            public static List<Verbale> GetVerbaliConImportoSuperiore(int importoMinimo)
            {
                List<Verbale> verbaliList = new List<Verbale>();

                string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = @"SELECT IdVerbale, DataViolazione, IndirizzoViolazione, Nominativo_Agente, 
                                           DataTrascrizioneVerbale, Importo, DecurtamentoPunti, idanagrafica, idviolazione
                                     FROM VERBALE
                                     WHERE Importo >= @ImportoMinimo ORDER BY Importo Desc ";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ImportoMinimo", importoMinimo);

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Verbale verbale = new Verbale
                            {
                                IdVerbale = Convert.ToInt32(reader["IdVerbale"]),
                                DataViolazione = Convert.ToDateTime(reader["DataViolazione"]),
                                IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                NominativoAgente = reader["Nominativo_Agente"].ToString(),
                                DataTrascrizioneVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]),
                                Importo = Convert.ToDecimal(reader["Importo"]),
                                DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]),
                                IdAnagrafica = Convert.ToInt32(reader["idanagrafica"]),
                                IdViolazione = Convert.ToInt32(reader["idviolazione"])
                            };
                            verbaliList.Add(verbale);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                    }
                }

                return verbaliList;
            }
    }
}
    
