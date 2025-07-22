using System;
using System.Collections.Generic;
using System.IO;

// @todo validate
// @todo add test

public class LnzData
{
    public List<Lnz.Line> Linez = new List<Lnz.Line>();
    
    // ballz
    public List<int> Omissions = new List<int>();
    public List<Lnz.BallzInfo> BallzInfoz = new List<Lnz.BallzInfo>();
    public List<Lnz.AddBall> AddBallz = new List<Lnz.AddBall>();
    public List<Lnz.PaintBall> PaintBallz = new List<Lnz.PaintBall>();
    public List<Lnz.ProjectBall> ProjectBallz = new List<Lnz.ProjectBall>();
    public List<Lnz.FurPatternBall> FurPatternBalls = new List<Lnz.FurPatternBall>();
    
    public List<Lnz.Eye> Eyes = new List<Lnz.Eye>();
    
    public List<Lnz.Whisker> Whiskers = new List<Lnz.Whisker>();
    
    public List<Lnz.EarExtension> EarExtensions = new List<Lnz.EarExtension>();
    
    public List<Lnz.Move> Movez = new List<Lnz.Move>();
    
    public List<Lnz.BodyArea> BodyAreaz = new List<Lnz.BodyArea>();

    public bool IsBallOmmited(int ballID)
    {
        foreach (var omission in Omissions)
        {
            if (omission == ballID)
            {
                return true;
            }
        }
        
        return false;
    }
    
    public void ParseFile(string fileName)
    {
        var lines = File.ReadAllLines(fileName);

        int lineIndex = 0;
        while (lineIndex < lines.Length) // for (int i = 0; i < lines.Length;)
        {
            // Console.WriteLine("Parsing line '" + lines[lineIndex] + '"');

            var line = lines[lineIndex].Trim();

            // Lines sometimes have a comments after the section name.
            // That comment can be whatever, sometimes it is a proper comment too (";", "//").
            // -> use StartsWith

            if (line.StartsWith("[Fur Pattern Balls]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.FurPatternBall.FromLine(row);
                    if (item != null)
                    {
                        FurPatternBalls.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Body Area]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.BodyArea.FromLine(row);
                    if (item != null)
                    {
                        BodyAreaz.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Add Ball]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.AddBall.FromLine(row);
                    if (item != null)
                    {
                        AddBallz.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Omissions]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.Omission.FromLine(row);
                    if (item != null)
                    {
                        Omissions.Add(item.IgnoredID);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Project Ball]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.ProjectBall.FromLine(row);
                    if (item != null)
                    {
                        ProjectBallz.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Move]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.Move.FromLine(row);
                    if (item != null)
                    {
                        Movez.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Ballz Info]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.BallzInfo.FromLine(row);
                    if (item != null)
                    {
                        BallzInfoz.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Paint Ballz]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.PaintBall.FromLine(row);
                    if (item != null)
                    {
                        PaintBallz.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Linez]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.Line.FromLine(row);
                    if (item != null)
                    {
                        Linez.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Eyes]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.Eye.FromLine(row);
                    if (item != null)
                    {
                        Eyes.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else if (line.StartsWith("[Whiskers]"))
            {
                ForeachRowInSection((row) =>
                {
                    var item = Lnz.Whisker.FromLine(row);
                    if (item != null)
                    {
                        Whiskers.Add(item);
                    }
                }, lines, ref lineIndex);
            }
            else
            {
                lineIndex++;
            }
        }
    }

    void ForeachRowInSection(Action<string> lineCallback, string[] lines, ref int lineIndex)
    {
        // next line
        lineIndex++;

        while (lineIndex < lines.Length)
        {
            string line = lines[lineIndex].Trim();

            // Remove comment (Some Petz 3 files had it)
            int commentIndex = line.IndexOf("//", 0, StringComparison.Ordinal);
            if (commentIndex != -1)
            {
                line = line.Substring(0, commentIndex);
                line = line.Trim();
            }

            // Console.WriteLine("Parsing item ... '" + lines[lineIndex] + '"');

            // skip empty lines
            bool isEmptyLine = line.Length == 0;
            if (isEmptyLine)
            {
                lineIndex++;

                continue;
            }

            bool nextEntry = line[0] == '[';
            if (nextEntry)
            {
                return; // all items are parsed, keep the current line index
            }

            bool isComment = line[0] == ';' || line[0] == '#';
            if (!isComment)
            {
                lineCallback(line);
            }

            lineIndex++; // next line
        }
    }
}