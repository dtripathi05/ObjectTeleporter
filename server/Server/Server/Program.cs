using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            IPAddress localAddress = IPAddress.Any;
            TcpListener server = new TcpListener(localAddress, 8080);
            server.Start();
            while (true)
            {
                Socket clientSocket = server.AcceptSocket();
                byte[] incomingStream = new byte[2048];
                clientSocket.Receive(incomingStream);
                string message = Encoding.ASCII.GetString(incomingStream);
                var result = /*JsonConvert.DeserializeObject<Person>(message);*/
                    Serializer.Deserialize(message);
                    Console.WriteLine(result.ToString());
                clientSocket.Close();
                clientSocket.Dispose();
            }

        }
    }
    public class Person
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string sex;
        public Person(string name,string dateOfBirth,string sex)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            this.sex = sex;
        }
        public override string ToString()
        {
            return "Name:" + Name.ToString() + "\nDate of Birth: " + DateOfBirth + "\nSex: " + sex.ToString();
        }
    }
    //public class Name
    //{
    //    public string FirstName { get; set; }
    //    public string MiddleName { get; set; }
    //    public string LastName { get; set; }
    //    public Name(string firstName, string middleName, string lastName)
    //    {
    //        FirstName = firstName;
    //        MiddleName = middleName;
    //        LastName = lastName;
    //    }
        //public override string ToString()
        //{
        //    return (FirstName + " " + MiddleName + " " + LastName);
        //}
    //}
    public class Serializer
    {
        public static string Serialize(Person person)
        {
            return $"{person.Name}-{person.DateOfBirth.ToString()}-{person.sex}";
        }
        public static Person Deserialize(string data)
        {
           var input= data.TrimEnd('\0');
            string[] member = input.Split("<>".ToCharArray());
            return (new Person(member[0], member[2], member[4]));
        }
    }
}
