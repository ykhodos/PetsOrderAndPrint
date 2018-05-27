/****************************************************************************************************************
 *  This class contains the properties of pets owner.                                                           *
 *  The properties are :                                                                                        *
 *      string name - owner's name                                                                              *
 *      string gender - owner's gender                                                                          *
 *      int age - owner's age                                                                                   *
 *      List<Pet> pets - the list of owner's pets                                                               *
 *                                                                                                              *
 *  Author: Yuri Khodos                                                                                         *
****************************************************************************************************************/
using System.Collections.Generic;

namespace Cats
{
    public class PetsOwner
    {
        public string name { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public List<Pet> pets { get; set; }
    }
}
