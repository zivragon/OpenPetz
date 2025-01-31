using System.Runtime.InteropServices;

namespace OpenPetz {
//  GAME STRUCTS FOR BINARY FILE PARSING PURPOSES -- .bhd, .bdt
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct BhdSerialized {                                    //  memory layout of a BHD file
        [FieldOffset(0x000)] public short HeaderSize;                           //  [+000]  in bytes
        [FieldOffset(0x002)] public short BallFrameHeaderSize;                  //  [+002]  in bytes
        [FieldOffset(0x004)] public short FileVersion;                          //  [+004]
        [FieldOffset(0x006)] public short NumBallz;                             //  [+006]  number of ballz in the skeleton
        [FieldOffset(0x008)] public int StartFrame;                             //  [+008]  starting frame number
        [FieldOffset(0x00C)] public int NumFrames;                              //  [+00C]  total number of frames
        [FieldOffset(0x010)] public int StandFrame;                             //  [+010]  (todo: RE this. seems more important in toyz)
        [FieldOffset(0x014)] public short Unk_14;                               //  [+014]  
        [FieldOffset(0x016)] public short Unk_16;                               //  [+016]
        [FieldOffset(0x018)] public short Unk_18;                               //  [+018]
        [FieldOffset(0x01A)] public short Unk_1A;                               //  [+01A]  (todo: RE these.)
        [FieldOffset(0x01C)] public short Unk_1C;                               //  [+01C]  (they seem to represent minimum and maximum XYZ values for something)
        [FieldOffset(0x01E)] public short Unk_1E;                               //  [+01E]
        [FieldOffset(0x020)] public short Unk_20;                               //  [+020]
        [FieldOffset(0x022)] public short Unk_22;                               //  [+022]
        [FieldOffset(0x024)] public short Unk_24;                               //  [+024]
        [FieldOffset(0x026)] public fixed short BallSizes[67];                  //  [+026]  default size of each ball in the skeleton
        [FieldOffset(0x0AC)] public short NumFrameGroups;                       //  [+0AC]  number of animations
        [FieldOffset(0x0AE)] public fixed short FrameGroupFirstRawFrame[1500];  //  [+0AE]  length of each animation in frames
        [FieldOffset(0xC66)] public int FrameOffsets;                           //  [+C66]  (evil, use pointer) array of byte offset to the start of each frame in its respective BDT file
    };


    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct BdtHeaderSerialized {                              //  memory layout of a BDT file header
        [FieldOffset(0x000)] public int FileSize;                               //  [+000]  in bytes
        [FieldOffset(0x004)] public short FileVersion;                          //  [+004]  must be 0x0E
        [FieldOffset(0x006)] public fixed byte Checksum[78];                    //  [+006]  "PFM_[V4.015] (c) 1999 The Learning Company Properties, Inc. (Hi Mom, indeed!)\x00"
    };


    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct BdtFrameHeaderSerialized {                         //  memory layout of a BDT frame header
        [FieldOffset(0x000)] public XTVector3_Int16 MinBounds;                  //  [+000]  minimum XYZ values of ballz in this frame
        [FieldOffset(0x006)] public XTVector3_Int16 MaxBounds;                  //  [+006]  maximum XYZ values of ballz in this frame
        [FieldOffset(0x00C)] public short Flags;                                //  [+00C]  (todo: RE this)
    };


    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct BdtFrame {                                         //  memory layout of a BDT frame (WARNING: ASSUMES SKELETON HAS EXACTLY 67 REAL BALLZ)
        [FieldOffset(0x000)] public BdtFrameHeaderSerialized Header;            //  [+000]  frame header
        [FieldOffset(0x00E)] public XPointRot3_16 Ballz;                        //  [+00E]  (evil, use pointer) array of ballz positions and rotations
        [FieldOffset(0x2AC)] public BdtBallSizeOverrideArray SizeArray;         //  [+2AC]  ballz whose size we're overriding

        [StructLayout(LayoutKind.Explicit)]
        public unsafe struct BdtBallSizeOverrideArray {                     //  memory layout of an array of ball size overrides
            [FieldOffset(0x000)] public short ArrayLength;                      //  [+000]  number of ballz whose size we're overriding (CAREFUL: MIGHT BE 0)
            [FieldOffset(0x002)] public BdtFrameBallSizeOverride SizeOverrides; //  [+002]  (evil, use pointer) array of ball size overrides

            [StructLayout(LayoutKind.Explicit)]
            public unsafe struct BdtFrameBallSizeOverride {                 //  memory layout of a single ball size override
                [FieldOffset(0x000)] public short Ball;                         //  [+000]  ball number
                [FieldOffset(0x002)] public short SizeDiff;                     //  [+002]  offset from default size
            };
        };
    };
    

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct XTVector3_Int16 {                      //  memory layout of a 3D vector
        [FieldOffset(0x000)] public short X;                        //  [+000]  x-axis position
        [FieldOffset(0x002)] public short Y;                        //  [+002]  y-axis position
        [FieldOffset(0x004)] public short Z;                        //  [+004]  z-axis position
    };

    
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct XPointRot3_16 {                        //  memory layout of a 3D point with rotation
        [FieldOffset(0x000)] public XTVector3_Int16 Position;       //  [+000]  xyz position
        [FieldOffset(0x006)] public sbyte Tilt;                     //  [+006]  x-axis rotation
        [FieldOffset(0x007)] public sbyte Rotation;                 //  [+007]  y-axis rotation
        [FieldOffset(0x008)] public sbyte Roll;                     //  [+008]  z-axis rotation
        [FieldOffset(0x009)] public sbyte AfterTilt;                //  [+009]  extra x-axis rotation after all other rotations are applied
    };
};
