using UnityEngine;
using System.Collections.Generic;

namespace Character
{
    public class ChangeColorPlayer : MonoBehaviour
    {
        [Header("Setting")] 
        [SerializeField] private Material material;
        [SerializeField] private TrailRenderer trailLeft;
        [SerializeField] private TrailRenderer trailRight;

        private GameObject currentParticle;
        private string currentSkin;

        [Header("Season Particles & Texture Offsets")]
        [SerializeField] private GameObject particleSpring;
        [SerializeField] private Vector2 positionSpringMask = new Vector2(0.1f, 0.5f);

        [SerializeField] private GameObject particleSummer;
        [SerializeField] private Vector2 positionSummerMask = new Vector2(0.2f, 0f);

        [SerializeField] private GameObject particleAutumn;
        [SerializeField] private Vector2 positionAutumnMask = new Vector2(0, 0.5f);

        [SerializeField] private GameObject particleWinter;
        [SerializeField] private Vector2 positionWinterMask = new Vector2(0.3f, -0.02f);

        private Dictionary<string, System.Action> seasonActions;

        private void Awake()
        {
            seasonActions = new Dictionary<string, System.Action>
            {
                { "Sp", () => SetSeasonState(particleSpring, positionSpringMask, new Color(1f, 1f, 0.6f), new Color(0.6f, 1f, 0.6f)) },
                { "Su", () => SetSeasonState(particleSummer, positionSummerMask, new Color(1f, 0.2f, 0.6f), new Color(0.6f, 0.3f, 1f)) },
                { "Au", () => SetSeasonState(particleAutumn, positionAutumnMask, new Color(0.6f, 0.2f, 0.3f), new Color(0.5f, 0.3f, 0.2f)) },
                { "Wi", () => SetSeasonState(particleWinter, positionWinterMask, new Color(0.6f, 0.8f, 1f), Color.white) }
            };
        }

        public void ChangeColor(string nameParent)
        {
            string seasonKey = nameParent.Substring(0, 2);
            if (currentSkin == seasonKey) return;

            currentSkin = seasonKey;
            if (seasonActions.ContainsKey(seasonKey))
            {
                seasonActions[seasonKey].Invoke();
            }
        }

        private void SetSeasonState(GameObject newParticle, Vector2 textureOffset, Color startColor, Color endColor)
        {
            material.mainTextureOffset = textureOffset;

            if (currentParticle != null) 
                currentParticle.SetActive(false);

            currentParticle = newParticle;
            currentParticle.SetActive(true);

            Gradient gradient = CreateGradient(startColor, endColor);
            trailLeft.colorGradient = gradient;
            trailRight.colorGradient = gradient;
        }

        private Gradient CreateGradient(Color startColor, Color endColor)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(startColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            return gradient;
        }
    }
}
