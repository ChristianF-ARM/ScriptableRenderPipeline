using System;

namespace UnityEngine.Rendering.Universal
{
    public enum BloomDownsample
    {
        Half = 1,
        Quarter = 2,
    }

    [Serializable, VolumeComponentMenu("Post-processing/Bloom")]
    public sealed class Bloom : VolumeComponent, IPostProcessComponent
    {
        [Tooltip("Filters out pixels under this level of brightness. Value is in gamma-space.")]
        public MinFloatParameter threshold = new MinFloatParameter(0.9f, 0f);

        [Tooltip("Strength of the bloom filter.")]
        public MinFloatParameter intensity = new MinFloatParameter(0f, 0f);

        [Tooltip("Changes the extent of veiling effects.")]
        public ClampedFloatParameter scatter = new ClampedFloatParameter(0.7f, 0f, 1f);

        [Tooltip("Clamps pixels to control the bloom amount.")]
        public MinFloatParameter clamp = new MinFloatParameter(65472f, 0f);

        [Tooltip("Global tint of the bloom filter.")]
        public ColorParameter tint = new ColorParameter(Color.white, false, false, true);

        [Tooltip("Use bicubic sampling instead of bilinear sampling for the upsampling passes. This is slightly more expensive but helps getting smoother visuals.")]
        public BoolParameter highQualityFiltering = new BoolParameter(false);

        [Tooltip("Dirtiness texture to add smudges or dust to the bloom effect.")]
        public TextureParameter dirtTexture = new TextureParameter(null);

        [Tooltip("Amount of dirtiness.")]
        public MinFloatParameter dirtIntensity = new MinFloatParameter(0f, 0f);

        [Tooltip("Controls the maximum number of blur iterations. Lowering this value will help with performance at a cost of visual fidelity.")]
        public ClampedIntParameter maxIterations = new ClampedIntParameter(16, 2, 16);

        [Tooltip("The starting resolution when performing the bloom. Lower this to gain performance and lower memory usage.")]
        public BloomDownsampleParameter downsample = new BloomDownsampleParameter(BloomDownsample.Half, false);

        public bool IsActive() => intensity.value > 0f;

        public bool IsTileCompatible() => false;
    }

    [Serializable]
    public sealed class BloomDownsampleParameter : VolumeParameter<BloomDownsample> { public BloomDownsampleParameter(BloomDownsample value, bool overrideState = false) : base(value, overrideState) { } }
}
