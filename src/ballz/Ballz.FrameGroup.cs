using System.Collections.Generic;
using System.IO;

namespace OpenPetz {
    public unsafe partial class BallzModel {
//      class for an animation (a group of frames)
        public unsafe class FrameGroup {
            public List<Frame>  Frames    { get; set; }
            public int          NumFrames   { get; }
            
//          parse all frames in the group (animation); frameOffsets points to BHD FrameOffsets
            public FrameGroup(string bdtPath, int numRealBallz, int numFrames, int* frameOffsets) {
                Frames = new List<Frame>(numFrames);

                fixed (byte* pBdtBytes = File.ReadAllBytes(bdtPath)) {
//                  .................................
//                  ...TODO: safety checks for BDT...
//                  .................................

                    for (int i = 0; i < numFrames; i++) {
                        Frame frame = new((BdtFrame*)(pBdtBytes + frameOffsets[i]), numRealBallz);
                        Frames[i] = frame;
                    };
                    NumFrames = numFrames;
                };
            }
        };
    };
};