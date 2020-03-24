using UnityEngine.Rendering;
using System;

[VolumeComponentMenu("Lighting/Screen Space Ambient Occlusion")]
class ScreenSpaceAmbientOcclusionVolume : VolumeComponent
{
    // Public properties
    public BoolParameter downSample = new BoolParameter(false);
    public ClampedIntParameter sampleCount = new ClampedIntParameter(10, 2, 32, false);
    public ClampedFloatParameter intensity = new ClampedFloatParameter(2f, 0f, 10f, false);
    public ClampedFloatParameter radius = new ClampedFloatParameter(0.05f, 0.001f, 10.0f, false);
    public NormalReconstructionQuality normalReconstructionQuality = new NormalReconstructionQuality(ScreenSpaceAmbientOcclusionFeature.NormalReconstructionQuality.Medium);

    // Classes
    [Serializable]
    public sealed class NormalReconstructionQuality : VolumeParameter<ScreenSpaceAmbientOcclusionFeature.NormalReconstructionQuality>
    {
        public NormalReconstructionQuality(ScreenSpaceAmbientOcclusionFeature.NormalReconstructionQuality value, bool overrideState = false) : base(value, overrideState)
        {
        }
    }

    // This will be used once we've exposed render feature requirements.
    //public DepthSource depthSource = new DepthSource(ScreenSpaceAmbientOcclusionFeature.DepthSource.Depth);
    //[Serializable]
    //public sealed class DepthSource : VolumeParameter<ScreenSpaceAmbientOcclusionFeature.DepthSource>
    //{
    //    public DepthSource(ScreenSpaceAmbientOcclusionFeature.DepthSource value, bool overrideState = false) : base(value, overrideState)
    //    {
    //    }
    //}
}
