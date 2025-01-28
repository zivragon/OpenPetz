using System;
using System.Linq;
using Godot;

class LnzRowParser
{
    string[] parts;
    int index = 0;
    public bool isOK = true;

    public LnzRowParser(string row)
    {
        parts = row.Split(
            new char[] { ' ', '\t', ',' },
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
        );
    }

    public void Percentage(ref float percentage)
    {
        this.Float(ref percentage);
        if (isOK)
        {
            percentage = percentage / 100f;
        }
    }

    public void Vector3(ref Vector3 outValue)
    {
        float X = 0;
        float Y = 0;
        float Z = 0;

        if (!isOK)
        {
            return;
        }

        // need to have space for 3 items
        if (index + 2 >= parts.Length)
        {
            isOK = false;
            return;
        }

        this.Float(ref X);
        this.Float(ref Y);
        this.Float(ref Z);

        if (isOK)
        {
            outValue.X = X;
            outValue.Y = Y;
            outValue.Z = Z;
        }
        else
        {
            isOK = false;
        }
    }

    public void Int(ref int intValue)
    {
        if (!isOK)
        {
            return;
        }

        if (index >= parts.Length)
        {
            isOK = false;
            return;
        }

        if (int.TryParse(parts[index], out intValue))
        {
            index++; // prepare to read next value
        }
        else
        {
            isOK = false;
        }
    }

    public void Enum(ref int outValue, int[] validValues)
    {
        if (!isOK)
        {
            return;
        }

        if (index >= parts.Length)
        {
            isOK = false;
            return;
        }

        int value = 0;
        if (int.TryParse(parts[index], out value))
        {
            if (validValues.Contains(value))
            {
                outValue = value;

                index++; // prepare to read next value
            }
            else
            {
                isOK = false;
            }
        }
        else
        {
            isOK = false;
        }
    }

    public void Float(ref float outValue)
    {
        if (!isOK)
        {
            return;
        }

        if (index >= parts.Length)
        {
            isOK = false;
            return;
        }

        // ".3" can't be parsed by just `float.TryParse(".3", out value)`
        if (float.TryParse(
                parts[index],
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out outValue
            ))
        {
            index++; // prepare to read next value
        }
        else
        {
            isOK = false;
        }
    }

    public void String(ref string stringValue)
    {
        if (!isOK)
        {
            return;
        }

        if (index >= parts.Length)
        {
            isOK = false;
            return;
        }

        stringValue = parts[index];
        index++;
    }


    public void Test()
    {
        // test parsing the sequence and float values
        var rp = new LnzRowParser(".25 0.125 123456 123.125");

        float X = 0;

        rp.Float(ref X);
        if (X != .25f)
        {
            GD.Print("Parsing .25 failed");
        }

        rp.Float(ref X);
        if (X != .125f)
        {
            GD.Print("Parsing 0.125 failed");
        }

        rp.Float(ref X);
        if (X != 123456f)
        {
            GD.Print("Parsing 123456f failed");
        }

        rp.Float(ref X);
        if (X != 123.125f)
        {
            GD.Print("Parsing 123.125 failed");
        }
    }
};