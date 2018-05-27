/****************************************************************************************************************
 *  This program is AGL developer test.                                                                         *
 *  It consumes the json from http://agl-developer-test.azurewebsites.net/people.json                           *
 *  and outputs a list of all the cats in alphabetical order under a heading of the gender of their owner.      *
 *                                                                                                              *
 *  Author: Yuri Khodos                                                                                         *
****************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RestSharp;

namespace Cats
{
    class MainClass
    {
        public static void Main()
        {
            JArray resultArray = GetJsonArray(@"http://agl-developer-test.azurewebsites.net/people.json");

            if(resultArray == null || !IsJsonValid(resultArray))
            {
                Console.WriteLine("The Json is not valid!");
                Environment.Exit(1);
            };

            var cpets = CatsOrderedByNameAndOwnersGender(resultArray);


            PrintPetsNames(cpets);

        }

        //This method receives the url, from which Json is to be consumed
        //and converts it into JArray object
        //Params: string URL
        //Returns: JArray on success, null in case of an error.
        public static JArray GetJsonArray(string url)
        {
            JArray resultArray = null;
            RestClient client = new RestClient(url);
            IRestResponse response = client.Execute(new RestRequest());

            if(response.ResponseStatus == ResponseStatus.Error)
            {
                Console.WriteLine("GetJsonArray method " + response.ErrorMessage);
            }

            try
            {
                resultArray = JArray.Parse(response.Content);
            }
            catch(Exception)
            {
                Console.WriteLine("GetJsonArray method Error while parsing the response.");
            }
            return resultArray;

        }

        //This method receives JArray object and validates it against JSchema
        //Params: JArray json array to be validated
        //Returns: True if the JArray is valid, or false otherwise
        public static bool IsJsonValid(JArray jsonTovalidate)
        {
            string jsonSchema = @"{
                'type':'array',
                'items':{
                        'type':'object',
                        'properties':{
                                'name': {'type':'string'},
                                'gender':{'type':'string'},
                                'age':{'type':'integer'},
                                'pets':{
                                        'type':['array','null'],
                                        'items':{
                                                'type':'object',
                                                 'properties':{
                                                        'name':{'type':'string'},
                                                        'type':{'type':'string'}
                                                  }
                                        }
                                }
                         }
                 }   
            }";
            JSchema schema = JSchema.Parse(jsonSchema);

            return jsonTovalidate.IsValid(schema);
        }

        //This method receives JArray object and creates a List of PetsOwner objects grouped by their gender.
        //Each PetsOwner object contains a list of cats, sorted by cat's names.
        //Params: JArray json array
        //Returns: List of PetsOwner objects
        public static List<PetsOwner> CatsOrderedByNameAndOwnersGender(JArray jsonArray)
        {
            return jsonArray.GroupBy(pos => pos["gender"]).
                                 Select(g => new PetsOwner
                                 {
                                     gender = g.Key.Value<string>(),
                                     pets = g.SelectMany(p => p["pets"]).
                                               Where(pt => pt["type"].Value<string>() == "Cat").
                                               Select(pn => new Pet { name = pn["name"].Value<string>() }).
                                               OrderBy(n => n.name).ToList<Pet>()
                                 }).ToList<PetsOwner>();
        }

        //This method receives a List of PetsOwner objects and prints the names of their pets in Ascending order,
        //under a heading of the gender of their owner.
        public static void PrintPetsNames(List<PetsOwner> lst)
        {
            lst.ForEach(g =>
            {
                Console.WriteLine(g.gender + ":");
                g.pets.ForEach(p => Console.WriteLine("\t" + p.name));
            });
        }

    }
}
