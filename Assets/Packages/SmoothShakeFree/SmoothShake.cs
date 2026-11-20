
using UnityEngine;

namespace SmoothShakeFree
{
    [AddComponentMenu("Smooth Shake Free/Smooth Shake Free")]
    public class SmoothShake : ShakeBase
    {
        public SmoothShakeFreePreset preset;

        [Header("Position Shake Settings")]
        public Shaker positionShake;

        [Header("Rotation Shake Settings")]
        public Shaker rotationShake;

        [HideInInspector] internal Vector3 startPosition;
        [HideInInspector] internal Vector3 startRotation;

        private Vector3 lastPositionOffset;
        private Vector3 lastRotationOffset;

        internal sealed override void Apply(Vector3[] value)
        {
            // Убираем предыдущую тряску
            transform.localPosition -= lastPositionOffset;
            transform.localEulerAngles -= lastRotationOffset;

            // Сохраняем новую тряску
            lastPositionOffset = value[0];
            lastRotationOffset = value[1];

            // Накладываем новую тряску на текущую позицию
            transform.localPosition += lastPositionOffset;
            transform.localEulerAngles += lastRotationOffset;
        }

        protected override Shaker[] GetShakers()
        {
            return new Shaker[] { positionShake, rotationShake };
        }

        internal override void ResetDefaultValues()
        {
            transform.localPosition -= lastPositionOffset;
            transform.localEulerAngles -= lastRotationOffset;

            lastPositionOffset = Vector3.zero;
            lastRotationOffset = Vector3.zero;
        }

        internal sealed override void SaveDefaultValues()
        {
            startPosition = transform.localPosition;
            startRotation = transform.localEulerAngles;

            lastPositionOffset = Vector3.zero;
            lastRotationOffset = Vector3.zero;
        }

        internal sealed override void ApplyPresetSettings(SmoothShakeFreePreset preset)
        {
            positionShake.noiseType = preset.positionShake.noiseType;
            positionShake.amplitude = preset.positionShake.amplitude;
            positionShake.frequency = preset.positionShake.frequency;

            rotationShake.noiseType = preset.rotationShake.noiseType;
            rotationShake.amplitude = preset.rotationShake.amplitude;
            rotationShake.frequency = preset.rotationShake.frequency;

            timeSettings.enableOnStart = preset.timeSettings.enableOnStart;
            timeSettings.constantShake = preset.timeSettings.constantShake;
            timeSettings.fadeInDuration = preset.timeSettings.fadeInDuration;
            timeSettings.fadeOutDuration = preset.timeSettings.fadeOutDuration;
            timeSettings.fadeInCurve = preset.timeSettings.fadeInCurve;
            timeSettings.fadeOutCurve = preset.timeSettings.fadeOutCurve;
            timeSettings.holdDuration = preset.timeSettings.holdDuration;
        }
    }
}
