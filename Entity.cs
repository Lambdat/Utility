using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility
{

    //Per Eseguire il toString ci serviremo della Reflection, ossia la capacità di un oggetto di ragionare
    // sulla sua stessa struttura (un oggetto che riflette su se stesso)





    // i metodi astratti non possono istanziare oggetti propri; fungono solo per il tipo formale (il contenitore a sinistra)            
    public abstract class Entity //Padre Comune
    {
        // Creiamo direttamente la Proprietà
        public int Id { get; set; }        //TUTTE LE CLASSI FIGLIE HANNO SEMPRE LA PK ID 

        public Entity() { }

        public Entity(int Id)
        {
            this.Id = Id;
        }


        public override string ToString()
        {
            string ris = "";

            //PropertyInfo è un oggetto che conterrà tutte le informazioni di una
            //determinata proprietà


            //this.GetType().GetProperties():

            //this -> indica la classe Entity

            //GetType() -> è un metodo che si trova nella classe Object che ci restituisce
            //             un oggetto di tipo Type

            //GetProperties() -> è un metodo che si trova nella classe Type e ci restituisce
            //                   un vettore di oggetti PropertyInfo

            foreach (PropertyInfo proprieta in this.GetType().GetProperties()) //Cicliamo una ad una tutte le proprietà dell'oggetto
            {

                ris += proprieta.Name + " : " + proprieta.GetValue(this) + "\n";

            }

            return ris;
        }
        // proprieta.Name prende il nome della proprietà (al primo giro sarà "Id")
        // proprieta.GetValue(this) : vai a prendere il valore della proprietà che
        //       stai ciclando ("Id") dall'oggetto this (l'oggetto sul quale è
        //       stato chiamato il metodo ToString



        //Questo metodo riceve come parametro di ingresso un dizionario,
        //nel nostro immaginario riceverà la riga della tabella del database
        // e valorizzerà le proprietà dell'oggetto
        //pescandole dal Dictionary

        public void FromDictionary(Dictionary<string, string> riga)
        {
            //Guardare una ad una tutte le proprietà dell'oggetto
            //Prenderò la singola proprietà e cercherò la chiave con quel nome
            //Dovrò valorizzare la proprietà con il valore contenuto in riga

            foreach (PropertyInfo proprieta in this.GetType().GetProperties())
            {
                if (riga.ContainsKey(proprieta.Name.ToLower()))
                {
                    object valore = riga[proprieta.Name.ToLower()];

                    switch (proprieta.PropertyType.Name.ToLower())
                    {
                        case "datetime":
                            valore = DateTime.Parse(riga[proprieta.Name.ToLower()]);
                            break;
                        case "int32":
                            valore = int.Parse(riga[proprieta.Name.ToLower()]);
                            break;
                        case "double":
                            valore = double.Parse(riga[proprieta.Name.ToLower()]);
                            break;
                        case "bool":
                            valore = bool.Parse(riga[proprieta.Name.ToLower()]);
                            break;
                        case "boolean":
                            valore = bool.Parse(riga[proprieta.Name.ToLower()]);
                            break;
                    }

                    proprieta.SetValue(this, valore);
                }
            }

        }




    }








}

