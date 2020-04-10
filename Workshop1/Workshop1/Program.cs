using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Workshop1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TaskModel> taskModel = new List<TaskModel>();
            string command = null;
            string dir = $"D:\\CodeLAB\\Workshop1";
            Directory.SetCurrentDirectory(dir);
            Task task = new Task();
            do
            {
                task.load();
                task.list();
                Console.WriteLine("Command:");
                command = Console.ReadLine();
                Console.Clear();
                if (command.ToLower() == "add")
                {                    
                    Console.WriteLine("Input data in correct form [Description];[StartDate];[EndDate];[WholeDay];[Important]");
                    Console.WriteLine("Dormat text it this way:   [Description];[YYYY-MM-DD HH24:MI];[YYYY-MM-DD HH24:MI];[true/false];[true/false]");
                    task.add(Console.ReadLine());                    
                }
                if (command.ToLower() == "list")
                {
                    task.list();                   
                }
                if (command.ToLower() == "save")
                {
                    task.save();
                }
                if (command.ToLower() == "load")
                {
                    task.load();
                }
                if (command.ToLower() == "remove")
                {
                    task.remove();
                }

            } while (command.ToLower() != "exit");


        }

        public class TaskModel
        {
            public int No { get; set; }
            public string Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public bool WholeDay { get; set; }
            public bool Important { get; set; }            
        }

        public class Task
        {
            List<TaskModel> taskModel = new List<TaskModel>();

            //int max_id = 0;
            string temp = null;
            string file_ = "plik.txt";
            private int rows()
            {
                int max_id = 0;
                foreach (var rw in taskModel)
                {
                    max_id++;
                }
                return max_id;
            }
            public int load()
            {
                int max_id = 0;
                if (File.Exists(file_))
                {

                    using (StreamReader file_r = new StreamReader(file_))
                    {
                        //string[] test = file_r.ReadLine().Split(";");
                        int counter = file_r.ReadToEnd().Split(new char[] { '\n' }).Length;
                        taskModel.Clear();
                        for (int i = 0; i < counter - 1; i++)
                        {
                            file_r.BaseStream.Seek(0, SeekOrigin.Begin);
                            string[] data = file_r.ReadLine().Split(";");
                            string no = data[0];
                            string description = data[1];
                            string startDate = data[2];
                            string endDate = data[3];
                            string wholeDay = data[4];
                            string important = data[5];
                            bool n1 = Int32.TryParse(no, out int n);
                            bool s1 = DateTime.TryParse(startDate, out DateTime s);
                            bool e1 = DateTime.TryParse(endDate, out DateTime e);
                            bool w1 = Boolean.TryParse(wholeDay, out bool w2);
                            bool i1 = Boolean.TryParse(important, out bool i2);
                            if (n1 == true && s1 == true && e1 == true && w1 == true && i1 == true)
                            {
                                TaskModel p2 = new TaskModel()
                                {
                                    No = n,
                                    Description = description,
                                    StartDate = s,
                                    EndDate = e,
                                    WholeDay = w2,
                                    Important = i2
                                };
                                taskModel.Add(p2);
                            }
                            max_id ++;
                        }
                        file_r.Close();
                    }

                }
                else { Console.WriteLine("Missing file"); }
                return max_id;
            }
            public void list()
            {
                    Console.Clear();
                    temp = $"{"No.".PadLeft(5)}|{"Description".PadLeft(15)}|{"StartDate".PadLeft(20)}|{"EndDate".PadLeft(20)}|{"WholeDay".PadLeft(10)}|{"Important".PadLeft(10)}|";
                    Console.WriteLine("".PadLeft(temp.Length, '='));
                    Console.WriteLine(temp);
                    Console.WriteLine("".PadLeft(temp.Length, '='));
                foreach (var item in taskModel)
                {
                        if (item.Important)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"{item.No.ToString().PadLeft(5)}|{item.Description.PadLeft(15)}|{item.StartDate.ToString().PadLeft(20)}|{item.EndDate.ToString().PadLeft(20)}|{item.WholeDay.ToString().PadLeft(10)}|{item.Important.ToString().PadLeft(10)}|");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.WriteLine($"{item.No.ToString().PadLeft(5)}|{item.Description.PadLeft(15)}|{item.StartDate.ToString().PadLeft(20)}|{item.EndDate.ToString().PadLeft(20)}|{item.WholeDay.ToString().PadLeft(10)}|{item.Important.ToString().PadLeft(10)}|");
                        }
                        Console.BackgroundColor = ConsoleColor.Black;             
                }
                    Console.WriteLine("".PadLeft(temp.Length, '='));
            }
            public void add(string data)
            {
                string[] v = data.Split(";");
                if (v.Length == 5)
                {
                    string description = v[0];
                    string startDate = v[1];
                    string endDate = v[2];
                    string wholeday = v[3];
                    string important = v[4];
                    if (!string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(startDate) && !string.IsNullOrWhiteSpace(endDate) && !string.IsNullOrWhiteSpace(wholeday) && !string.IsNullOrWhiteSpace(important))
                    {
                        bool s1 = DateTime.TryParse(startDate, out DateTime s);
                        bool e1 = DateTime.TryParse(endDate, out DateTime e);
                        bool w1 = Boolean.TryParse(wholeday, out bool w);
                        bool i1 = Boolean.TryParse(wholeday, out bool i);

                        if (s1 == true && e1 == true && w1 == true && i1 == true)
                        {
                            TaskModel t1 = new TaskModel()
                            {
                                No = rows() + 1,
                                Description = description,
                                StartDate = s,
                                EndDate = e,
                                WholeDay = w,
                                Important = i
                            };
                            taskModel.Add(t1);
                            save();
                        }
                        else
                        {
                            Console.WriteLine("Wrong filling for description.");
                        }
                        if (string.IsNullOrWhiteSpace(description))
                        {
                            Console.WriteLine("description and name can't be null.");
                        }
                    }

                }
            }
            public void save()
            {
                if (!File.Exists(file_))
                {
                    using (var file = File.Create(file_))
                    {

                    }
                }
                File.WriteAllText(file_, string.Empty);
                using (StreamWriter file_w = new StreamWriter(file_, true))
                {
                    foreach (var item in taskModel)
                    {

                        file_w.WriteLine($"{item.No};{item.Description};{item.StartDate.ToString()};{item.EndDate.ToString()};{item.WholeDay.ToString()};{item.Important.ToString()}");

                    }
                    file_w.Close();
                }
            }
            public void remove()
            {
                Console.Clear();
                Console.WriteLine("Which row shuld be deleted");
                string srow = Console.ReadLine();
                bool r1 = Int32.TryParse(srow, out int rw);
                int[] counter = new int[taskModel.Count()] ;
                int c = 0;
                //foreach (var rw1 in taskModel)
                //{
                //    counter[c] = rw1.No;
                //}
                //if (r1)
                //{
                //    if (rw>=counter)
                //    {
                int f = 0;
                    
                        foreach (var item in taskModel)
                        {
                            if (item.No==rw)
                            {
                                f++;
                                taskModel.RemoveAt(f-1);
                                break;
                            }
                    f++;
                        }
                //    }
                //}
                save();
            }
        }
    }
}
