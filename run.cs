using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class DateComparer : IComparer<String>
{
    public int Compare(string x, string y)
    {
        string[] xDate = x.Split(new[] { '-' });
        int xYear = int.Parse(xDate[0]);
        int xMonth = int.Parse(xDate[1]);
        int xDay = int.Parse(xDate[2]);
        string[] yDate = y.Split(new[] {'-'});
        int yYear = int.Parse(yDate[0]);
        int yMonth = int.Parse(yDate[1]);
        int yDay = int.Parse(yDate[2]);
        if (xYear != yYear)
            return xYear - yYear;
        else if (xMonth != yMonth)
            return xMonth - yMonth;
        return xDay-yDay;
    }
}

class HotelCapacity
{
    static bool CheckCapacity(int maxCapacity, List<Guest> guests)
    {
        if (guests.Count <= maxCapacity)
            return true;
        else
        {
            string[] checkIns = new string[guests.Count];
            string[] checkOuts = new string[guests.Count];
            int i = 0;
            foreach(var guest in guests)
            {
                checkIns[i] = guest.CheckIn;
                checkOuts[i] = guest.CheckOut;
                i++;
            }
            DateComparer dateC = new DateComparer();
            Array.Sort(checkIns, dateC);
            Array.Sort(checkOuts, dateC);
            int inI = 0;
            int outI = 0;
            var count = 0;
            while(inI < checkIns.Length)
            {
                if (dateC.Compare(checkIns[inI], checkOuts[outI]) < 0)
                {
                    count++;
                    inI++;
                    if (count > maxCapacity)
                        return false;
                }
                else
                {
                    count--;
                    outI++;
                }
            }
            return true;
        }
    }


    class Guest
    {
        public string Name { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
    }


    static void Main()
    {
        int maxCapacity = int.Parse(Console.ReadLine());
        int n = int.Parse(Console.ReadLine());


        List<Guest> guests = new List<Guest>();


        for (int i = 0; i < n; i++)
        {
            string line = Console.ReadLine();
            Guest guest = ParseGuest(line);
            guests.Add(guest);
        }


        bool result = CheckCapacity(maxCapacity, guests);


        Console.WriteLine(result ? "True" : "False");
    }


    // Простой парсер JSON-строки для объекта Guest
    static Guest ParseGuest(string json)
    {
        var guest = new Guest();


        // Извлекаем имя
        Match nameMatch = Regex.Match(json, "\"name\"\\s*:\\s*\"([^\"]+)\"");
        if (nameMatch.Success)
            guest.Name = nameMatch.Groups[1].Value;


        // Извлекаем дату заезда
        Match checkInMatch = Regex.Match(json, "\"check-in\"\\s*:\\s*\"([^\"]+)\"");
        if (checkInMatch.Success)
            guest.CheckIn = checkInMatch.Groups[1].Value;


        // Извлекаем дату выезда
        Match checkOutMatch = Regex.Match(json, "\"check-out\"\\s*:\\s*\"([^\"]+)\"");
        if (checkOutMatch.Success)
            guest.CheckOut = checkOutMatch.Groups[1].Value;


        return guest;
    }
}



