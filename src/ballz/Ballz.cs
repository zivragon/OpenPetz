using Godot;
using System;
using System.IO;
using System.Collections.Generic;

namespace OpenPetz {
    public class BallOrientation3D {
        public Vector3  Position    { get; }
        public float    PosX => Position.X;
        public float    PosY => Position.Y;
        public float    PosZ => Position.Z;

        public Vector3  Rotation    { get; }
        public float    RotX => Rotation.X;
        public float    RotY => Rotation.Y;
        public float    RotZ => Rotation.Z;
        public float    AfterTilt   { get; }


        public BallOrientation3D(float posx, float posy, float posz, float tilt, float rotation, float roll, float aftertilt) {
            Position = new Vector3(posx, posy, posz);
			Rotation = new Vector3(tilt, rotation, roll);
            AfterTilt = aftertilt;
        }

        public BallOrientation3D(Vector3 pos, Vector3 rot, float afterTilt) {
            Position = pos;
            Rotation = rot;
            AfterTilt = afterTilt;
        }

        public BallOrientation3D(XPointRot3_16 orientation) {
            Position = new Vector3(orientation.Position.X, orientation.Position.Y, orientation.Position.Z);
            Rotation = new Vector3(orientation.Tilt, orientation.Rotation, orientation.Roll);
            AfterTilt = orientation.AfterTilt;
        }
        
        public unsafe BallOrientation3D(XPointRot3_16* orientation) {
            Position = new Vector3(orientation->Position.X, orientation->Position.Y, orientation->Position.Z);
            Rotation = new Vector3(orientation->Tilt, orientation->Rotation, orientation->Roll);
            AfterTilt = orientation->AfterTilt;
        }

        public BallOrientation3D(BallOrientation3D other) {
            Position = other.Position;
            Rotation = other.Rotation;
            AfterTilt = other.AfterTilt;
        }
    };


    unsafe public partial class BallzModel {
        private List<int>          AnimationFirstRawFrame;      //  so we can locate the corresponding animation for a given raw frame number
//      --------------------------
        public List<FrameGroup>     Animations      { get; }    //  list of all animations
        public List<int>            BallSizes       { get; }    //  default size of each ball in the skeleton
        public int                  NumBallz        { get; }    //  number of ballz in the skeleton
        public int                  NumAnimations   { get; }    //  number of animations
        public int                  NumFrames       { get; }    //  total number of frames in all animations
        public int                  StartFrame      { get; }    //  todo
        public int                  StandFrame      { get; }    //  todo



        public BallzModel(string bhdPath, List<string> bdtFiles) {
            fixed (byte* pBhdBytes = File.ReadAllBytes(bhdPath)) {
                BhdSerialized* pBhd = (BhdSerialized*)pBhdBytes;
                int* pFrameOffsets = &pBhd->FrameOffsets;           //  establish pointer to the beginning of an array of frame offsets
                                                                    //  these are the starting byte offsets of each frame within their respective bdt files

//              .................................
//              ...TODO: safety checks for BHD...
//              .................................


//              easy variables
                NumBallz = pBhd->NumBallz;
                NumAnimations = pBhd->NumFrameGroups;
                NumFrames = pBhd->NumFrames;
                StartFrame = pBhd->StartFrame;
                StandFrame = pBhd->StandFrame;
                
                BallSizes = new List<int>(NumBallz);

//              parse default ball sizes
                for (int i = 0; i < NumBallz; i++) {
                    BallSizes.Add(pBhd->BallSizes[i]);
                };

                int thisNumFrames = 0;
                int frameGroupEnd = 0;
                int rawFrameNumber = 0;
                Animations = new List<FrameGroup>(/*NumAnimations*/);
                AnimationFirstRawFrame = new List<int>(/*NumAnimations*/);

//              parse animations
                for (int i = 0; i < /*NumAnimations*/ 50; i++) {
                    frameGroupEnd = pBhd->FrameGroupFirstRawFrame[i];
                    thisNumFrames = frameGroupEnd - rawFrameNumber;
                    AnimationFirstRawFrame.Add(rawFrameNumber);
                    Animations.Add(new(bdtFiles[i], NumBallz, thisNumFrames, pFrameOffsets)); //???
                    rawFrameNumber += thisNumFrames;
                    pFrameOffsets += thisNumFrames;                //  advance pointer to next animation's first frame's offset
                };
            };
        }

        public FrameGroup GetAnimation(int i) {
            if (i < 0 || i >= NumAnimations) {
                throw new ArgumentOutOfRangeException(nameof(i), "Animation index out of range");
            };
            return Animations[i];
        }

        public Frame GetFrameInAnimation(int aIndex, int fIndex) {
            if (aIndex < 0 || aIndex >= NumAnimations) {
                throw new ArgumentOutOfRangeException(nameof(aIndex), "Animation index out of range");
            };
            if (fIndex < 0 || fIndex >= Animations[aIndex].NumFrames) {
                throw new ArgumentOutOfRangeException(nameof(fIndex), "Frame index out of range");
            };
            return Animations[aIndex].Frames[fIndex];
        }

        public Frame GetRawFrame(int fNumber) {
            if (fNumber < 0 || fNumber >= NumFrames) {
                throw new ArgumentOutOfRangeException(nameof(fNumber), "Frame number out of range");
            };

//          do a binary search to find the animation that contains the frame
            int low = 0;
            int high = NumAnimations - 1;

            while (low <= high) {
                int mid = low + (high - low) / 2;
                if (fNumber < AnimationFirstRawFrame[mid]) {
                    high = mid - 1;
                } else if (fNumber >= AnimationFirstRawFrame[mid] + Animations[mid].Frames.Count) {
                    low = mid + 1;
                } else {
                    return Animations[mid].Frames[(fNumber - AnimationFirstRawFrame[mid])];
                };
            };
            throw new Exception("Frame not found");
        }

        public int GetDefaultBallSize(int bNumber) {
            if (bNumber < 0 || bNumber >= NumBallz) {
                throw new ArgumentOutOfRangeException(nameof(bNumber), "Ball number out of range");
            };
            return BallSizes[bNumber];
        }

        public int GetBallSizeThisFrame(int bNumber, int fNumber) {
            if (bNumber < 0 || bNumber >= NumBallz) {
                throw new ArgumentOutOfRangeException(nameof(bNumber), "Ball number out of range");
            };
            return GetRawFrame(fNumber).BallSizeOffset(bNumber);
        }

        public int GetBallSizeThisFrame(int bNumber, int aIndex, int fIndex) {
            if (bNumber < 0 || bNumber >= 67) {
                throw new ArgumentOutOfRangeException(nameof(bNumber), "Ball number out of range");
            };
            return GetFrameInAnimation(aIndex, fIndex).BallSizeOffset(bNumber);
        }
    };
};
