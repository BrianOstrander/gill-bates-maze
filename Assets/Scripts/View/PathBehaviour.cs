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
        
        public void Initialize(Node node)
        {
            pathRenderer.sharedMaterial = new Material(pathRenderer.sharedMaterial);
            
            node.CheesePowerUpdate += OnCheesePowerUpdate;
            OnCheesePowerUpdate(node.CheesePower);
        }

        void OnCheesePowerUpdate(
            int cheesePower
        )
        {
            var normal = cheesePower / (float)PersistantData.CheesePower.Value;
            
            pathRenderer.sharedMaterial.color = cheesePowerGradient.Evaluate(
                normal
            );
        }
    }
}