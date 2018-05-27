/****************************************************************************************************************
 *  This class contains Unit Tests for AGL developer test program                                               *
 *                                                                                                              *
 *  Author: Yuri Khodos                                                                                         *
****************************************************************************************************************/

using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Cats
{
    [TestFixture]
    public class CatsUnitTests
    {

        //This test verifies that GetJsonArray method consumes the Json from http://agl-developer-test.azurewebsites.net/people.json
        //and converts it into JArray.
        [Test]
        public void TestGetJsonArray()
        {
            string validUrl = @"http://agl-developer-test.azurewebsites.net/people.json";

            JArray resArray = MainClass.GetJsonArray(validUrl);

            Assert.IsInstanceOf<JArray>(resArray, "GetJsonArray method could not get JArray from the input URL");
        }

        //This test verifies that a Json constructed according to schema validates.
        //The schema is defined in MainClass.IsJsonValid method
        [Test]
        public void TestIsJsonValid()
        {
            string validJsonStr = @"[
                                {
                                    'name': 'Bob',
                                    'gender': 'Male',
                                    'age': 23,
                                    'pets': [
                                        {
                                            'name': 'Garfield',
                                            'type': 'Cat'
                                        },
                                        {
                                            'name': 'Fido',
                                            'type': 'Dog'
                                        }   
                                    ]
                                }]";

            JArray resultArray = JArray.Parse(validJsonStr);

            Assert.IsTrue(MainClass.IsJsonValid(resultArray), "IsJsonValid method should return True.");
        }

        //This test verifies that given the valid Json, MainClass.CatsOrderedByNameAndOwnersGender returns 
        //correct list of pets (sorted by names and grouped by owners gender)
        [Test]
        public void TestCatsOrderedByNameAndOwnersGender()
        {
            string validJsonStr = @"[
                                {
                                    'name': 'Bob',
                                    'gender': 'Male',
                                    'age': 23,
                                    'pets': [
                                        {
                                            'name': 'Garfield',
                                            'type': 'Cat'
                                        },
                                        {
                                            'name': 'Fido',
                                            'type': 'Dog'
                                        }   
                                    ]
                                },
                                {
                                    'name': 'Jennifer',
                                    'gender': 'Female',
                                    'age': 18,
                                    'pets': [
                                        {
                                            'name': 'Garfield',
                                            'type': 'Cat'
                                        }
                                    ]
                                 },
                                {
                                    'name': 'Fred',
                                    'gender': 'Male',
                                    'age': 40,
                                    'pets': [
                                        {
                                            'name': 'Tom',
                                            'type': 'Cat'
                                        },
                                        {
                                            'name': 'Max',
                                            'type': 'Cat'
                                        },
                                        {
                                            'name': 'Sam',
                                            'type': 'Dog'
                                        },
                                        {
                                            'name': 'Jim',
                                            'type': 'Cat'
                                        }
                                    ]
                                    }]";

            const int numOfOwners = 2;
            const int numOfPetsMale = 4;
            const int numOfPetsFemale = 1;

            List<string> petsMale = new List<string>{"Garfield", "Jim", "Max", "Tom"};
            List<string> petsFemale = new List<string> {"Garfield"};
      

            JArray resultArray = JArray.Parse(validJsonStr);

            List<PetsOwner> actualLst = MainClass.CatsOrderedByNameAndOwnersGender(resultArray);

            Assert.IsTrue(actualLst.Count == numOfOwners, "Actual list should contain 2 owners: Male and Female");
            Assert.IsTrue(actualLst[0].pets.Count == numOfPetsMale, "Male should have 4 pets ");
            Assert.IsTrue(actualLst[1].pets.Count == numOfPetsFemale, "Female should have 1 pet");

            for (int i = 0; i < numOfPetsMale; i++)
            {
                Assert.IsTrue(actualLst[0].pets[i].name == petsMale[i], "Names and order of the pets of Male should be 'Garfield,'Jim','Max','Tom'");
            }
           
            for (int i = 0; i < numOfPetsFemale; i++)
            {
                Assert.IsTrue(actualLst[1].pets[i].name == petsFemale[i], "Names and order of the pets of Female should be 'Garfield");
            }
        }
    }
}
