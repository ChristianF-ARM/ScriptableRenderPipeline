using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Rendering.Universal
{
    [VolumeComponentEditor(typeof(ScreenSpaceAmbientOcclusionVolume))]
    sealed class ScreenSpaceAmbientOcclusionVolumeEditor : VolumeComponentEditor
    {
        SerializedDataParameter m_Intensity;
        SerializedDataParameter m_Radius;
        SerializedDataParameter m_DownSample;
        SerializedDataParameter m_SampleCount;
        //SerializedDataParameter m_DepthSource;
        SerializedDataParameter m_NormalReconstructionQuality;

        public override void OnEnable()
        {
            PropertyFetcher<ScreenSpaceAmbientOcclusionVolume> volume = new PropertyFetcher<ScreenSpaceAmbientOcclusionVolume>(serializedObject);

            //m_DepthSource = Unpack(volume.Find(x => x.depthSource));
            m_NormalReconstructionQuality = Unpack(volume.Find(x => x.normalReconstructionQuality));
            m_Intensity                   = Unpack(volume.Find(x => x.intensity));
            m_Radius                      = Unpack(volume.Find(x => x.radius));
            m_DownSample                  = Unpack(volume.Find(x => x.downSample));
            m_SampleCount                 = Unpack(volume.Find(x => x.sampleCount));
        }

        public override void OnInspectorGUI()
        {
            //PropertyField(m_DepthSource);
            //if (m_DepthSource == DepthSource.Depth)
            {
                PropertyField(m_NormalReconstructionQuality);
            }
            PropertyField(m_DownSample);
            PropertyField(m_Intensity);
            PropertyField(m_Radius);
            PropertyField(m_SampleCount);
        }
    }
}
