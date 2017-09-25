using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace ServerClient
{
    class Client
    {
        public static object Newtonsoft { get; private set; }

        static void Main(string[] args)
        {
            while (true)
            {
                Person person = new Person();
                Console.WriteLine("Enter First Name");
                string first = Console.ReadLine();
                Console.WriteLine("Enter Middle Name");
                string middle = Console.ReadLine();
                Console.WriteLine("Enter Last Name");
                string last = Console.ReadLine();
                Console.WriteLine("Enter Sex");
                string sex = Console.ReadLine();
                person.sex = sex;
                Name name = new Name(first, middle, last);
                person.Name = name;
                Console.WriteLine("Enter Date Of Birth(yyyy/mm/dd) ");
                DateTime date = DateTime.Parse(Console.ReadLine()).Date;
                person.DateOfBirth = date;
                //var result = JsonConvert.SerializeObject(person);
                var result2 = Serializer.Serialize(person);
                string serverIpAddress = "172.16.14.121";
                int serverPortNumber = 8080;
                TcpClient call = new TcpClient(serverIpAddress, serverPortNumber);
                NetworkStream serverStream = call.GetStream();
                byte[] stringToByte = Encoding.ASCII.GetBytes(result2);
                serverStream.Write(stringToByte, 0, stringToByte.Length);

            }
        }
    }
    public class Person
    {
        public Name Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string sex;  
    }
    public class Name
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Name(string firstName, string middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }
        public override string ToString()
        {
            return (FirstName + " " + MiddleName + " " + LastName);
        }
    }
    public class Serializer
    {
        public static string Serialize(Person person)
        {
            return $"{person.Name.ToString()}<>{person.DateOfBirth.ToString()}<>{person.sex}";
        }
    }
}
