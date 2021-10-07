using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Utility
{

    //Questa classe si occuperà semplicemente di connettere qualcuno
    //al Database e a permettergli di eseguire query

    public class Database
    {

        private MySqlConnection con; //La connessione 

        //Per costruire una connessione ci servnono: SERVER (ip), DATABASE (nomedb), UID e PASSWORD
        //I parametri opzionali entrano in azione solo se chi crea l'oggetto non li fornisce
        public Database(string nomedb, string server = "localhost", string user = "root", string pass = "root")
        {

            string connectionString = $"SERVER={server};DATABASE={nomedb};" +
                                     $"UID={user};PASSWORD={pass};";

            con = new MySqlConnection(connectionString);
        }



        //Cos'è una Lista?
        //Un contenitore di oggetti dello stesso tipo ordinati con dimensione variabile

        //Cos'è un Dictionary?
        //Un Dictionary è una Lista ma al posto degli indici che partono da 0 ci metto
        //io degli indici che voglio (sia per tipo che per valore).
        //DOMANDA CHE METTO NEL TEST SE NON MI SCRIVETE QUESTA DEFINIZIONE BENE
        //VI MANDO LA GUARDA DI FINANZA A CASA MALEDETTI
        //Un Dictionary è un insieme di coppie chiave-valore in cui il tipo,
        //sia delle chiavi che dei valori, lo scegliamo noi e le cui chiavi non si 
        //ripetono mai.

        // colonna , riga
        public List<Dictionary<string, string>> Read(string query)
        {
            List<Dictionary<string, string>> ris = new List<Dictionary<string, string>>();

            //Aprire la connessione grazie all'oggetto MySqlConnection con
            con.Open();

            //Creerà poi il Command per l'esecuzione della query arrivata come parametro
            MySqlCommand cmd = new MySqlCommand(query, con);


            //Creerà il DataReader che conterrà il risultato del Command eseguito
            MySqlDataReader dr = cmd.ExecuteReader();

            //Cicleremo il nostro DR e ciascuna riga la trasformeremo in un Dictionary
            while (dr.Read())  // UNA RIGA PER VOLTA (LETTURA VERTICALE)
            {
                //Come chiavi ci inseriremo i nomi delle colonne
                //come valori ciò che è scritto nella tabella
                Dictionary<string, string> riga = new Dictionary<string, string>();
                // Dictionary<id,1>


                //Inseriremo il Dictionary in un Lista di Dictionary
                for (int i = 0; i < dr.FieldCount; i++) //Cicliamo le colonne   (LETTURA ORIZZONTALE)
                    riga.Add(dr.GetName(i).ToLower(), dr.GetValue(i).ToString());

                //                        id               1




                //Quando il ciclo sarà terminato restituiremo la Lista
                ris.Add(riga);

                //Chiudiamo Connessione

            }

            dr.Close();
            con.Close();

            return ris;

        }



        // esempio la media, un contatore , where id= 
        public Dictionary<string, string> ReadOne(string query)
        {
            try
            {
                return Read(query)[0]; //Prima riga della lista di dizioniari restituita dal metodo
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //se finisco nel catch significa che la query non ha prodotto risultati
                return null;
            }



        }

        // Questo metodo esegue un operazione di INSERT, UPDATE o DELETE
        //che viene passata come parametro
        public bool Update(string query)
        {

            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(query, con);

                cmd.ExecuteNonQuery();


                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n" +
                                    "La seguente query ha prodotto un errore: \n" +
                                    $"{query} + \n" +
                                  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n");

                return false;
            }
            finally // In ogni caso (che tu sia stato nel try o nel catch) chiudi la connessione
            {
                con.Close();
            }

        }

    }
}
