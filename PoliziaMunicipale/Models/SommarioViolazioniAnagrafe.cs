using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PoliziaMunicipale.Models
{
    public class SommarioViolazioniAnagrafe
    {
        [ScaffoldColumn(false)]
        public int IdAnagrafica { get; set; }

        
        public string Nome { get; set; }

        
        public string Cognome { get; set;}

        
        public string Cod_Fisc { get; set; }

        public int NumVerbali { get; set; }

        public int NumPunti { get; set; }

        public int ImportoViolazione { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataViolazione { get; set; }

        public SommarioViolazioniAnagrafe() { }


        public SommarioViolazioniAnagrafe( int importoViolazione ,string nome, string cognome, DateTime dataViolazione , int numPunti)
        {
            
            ImportoViolazione = importoViolazione;
            Nome = nome;
            Cognome = cognome;
            DataViolazione = dataViolazione;
            NumPunti = numPunti;
            
        }


        public SommarioViolazioniAnagrafe(int idAnagrafica, string nome, string cognome, string cod_Fisc, int numVerbali)
        {
            IdAnagrafica = idAnagrafica;
            Nome = nome;
            Cognome = cognome;
            Cod_Fisc = cod_Fisc;
            NumVerbali = numVerbali;
        }

        public SommarioViolazioniAnagrafe(int idAnagrafica, string nome, string cognome, int numPunti, string cod_Fisc)
        {
            IdAnagrafica = idAnagrafica;
            Nome = nome;
            Cognome = cognome;
            Cod_Fisc = cod_Fisc;
            NumPunti = numPunti;
        }


        //Metodo per la richiesta relativa all'ottenimeto del numero di verbali raggruppati per anagrafe
        //Seleziona i campi dal DB e joina i risultati attraverso le chiavi IdAnagrafica
        //Crea una lista dei record e la restituisce
        public static List<SommarioViolazioniAnagrafe> GetSommarioViolazioniAnagrafe(int order)
        {
            List<SommarioViolazioniAnagrafe> sommarioList = new List<SommarioViolazioniAnagrafe>();

            string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string orderDirection = (order == 0) ? "ASC" : "DESC";
                    string query = $"SELECT A.IdAnagrafica, A.Nome, A.Cognome, A.Cod_Fisc, COUNT(*) AS NumVerbali " +
                                   $"FROM ANAGRAFICA A " +
                                   $"JOIN VERBALE V ON A.IdAnagrafica = V.IdAnagrafica " +
                                   $"GROUP BY A.IdAnagrafica, A.Nome, A.Cognome, A.Cod_Fisc " +
                                   $"ORDER BY NumVerbali {orderDirection}";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        SommarioViolazioniAnagrafe sommario = new SommarioViolazioniAnagrafe
                        {
                            IdAnagrafica = Convert.ToInt32(reader["IdAnagrafica"]),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString(),
                            Cod_Fisc = reader["Cod_Fisc"].ToString(),
                            NumVerbali = Convert.ToInt32(reader["NumVerbali"])
                        };
                        sommarioList.Add(sommario);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                }
            }

            return sommarioList;
        }

        //Metodo per la richiesta relativa all'ottenimeto del numero punti sottratti raggruppati per anagrafe
        //Seleziona i campi dal DB e joina i risultati attraverso le chiavi IdAnagrafica
        //Crea una lista dei record e la restituisce
        public static List<SommarioViolazioniAnagrafe> GetSommarioPuntiAnagrafe(int order)
        {
            List<SommarioViolazioniAnagrafe> sommarioList = new List<SommarioViolazioniAnagrafe>();

            string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sortOrder = (order == 0) ? "ASC" : "DESC";
                    string query = $@"SELECT A.IdAnagrafica, A.Nome, A.Cognome, A.Cod_Fisc, SUM(V.DecurtamentoPunti) AS NumPunti
                             FROM ANAGRAFICA A
                             LEFT JOIN VERBALE V ON A.IdAnagrafica = V.IdAnagrafica
                             GROUP BY A.IdAnagrafica, A.Nome, A.Cognome, A.Cod_Fisc
                             ORDER BY NumPunti {sortOrder}";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        SommarioViolazioniAnagrafe sommario = new SommarioViolazioniAnagrafe
                        {
                            IdAnagrafica = Convert.ToInt32(reader["IdAnagrafica"]),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString(),
                            Cod_Fisc = reader["Cod_Fisc"].ToString(),


                            
                        };

                        if (reader["NumPunti"] != DBNull.Value)
                        {
                            sommario.NumPunti = Convert.ToInt32(reader["NumPunti"]);
                        }
                        else
                        {
                            sommario.NumPunti = 0;
                        }

                        sommarioList.Add(sommario);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                }
            }

            return sommarioList;
        }

        //Metodo che restituisce i valori relativi alla richiesta di tutti i verbali che superano un certo numero di punti decurtati
        //Il metodo prevede due parametri per il filtraggio dei record in modo da essere magigormente dinamico
        //Crea una lista dei record e la restituisce
        public static List<SommarioViolazioniAnagrafe> GetSommarioAnagrafe(int type, int punti)
        {
            List<SommarioViolazioniAnagrafe> sommarioList = new List<SommarioViolazioniAnagrafe>();

            string connectionString = ConfigurationManager.ConnectionStrings["connectionStringDb"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string orderBy = "A.Cognome";
                    switch (type) {

                        case 1:
                            orderBy = "A.Cognome";
                            break;

                        case 2:
                            orderBy = "V.Importo";
                            break;

                        case 3:
                            orderBy = "V.DataViolazione";
                            break;

                        default:
                            break ;
                    
                    }



                    string query = $@"SELECT    V.Importo, 
                                                A.Cognome, 
                                                A.Nome, 
                                                V.DataViolazione, 
                                                V.DecurtamentoPunti 
                                                From VERBALE V LEFT JOIN ANAGRAFICA A ON 
                                                V.idanagrafica = A.idanagrafica WHERE 
                                                V.DecurtamentoPunti >= {punti} order by {orderBy} Desc";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        SommarioViolazioniAnagrafe sommario = new SommarioViolazioniAnagrafe
                        {
                            ImportoViolazione = Convert.ToInt32(reader["Importo"]),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString(),
                            DataViolazione = Convert.ToDateTime(reader["DataViolazione"].ToString()),
                            NumPunti = Convert.ToInt32(reader["DecurtamentoPunti"]),

                        };

                        sommarioList.Add(sommario);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Si è verificato un errore: {ex.Message}");
                }
            }

            return sommarioList;
        }

    }

}
