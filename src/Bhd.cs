using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;

namespace OpenPetz {
    public class BallOrientation3D {
        public Vector3 Position { get; }
        public Vector3 Rotation { get; }

        public BallOrientation3D(float posx, float posy, float posz, float tilt, float rotation, float roll, float aftertilt) {
            Position = new Vector3(posx, posy, posz);
			Rotation = new Vector3(tilt, rotation, roll);
        }
    };


	unsafe /*internal*/ public class Bhd {
		public List<FrameGroup> m_Animations { get;/* set;*/}
		public List<nint> m_BallSizes { get;/* set;*/}
		public nint NumBallz { get; }
		public nint NumAnimations { get => m_Animations.Count; }
		public nint NumFrames { get; }
		public nint StartFrame { get; }
		public nint StandFrame { get; }


        private List<nint> m_AnimationFirstRawFrame;    //  so we can locate the corresponding animation for a given raw frame number

        public Bhd(string bhdPath, List<string> bdtFiles) {
            m_Animations = new List<FrameGroup>();
            m_AnimationFirstRawFrame = new List<nint>();
            fixed (byte* pBhdBytes = File.ReadAllBytes(bhdPath)) {
                BhdSerialized* pBhd = (BhdSerialized*)pBhdBytes;
                int* pFrameOffsets = &pBhd->FrameOffsets;           //  establish pointer to the first animation's first frame offset

//              .................................
//              ...TODO: safety checks for BHD...
//              .................................

                NumBallz = pBhd->NumBallz;
                NumAnimations = /*pBhd->NumFrameGroups*/2;
                NumFrames = pBhd->NumFrames;
                StartFrame = pBhd->StartFrame;
                StandFrame = pBhd->StandFrame;

//              parse default ball sizes
                m_BallSizes = new List<nint>();
                for (nint i = 0; i < 67; i++) {
                    m_BallSizes.Add(pBhd->BallSizes[i]);
                };

//              parse animations
                nint rawFrameNumber = 0;
                for (nint i = 0; i < NumAnimations; i++) {
                    int frameGroupSize = pBhd->FrameGroupSizes[i];
                    m_Animations.Add(new(bdtFiles[(int)i], frameGroupSize - rawFrameNumber, pFrameOffsets));
                    m_AnimationFirstRawFrame.Add(rawFrameNumber);
                    rawFrameNumber += frameGroupSize;
                    pFrameOffsets += frameGroupSize;                //  advance pointer to next animation's first frame offset
                };
            };
        }

        public FrameGroup GetAnimation(int index) {
            if (index < 0 || index >= NumAnimations) {
                throw new ArgumentOutOfRangeException(nameof(index), "Animation index out of range");
            };
            return m_Animations[index];
        }

        public Frame GetFrameInAnimation(int animationIndex, int frameIndex) {
            if (animationIndex < 0 || animationIndex >= NumAnimations) {
                throw new ArgumentOutOfRangeException(nameof(animationIndex), "Animation index out of range");
            };
            if (frameIndex < 0 || frameIndex >= m_Animations[animationIndex].NumFrames) {
                throw new ArgumentOutOfRangeException(nameof(frameIndex), "Frame index out of range");
            };
            return m_Animations[animationIndex].m_Frames[frameIndex];
        }

        public Frame GetRawFrame(int frameNumber) {
            if (frameNumber < 0 || frameNumber >= NumFrames) {
                throw new ArgumentOutOfRangeException(nameof(frameNumber), "Frame number out of range");
            };

//          do a binary search to find the animation that contains the frame
            nint low = 0;
            nint high = NumAnimations - 1;

            while (low <= high) {
                nint mid = low + (high - low) / 2;
                if (frameNumber < m_AnimationFirstRawFrame[(int)mid]) {
                    high = mid - 1;
                } else if (frameNumber >= m_AnimationFirstRawFrame[(int)mid] + m_Animations[(int)mid].m_Frames.Count) {
                    low = mid + 1;
                } else {
                    return m_Animations[(int)mid].m_Frames[(int)(frameNumber - m_AnimationFirstRawFrame[(int)mid])];
                };
            };
            throw new Exception("Frame not found");
        }

        public nint GetDefaultBallSize(nint ballNumber) {
            if (ballNumber < 0 || ballNumber >= 67) {
                throw new ArgumentOutOfRangeException(nameof(ballNumber), "Ball number out of range");
            };
            return m_BallSizes[(int)ballNumber];
        }

        public nint GetBallSizeThisFrame(nint ballNumber, int frameNumber) {
            if (ballNumber < 0 || ballNumber >= 67) {
                throw new ArgumentOutOfRangeException(nameof(ballNumber), "Ball number out of range");
            };
            return GetRawFrame(frameNumber).BallSizeOffset(ballNumber);
        }

        public nint GetBallSizeThisFrame(nint ballNumber, int animationIndex, int frameIndex) {
            if (ballNumber < 0 || ballNumber >= 67) {
                throw new ArgumentOutOfRangeException(nameof(ballNumber), "Ball number out of range");
            };
            return GetFrameInAnimation(animationIndex, frameIndex).BallSizeOffset(ballNumber);
        }

//      class for an animation (a group of frames)
        unsafe /*internal*/ public class FrameGroup {
            public List<Frame> m_Frames { get; set; }
            public nint NumFrames { get; }

            public FrameGroup(string bdtPath, nint numFrames, int* frameOffsets) {
//              parse all frames in the animation; frameOffsets points to BHD FrameOffsets
                m_Frames = new List<Frame>();
                fixed (byte* pBdtBytes = File.ReadAllBytes(bdtPath)) {
                    
//                  .................................
//                  ...TODO: safety checks for BDT...
//                  .................................

                    for (nint i = 0; i < numFrames; i++) {
                        Frame frame = new((BdtFrame*)(pBdtBytes + frameOffsets[i]));
                        m_Frames.Add(frame);
                    };
                };
            }
        };


//      class for a single frame of an animation
        unsafe /*internal*/ public class Frame {
            List<KeyValuePair<nint /*ball number*/, Tuple<BallOrientation3D, nint> /*orientation, sizeoffset*/>> m_BallzData;

            public Frame(BdtFrame* pBallArray) {
                m_BallzData = new List<KeyValuePair<nint, Tuple<BallOrientation3D, nint>>>();
//              temporary lookup for ball size overrides (temporarily disabled)
                BdtFrame.BdtBallSizeOverrideArray.BdtFrameBallSizeOverride* pOverrideArray = &pBallArray->SizeArray.SizeOverrides;
                Dictionary<nint, short> sizeOverrides = new Dictionary<nint, short>();
                for (nint k = 0; k < pBallArray->SizeArray.ArrayLength; k++) {
                    sizeOverrides.Add(pOverrideArray->Ball, pOverrideArray->SizeDiff);
                    pOverrideArray++;
                };

//              for each ball in the frame, parse its position/rotation/size offset
                XPointRot3_16* pBall = &pBallArray->Ballz;
	
                for (nint k = 0; k < 67; k++) {
                    m_BallzData.Add(new(k, new(new BallOrientation3D(pBall->Position.X, pBall->Position.Y, pBall->Position.Z, pBall->Tilt, pBall->Rotation, pBall->Roll, pBall->AfterTilt), sizeOverrides.ContainsKey(k) ? sizeOverrides[k] : 0)));
                    pBall++;
                };
            }

            public BallOrientation3D BallOrientation(nint ballNumber) {
                return m_BallzData.Find(x => x.Key == ballNumber).Value.Item1;
            }

            public nint BallSizeOffset(nint ballNumber) {
                return m_BallzData.Find(x => x.Key == ballNumber).Value.Item2;
            }
        };
    };



//  GAME STRUCTS FOR PARSING PURPOSES

    [StructLayout(LayoutKind.Explicit)]
    /*internal*/ public unsafe struct BhdSerialized {                                  //  memory layout of an entire BHD file
        [FieldOffset(0x000)] public short HeaderSize;                       //  [+000]  in bytes
        [FieldOffset(0x002)] public short BallFrameHeaderSize;              //  [+002]  in bytes
        [FieldOffset(0x004)] public short FileVersion;                      //  [+004]
        [FieldOffset(0x006)] public short NumBallz;                         //  [+006]  number of ballz in the skeleton
        [FieldOffset(0x008)] public int StartFrame;                         //  [+008]  starting frame number
        [FieldOffset(0x00C)] public int NumFrames;                          //  [+00C]  total number of frames
        [FieldOffset(0x010)] public int StandFrame;                         //  [+010]  (todo: RE this. seems more important in toyz)
        [FieldOffset(0x014)] public short Unk_14;                           //  [+014]  
        [FieldOffset(0x016)] public short Unk_16;                           //  [+016]
        [FieldOffset(0x018)] public short Unk_18;                           //  [+018]
        [FieldOffset(0x01A)] public short Unk_1A;                           //  [+01A]
        [FieldOffset(0x01C)] public short Unk_1C;                           //  [+01C]  (these unknown values seem to represent minimum and maximum XYZ values for something)
        [FieldOffset(0x01E)] public short Unk_1E;                           //  [+01E]
        [FieldOffset(0x020)] public short Unk_20;                           //  [+020]
        [FieldOffset(0x022)] public short Unk_22;                           //  [+022]
        [FieldOffset(0x024)] public short Unk_24;                           //  [+024]
        [FieldOffset(0x026)] public fixed short BallSizes[67];              //  [+026]  default size of each ball in the skeleton
        [FieldOffset(0x0AC)] public short NumFrameGroups;                   //  [+0AC]  number of animations
        [FieldOffset(0x0AE)] public fixed short FrameGroupSizes[1500];      //  [+0AE]  length of each animation in frames
        [FieldOffset(0xC66)] public int FrameOffsets;                       //  [+C66]  (evil, use pointer) array of byte offset to the start of each frame in its respective BDT file
    };


    [StructLayout(LayoutKind.Explicit)]
    /*internal*/ public unsafe struct BdtHeaderSerialized {                            //  memory layout of a BDT file header
        [FieldOffset(0x000)] public int FileSize;                           //  [+000]  in bytes
        [FieldOffset(0x004)] public short FileVersion;                      //  [+004]  must be 0x0E
        [FieldOffset(0x006)] public fixed byte Checksum[78];                //  [+006]  "PFM_[V4.015] (c) 1999 The Learning Company Properties, Inc. (Hi Mom, indeed!)\x00"
    };


    [StructLayout(LayoutKind.Explicit)]
    /*internal*/ public unsafe struct BdtFrameHeaderSerialized {                       //  memory layout of a BDT frame header
        [FieldOffset(0x000)] public XTVector3_Int16 MinBounds;              //  [+000]  minimum XYZ values of ballz in this frame
        [FieldOffset(0x006)] public XTVector3_Int16 MaxBounds;              //  [+006]  maximum XYZ values of ballz in this frame
        [FieldOffset(0x00C)] public short Flags;                            //  [+00C]  (todo: RE this)
    };


    [StructLayout(LayoutKind.Explicit)]
    /*internal*/ public unsafe struct BdtFrame {                                       //  memory layout of a BDT frame
        [FieldOffset(0x000)] public BdtFrameHeaderSerialized Header;        //  [+000]  frame header
        [FieldOffset(0x00E)] public XPointRot3_16 Ballz;                    //  [+00E]  (evil, use pointer) array of ballz positions and rotations
        [FieldOffset(0x2AC)] public BdtBallSizeOverrideArray SizeArray;     //  [+2AC]  ballz whose size we're overriding

        [StructLayout(LayoutKind.Explicit)]
        /*internal*/ public unsafe struct BdtBallSizeOverrideArray {                           //  memory layout of an array of ball size overrides
            [FieldOffset(0x000)] public short ArrayLength;                          //  [+000]  number of ballz whose size we're overriding (CAREFUL: MIGHT BE 0)
            [FieldOffset(0x002)] public BdtFrameBallSizeOverride SizeOverrides;     //  [+002]  (evil, use pointer) array of ball size overrides

            [StructLayout(LayoutKind.Explicit)]
            /*internal*/ public unsafe struct BdtFrameBallSizeOverride {               //  memory layout of a single ball size override
                [FieldOffset(0x000)] public short Ball;                     //  [+000]  ball number
                [FieldOffset(0x002)] public short SizeDiff;                 //  [+002]  offset from default size
            };
        };
    };
    

    [StructLayout(LayoutKind.Explicit)]
    /*internal*/ public unsafe struct XTVector3_Int16 {        //  memory layout of a 3D vector
        [FieldOffset(0x000)] public short X;        //  [+000]  x-axis position
        [FieldOffset(0x002)] public short Y;        //  [+002]  y-axis position
        [FieldOffset(0x004)] public short Z;        //  [+004]  z-axis position
    };

    
    [StructLayout(LayoutKind.Explicit)]
    /*internal*/ public unsafe struct XPointRot3_16 {                      //  memory layout of a 3D point with rotation
        [FieldOffset(0x000)] public XTVector3_Int16 Position;   //  [+000]  xyz position
        [FieldOffset(0x006)] public sbyte Tilt;                 //  [+006]  x-axis rotation
        [FieldOffset(0x007)] public sbyte Rotation;             //  [+007]  y-axis rotation
        [FieldOffset(0x008)] public sbyte Roll;                 //  [+008]  z-axis rotation
        [FieldOffset(0x009)] public sbyte AfterTilt;            //  [+009]  extra x-axis rotation after all other rotations are applied
    };
};