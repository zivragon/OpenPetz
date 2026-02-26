using Godot;

namespace Lnz
{
    // [Move]
    public class Move
    {
        // https://web.archive.org/web/20190805174318fw_/http://www.angelfire.com/moon2/petzzoo2/id27.htm		//

        public int BallNumber = -1;
        public Vector3 Offset = new Vector3(0, 0, 0);
        public int AnchorBallID = -1; // optional

        public static Move FromLine(string str)
        {
            Move result = new Move();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.BallNumber);
            rowParser.Vector3(ref result.Offset);
            if (!rowParser.isOK)
            {
                return null;
            }

            // optional, so we can ignore if it isn't there
            rowParser.Int(ref result.AnchorBallID);

            return result;
        }
    };

    // [Fur Pattern Balls]
    // ==================== 
    // ; addballs which are part of fur only 
    //     addballs:	92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103	; tail
    public class FurPatternBall
    {
        // public List<int> BallID = new List<int>();

        public static FurPatternBall FromLine(string str)
        {
            // @todo do
            return null;
        }
    };

    //  [Add ball]
    public class AddBall
    {
        // https://web.archive.org/web/20190805174318fw_/http://www.angelfire.com/moon2/petzzoo2/id27.htm		//

        /** parent ball id */
        public int ParentBallID = 0;
        public Vector3 Offset = new Vector3(0, 0, 0); // in relation to center of the base
        public int Color = 0;
        public int OutlineColor = 0;
        public int spckCol = 0; // @todo ???
        public float Fuzz = 0;
        public int Group = 0; // @todo same as ball info
        public int Outline = 0;
        public float BallSize = 0;
        public float BodyArea = 0;
        public int AddGroup = 0; // ???
        public int Texture = 0;

        public static AddBall FromLine(string str)
        {
            AddBall result = new AddBall();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.ParentBallID);
            rowParser.Vector3(ref result.Offset);
            rowParser.Int(ref result.Color);
            rowParser.Int(ref result.OutlineColor);
            rowParser.Int(ref result.spckCol);
            rowParser.Float(ref result.Fuzz);
            rowParser.Int(ref result.Group);
            rowParser.Enum(ref result.Outline, new int[] { -1, 0, -2 });
            rowParser.Float(ref result.BallSize);
            rowParser.Float(ref result.BodyArea);
            rowParser.Int(ref result.AddGroup);
            rowParser.Int(ref result.Texture);

            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

    // [Body Area]
    public class BodyArea
    {
        // https://web.archive.org/web/20190805174318fw_/http://www.angelfire.com/moon2/petzzoo2/id27.htm		//

        public int BallID = 0;
        public float Area = 0;

        public static BodyArea FromLine(string str)
        {
            var result = new BodyArea();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.BallID);
            rowParser.Float(ref result.Area);
            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

    // [Omissions]
    public class Omission
    {
        // https://web.archive.org/web/20190805174318fw_/http://www.angelfire.com/moon2/petzzoo2/id27.htm		//

        public int IgnoredID = -1;

        public static Omission FromLine(string str)
        {
            var result = new Omission();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.IgnoredID);
            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

    // [Project Ball]
    public class ProjectBall
    {
        // https://web.archive.org/web/20190805174318fw_/http://www.angelfire.com/moon2/petzzoo2/id27.htm		//

        public int FixedBallID = -1;
        public int BallToMoveID = -1;
        public float Distance = 0;

        public static ProjectBall FromLine(string str)
        {
            ProjectBall result = new ProjectBall();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.FixedBallID);
            rowParser.Int(ref result.BallToMoveID);
            rowParser.Float(ref result.Distance);
            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

    // [Ballz Info]
    public class BallzInfo
    {
        // https://web.archive.org/web/20190805174318fw_/http://www.angelfire.com/moon2/petzzoo2/id27.htm		//

        public int Color = 0;
        public int OutlineColor = 0;
        public int spklCl = 0; // ???
        public float Fuzz = 0;
        public int otlntTyp = 0;
        public float SizeDif = 0;
        public int Group = 0;
        public int Texture = 0;
        public string BallID; // can be "eBall_irisL"

        public static BallzInfo FromLine(string str)
        {
            BallzInfo result = new BallzInfo();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.Color);
            rowParser.Int(ref result.OutlineColor);
            rowParser.Int(ref result.spklCl);
            rowParser.Float(ref result.Fuzz);
            rowParser.Int(ref result.otlntTyp);
            rowParser.Float(ref result.SizeDif);
            rowParser.Int(ref result.Group);
            rowParser.Int(ref result.Texture);
            rowParser.String(ref result.BallID);
            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

    // [Paint Ballz]
    public class PaintBall
    {
        //
        // https://web.archive.org/web/20240313175338/https://homebody.eu/carolyn/downloabx/tutorials/tutorialbits/LNZ2nd.txt
        //
        // ;base ball diameter(% of baseball) direction color outlinecolor fuzz outline group texture

        public int BaseBall = 0; // @todo ball index
        public float Diameter = 0;
        public Vector3 Direction = new Vector3(0, 0, 0);
        public int Color = 0;
        public int OutlineColor = 0;
        public float Fuzz = 0;
        public int Outline = 0;
        public int Group = 0;
        public int Texture = 0;

        public static PaintBall FromLine(string str)
        {
            PaintBall result = new PaintBall();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.BaseBall);
            rowParser.Percentage(ref result.Diameter);
            rowParser.Vector3(ref result.Direction);
            rowParser.Int(ref result.Color);
            rowParser.Int(ref result.OutlineColor);
            rowParser.Float(ref result.Fuzz);
            rowParser.Int(ref result.Outline);
            rowParser.Int(ref result.Group);
            rowParser.Int(ref result.Texture);
            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

    // https://web.archive.org/web/20220702142153/https://petz.filthyhippie.net/tutorials/lnz-pro-101/
    public class Line
    {
        // The 1st column is the start ball, or the ball number the line comes from.
        public int StartBall = 0;

        // The 2nd column is the end ball, the ball number the line connects to.
        public int EndBall = 0;

        // The 3rd column is the fuzz amount of the line.
        public float FuzzAmount = 0;

        // The 4th column always needs to be -1. This makes the line take on the color AND texture of it’s start ball.
        public int Color = 0;

        // The 5th and 6th columns are the outline colors of the left and right side of the line, respectively.
        public int LeftOutlineColor = 0;

        public int RightOutlineColor = 0;

        // The 7th and 8th columns are the starting thickness and ending thickness of the line.
        public int StartThickness = 0;
        public int EndThickness = 0;

        public static Line FromLine(string str)
        {
            Line result = new Line();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.StartBall);
            rowParser.Int(ref result.EndBall);
            rowParser.Float(ref result.FuzzAmount);
            rowParser.Int(ref result.Color); // The color of the line (usually left at -1).
            rowParser.Int(ref result.LeftOutlineColor);
            rowParser.Int(ref result.RightOutlineColor);
            rowParser.Int(ref result.StartThickness);
            rowParser.Int(ref result.EndThickness);
            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

    public class Whisker
    {
        public int StartBallID = 0;
        public int EndBallID = 0;
        public int Color = 0; // @todo what is the default value?

        public static Whisker FromLine(string str)
        {
            Whisker result = new Whisker();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.StartBallID);
            rowParser.Int(ref result.EndBallID);
            if (!rowParser.isOK)
            {
                return null;
            }

            // not required
            rowParser.Int(ref result.Color);

            return result;
        }
    };

    // [Ear Extension]
    public class EarExtension
    {
        public int Percent = 0;

        public static EarExtension FromLine(string str)
        {
            EarExtension result = new EarExtension();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.Percent);
            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

    /*

        https://petz.miraheze.org/wiki/LNZ

     */
    public class Eye
    {
        public int X = 0;
        public int Y = 0;
        public string ID = "";

        /*
        EXAMPLE:

        [Eyes]
        12, 34			RightEye/leftEye
        28, 56 			RightIris/leftIris

        */
        public static Eye FromLine(string str)
        {
            Eye result = new Eye();

            LnzRowParser rowParser = new LnzRowParser(str);
            rowParser.Int(ref result.X);
            rowParser.Int(ref result.Y);
            rowParser.String(ref result.ID);
            if (!rowParser.isOK)
            {
                return null;
            }

            return result;
        }
    };

}
