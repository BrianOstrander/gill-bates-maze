using GillBates.Data;
using UnityEngine;

namespace GillBates.View
{
    public class PathBehaviour : MonoBehaviour
    {
        [SerializeField]
        Renderer pathRenderer;

        [SerializeField]
        Gradient cheesePowerGradient;
        
        int cheesePowerMax;
        
        
        public void Initialize(
            Node node,
            int cheesePower,
            int cheesePowerMax
        )
        {
            this.cheesePowerMax = cheesePowerMax;

            pathRenderer.sharedMaterial = new Material(pathRenderer.sharedMaterial);
            
            node.CheesePowerUpdate += OnCheesePowerUpdate;
            OnCheesePowerUpdate(cheesePower);
        }

        void OnCheesePowerUpdate(
            int cheesePower
        )
        {
            var normal = cheesePower / (float)cheesePowerMax;
            
            pathRenderer.sharedMaterial.color = cheesePowerGradient.Evaluate(
                normal
            );
        }
    }
}