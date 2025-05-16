using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // Константы для символов ключей и дверей
    static readonly char[] keys_char = Enumerable.Range('a', 26).Select(i => (char)i).ToArray();
    static readonly char[] doors_char = keys_char.Select(char.ToUpper).ToArray();

    // Метод для чтения входных данных
    static List<List<char>> GetInput()
    {
        var data = new List<List<char>>();
        string line;
        while ((line = Console.ReadLine()) != null && line != "")
        {
            data.Add(line.ToCharArray().ToList());
        }
        return data;
    }

    static int Solve(List<List<char>> data)
    {
        (int x, int y)[] rCoords = new (int x, int y)[4];
        (int x, int y)[] posSteps = new (int x, int y)[4];
        int rCount = 0;
        int keyCount = 0;
        int doorCount = 0;
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[i].Count; j++)
            {
                if (data[i][j] == '@')
                {
                    rCoords[rCount] = (i, j);
                    rCount++;
                }
                else if (keys_char.Contains(data[i][j]))
                    keyCount++;
                else if (doors_char.Contains(data[i][j]))
                    doorCount++;
            }
        }
        HashSet<((int x, int y) r1, (int x, int y) r2, (int x, int y) r3, (int x, int y) r4, HashSet<char> colKeys)> states = new HashSet<((int x, int y) r1, (int x, int y) r2, (int x, int y) r3, (int x, int y) r4, HashSet<char> colKeys)>();
        LinkedList<((int x, int y) r1, (int x, int y) r2, (int x, int y) r3, (int x, int y) r4, int stepcount, HashSet<char> colKeys)> q = new LinkedList<((int x, int y) r1, (int x, int y) r2, (int x, int y) r3, (int x, int y) r4, int stepcount, HashSet<char> colKeys)>();
        List<HashSet<char>> k = new List<HashSet<char>>();
        var startedK = new HashSet<char>();
        q.AddLast((rCoords[0], rCoords[1], rCoords[2], rCoords[3], 0, startedK));
        states.Add((rCoords[0], rCoords[1], rCoords[2], rCoords[3],  startedK));
        k.Add(startedK);
        while (q.Count!=0)
        {
            var state = q.First.Value;
            q.RemoveFirst();
            if (state.colKeys.Count == keyCount)
                return state.stepcount;
            rCoords[0] = state.r1;
            rCoords[1] = state.r2;
            rCoords[2] = state.r3;
            rCoords[3] = state.r4;
            for (int i = 0; i < 4; i++)
            {
                var pos = rCoords[i];
                posSteps[0] = (pos.x + 1, pos.y);
                posSteps[1] = (pos.x, pos.y + 1);
                posSteps[2] = (pos.x - 1, pos.y);
                posSteps[3] = (pos.x, pos.y - 1);
                for (int j = 0; j < 4; j++)
                {
                    var point = posSteps[j];
                    if (data[point.x][point.y] == '.'|| data[point.x][point.y]== '@'|| (doors_char.Contains(data[point.x][point.y]) && state.colKeys.Contains(char.ToLower(data[point.x][point.y]))))
                    {
                        if (i == 0)
                        {
                            if (!states.Contains((point, state.r2, state.r3, state.r4,  state.colKeys)))
                            {
                                q.AddLast((point, state.r2, state.r3, state.r4, state.stepcount + 1, state.colKeys));
                                states.Add((point, state.r2, state.r3, state.r4, state.colKeys));
                            }
                        }
                        else if (i == 1)
                        {
                            if (!states.Contains((state.r1, point, state.r3, state.r4,  state.colKeys)))
                            {
                                q.AddLast((state.r1, point, state.r3, state.r4, state.stepcount + 1, state.colKeys));
                                states.Add((state.r1, point, state.r3, state.r4,  state.colKeys));
                            }
                        }
                        else if (i == 2)
                        {
                            if (!states.Contains((state.r1, state.r2, point, state.r4,  state.colKeys)))
                            {
                                q.AddLast((state.r1, state.r2, point, state.r4, state.stepcount + 1, state.colKeys));
                                states.Add((state.r1, state.r2, point, state.r4,  state.colKeys));
                            }
                        }
                        else
                        {
                            if (!states.Contains((state.r1, state.r2, state.r3, point, state.colKeys)))
                            {
                                q.AddLast((state.r1, state.r2, state.r3, point, state.stepcount + 1, state.colKeys));
                                states.Add((state.r1, state.r2, state.r3, point, state.colKeys));
                            }
                        }
                    }
                    else if (keys_char.Contains(data[point.x][point.y]))
                    {
                        var newK = state.colKeys;
                        if (!newK.Contains(data[point.x][point.y]))
                        {
                            var same = false;
                            foreach (var keys in k)
                            {
                                if (keys.Count  == newK.Count +1 )
                                {
                                    var p = true;
                                    foreach (var key in keys)
                                    {
                                        if (!newK.Contains(key) && key != data[point.x][point.y])
                                        {
                                            p = false;
                                            break;
                                        }
                                    }
                                    if (p)
                                    {
                                        same = true;
                                        newK = keys;
                                        break;
                                    }
                                }
                            }
                            if (!same)
                            {
                                newK = new HashSet<char>(state.colKeys);
                                newK.Add(data[point.x][point.y]);
                                k.Add(newK);
                            }
                        }
                        if (i == 0)
                        {
                            if (!states.Contains((point, state.r2, state.r3, state.r4, newK)))
                            {
                                q.AddLast((point, state.r2, state.r3, state.r4, state.stepcount + 1, newK));
                                states.Add((point, state.r2, state.r3, state.r4, newK));
                            }
                        }
                        else if (i == 1)
                        {
                            if (!states.Contains((state.r1, point, state.r3, state.r4, newK)))
                            {
                                q.AddLast((state.r1, point, state.r3, state.r4, state.stepcount + 1, newK));
                                states.Add((state.r1, point, state.r3, state.r4, newK));
                            }
                        }
                        else if (i == 2)
                        {
                            if (!states.Contains((state.r1, state.r2, point, state.r4, newK)))
                            {
                                q.AddLast((state.r1, state.r2, point, state.r4, state.stepcount + 1, newK));
                                states.Add((state.r1, state.r2, point, state.r4, newK));
                            }
                        }
                        else
                        {
                            if (!states.Contains((state.r1, state.r2, state.r3, point, newK)))
                            {
                                q.AddLast((state.r1, state.r2, state.r3, point, state.stepcount + 1, newK));
                                states.Add((state.r1, state.r2, state.r3, point, newK));
                            }
                        }
                    }
                }
            }
        }
        return -1;
    }

    static void Main()
    {
        var data = GetInput();
        int result = Solve(data);

        if (result == -1)
        {
            Console.WriteLine("No solution found");
        }
        else
        {
            Console.WriteLine(result);
        }
    }
}