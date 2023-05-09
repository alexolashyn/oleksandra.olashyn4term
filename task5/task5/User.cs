﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace task5
{
    internal class User
    {
        private string firstname;
        private string lastname;
        private string email;
        private string password;
        private string role;
        public string Firstname
        { get { return firstname; } set { firstname = Regex.IsMatch(value, @"^[A-Z][a-z]+(-[A-Z][a-z]+)?$") ? value : "-1"; } }
        public string Lastname { get { return lastname; } set { lastname = Regex.IsMatch(value, @"^[A-Z][a-z]+(-[A-Z][a-z]+)?$") ? value : "-1"; } }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = Regex.IsMatch(value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") ? value : "-1";
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = Regex.IsMatch(value, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$") ? value : "-1";
            }
        }
        public string Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value == "admin" || value == "customer" || value == "manager" ? value : "-1";
            }
        }
        public User() { }

        public override string ToString()
        {
            return $"{firstname}, {lastname}, {email}, {password}, {role}";
        }
    }
    internal class Registration
    {
        private Validation validator;
        private bool isActive;
        private User user;
        public Registration(User new_user)
        {
            validator = new Validation();
            isActive = false;
            user = new_user;
        }
        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }
        public User ThisUser
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }

        public void SignUp()
        {
            validator.validate(user);
            if (!validator.Is_valid)
            {
                validator.show_errors();
            }
            else
            {
                List<User> lst = new List<User>();
                string filename = @"C:\np_4term\task5\task5\RegistratedUsers.json";
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    var users = JsonSerializer.Deserialize<List<User>>(json);
                    users.Add(user);
                    lst = users;
                }
                string output = JsonSerializer.Serialize(lst);
                File.WriteAllText(filename, output);
                isActive = true;
            }
            validator = new Validation();
        }
        public void Login(string role)
        {
            string filename = @"C:\np_4term\task5\task5\RegistratedUsers.json";
            if (role == "admin" || role == "manager")
            {
                filename = @"C:\4term\task5\task5\admin.json";
            }
            Console.Write("Enter your email: ");
            string email = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();
            using (StreamReader r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();
                var users = JsonSerializer.Deserialize<List<User>>(json);
                foreach (var u in users)
                {
                    if (u.Email == email && u.Password == password)
                    {
                        var properties = user.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            property.SetValue(user, u.GetType().GetProperty(property.Name).GetValue(u));
                        }
                        isActive = true;
                        break;
                    }
                }

            }
        }
        public void Logout()
        {
            isActive = false;
        }

    }
}
